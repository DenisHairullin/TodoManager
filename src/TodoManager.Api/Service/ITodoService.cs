using TodoManager.Api.Model;

namespace TodoManager.Api.Service;

public interface ITodoService
{
    Task<IEnumerable<TodoTask>> GetAllTasksAsync();
    Task<TodoTask?> GetTaskByIdAsync(int id);
    Task<TodoTask> AddTaskAsync(TodoTask task, TodoTask? parent = null);
    Task DeleteTaskAsync(TodoTask task);
    Task<IEnumerable<TodoTag>> GetAllTagsAsync();
    Task<TodoTag?> GetTagByNameAsync(string name);
    Task<TodoTag> AddTagAsync(TodoTag tag);
}