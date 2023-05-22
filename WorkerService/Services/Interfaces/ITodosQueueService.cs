using WorkerService.DTOs;

namespace WorkerService.Services.Interfaces;

public interface ITodosQueueService
{
    public Task<TodoDto?> ReceiveAsync();
}