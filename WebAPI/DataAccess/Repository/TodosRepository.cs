using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;

namespace Web.UI.Services;

public class TodosRepository : ITodoRepository
{
    private readonly List<TodoDto> _todoDtos = new();
    
    public Task<TodoDto> CreateAsync(CreateTodoDto createTodoDto)
    {
        var id = Guid.NewGuid().ToString();
        var todoDto = new TodoDto(id, createTodoDto.Title);
        
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

    public Task DeleteAsync(string id)
    {
        var todoDto = _todoDtos.FirstOrDefault(x => x.Id == id);
        
        if (todoDto is not null)
        {
            _todoDtos.Remove(todoDto);
        }
        
        return Task.CompletedTask;
    }
}