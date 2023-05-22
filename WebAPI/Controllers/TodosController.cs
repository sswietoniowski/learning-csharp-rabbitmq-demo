using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;
using Web.UI.Services.Interfaces;

namespace Web.UI.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly ITodosQueueService _todosQueueService;

    public TodosController(ITodoRepository todoRepository, ITodosQueueService todosQueueService)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        _todosQueueService = todosQueueService ?? throw new ArgumentNullException(nameof(todosQueueService));
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
        var todoDtos = await _todoRepository.GetAllAsync();
        
        foreach (var todoDto in todoDtos)
        {
            await _todosQueueService.SendAsync(todoDto);
            
            await _todoRepository.DeleteAsync(todoDto.Id);
        }

        return NoContent();
    }
}