using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;

namespace Web.UI.Services;

public class TodosRepository : ITodoRepository
{
    private readonly List<TodoDto> _todos = new();
    
    public Task<TodoDto> CreateAsync(CreateTodoDto dto)
    {
        var id = Guid.NewGuid().ToString();
        var todoDto = new TodoDto(id, dto.Title, false);
        
        _todos.Add(todoDto);
        
        return Task.FromResult(todoDto);
    }

    public Task<TodoDto?> GetAsync(string id)
    {
        return Task.FromResult(_todos.FirstOrDefault(x => x.Id == id));
    }

    public Task<IEnumerable<TodoDto>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TodoDto>>(_todos);
    }
}