using TodoManager.Api.Model;

namespace TodoManager.Api.Repository;

public interface ITaskRepository
{
    Task<IEnumerable<TodoTask>> GetAllAsync();
    Task<TodoTask?> GetByIdAsync(int id);
    void Add(TodoTask task, TodoTask? parent = null);
    void Delete(TodoTask task);
}