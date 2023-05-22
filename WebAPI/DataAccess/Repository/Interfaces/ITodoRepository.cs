using Web.UI.DTOs;

namespace Web.UI.DataAccess.Repository.Interfaces;

public interface ITodoRepository
{
    Task<TodoDto> CreateAsync(CreateTodoDto createTodoDto);
    Task<TodoDto?> GetAsync(string id);
    Task<IEnumerable<TodoDto>> GetAllAsync();
    Task DeleteAsync(string id);
}