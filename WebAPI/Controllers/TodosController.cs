using Microsoft.AspNetCore.Mvc;
using RabbitMqLibrary.Services.Interfaces;
using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;

namespace Web.UI.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMessagePublisher<TodoDto> _messagePublisher;

    public TodosController(ITodoRepository todoRepository, IMessagePublisher<TodoDto> messagePublisher)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        
        _messagePublisher.QueueName = "Todos";
        _messagePublisher.ExchangeName = "Todos Exchange";
        _messagePublisher.RoutingKey = "Todos Routing Key";
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAllTodos()
    {
        var todoDtos = await _todoRepository.GetAllAsync();
        
        return Ok(todoDtos);
    }
    
    [HttpGet("{id}", Name = "GetTodo")]
    public async Task<ActionResult<TodoDto>> GetTodoById(string id)
    {
        var todoDto = await _todoRepository.GetAsync(id);

        if (todoDto is null)
        {
            return NotFound();
        }

        return Ok(todoDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo([FromBody] CreateTodoDto createTodoDto)
    {
        var todoDto = await _todoRepository.CreateAsync(createTodoDto);
        
        return CreatedAtRoute("GetTodo", new { id = todoDto.Id }, todoDto);
    }
    
    [HttpOptions("process")]
    public async Task<IActionResult> Process()
    {
        var todoDtos = await _todoRepository.GetUncompletedAsync();
        
        foreach (var todoDto in todoDtos)
        {
            _messagePublisher.Publish(todoDto);
            
            await _todoRepository.MarkAsCompletedAsync(todoDto.Id);
        }

        return NoContent();
    }
}