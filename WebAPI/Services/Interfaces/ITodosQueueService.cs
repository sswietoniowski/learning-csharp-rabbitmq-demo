using Web.UI.DTOs;

namespace Web.UI.Services.Interfaces;

public interface ITodosQueueService
{
    Task SendAsync(TodoDto todoDto);
}