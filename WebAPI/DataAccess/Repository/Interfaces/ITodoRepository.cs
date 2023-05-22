using Web.UI.DTOs;

namespace Web.UI.DataAccess.Repository.Interfaces;

public interface ITodoRepository
{
    Task<TodoDto> CreateAsync(CreateTodoDto dto);
    Task<TodoDto?> GetAsync(string id);
    Task<IEnumerable<TodoDto>> GetAllAsync();
}