using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;

namespace Web.UI.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TodosController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAllTodos()
    {
        var todos = await _todoRepository.GetAllAsync();
        
        return Ok(todos);
    }
    
    [HttpGet("{id}", Name = "GetTodo")]
    public async Task<ActionResult<TodoDto>> GetTodoById(string id)
    {
        var todo = await _todoRepository.GetAsync(id);

        if (todo is null)
        {
            return NotFound();
        }

        return Ok(todo);
    }
    
    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo([FromBody] CreateTodoDto dto)
    {
        var todo = await _todoRepository.CreateAsync(dto);
        
        return CreatedAtRoute("GetTodo", new { id = todo.Id }, todo);
    }
    
    [HttpOptions("process")]
    public Task<IActionResult> Process()
    {
        // TODO: Add to queue
        return Task.FromResult<IActionResult>(new OkResult());
    }
}