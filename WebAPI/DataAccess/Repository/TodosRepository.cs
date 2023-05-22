using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;

namespace Web.UI.Services;

public class TodosRepository : ITodoRepository
{
    private readonly List<TodoDto> _todoDtos = new();
    
    public Task<TodoDto> CreateAsync(CreateTodoDto createTodoDto)
    {
        var id = Guid.NewGuid().ToString();
        var todoDto = new TodoDto 
        { 
            Id = id, 
            Title = createTodoDto.Title
        };
        
        _todoDtos.Add(todoDto);
        
        return Task.FromResult(todoDto);
    }

    public Task<TodoDto?> GetAsync(string id)
    {
        return Task.FromResult(_todoDtos.FirstOrDefault(x => x.Id == id));
    }

    public Task<IEnumerable<TodoDto>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TodoDto>>(_todoDtos);
    }
    
    public Task<IEnumerable<TodoDto>> GetUncompletedAsync()
    {
        return Task.FromResult<IEnumerable<TodoDto>>(_todoDtos.Where(x => !x.IsCompleted));
    }

    public Task MarkAsCompletedAsync(string id)
    {
        var todoDto = _todoDtos.FirstOrDefault(x => x.Id == id);
        
        if (todoDto is not null)
        {
            todoDto.IsCompleted = true;
        }
        
        return Task.CompletedTask;
    }
}