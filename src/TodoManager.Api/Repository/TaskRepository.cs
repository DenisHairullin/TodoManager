using Microsoft.EntityFrameworkCore;
using TodoManager.Api.Model;
using TodoManager.Api.Data;

namespace TodoManager.Api.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationContext _applicationContext;

    public TaskRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IEnumerable<TodoTask>> GetAllAsync()
    {
        await _applicationContext.Tasks.Include(x => x.Tags).LoadAsync();
        return await _applicationContext.Tasks.Where(x => x.ParentTask == null)
            .ToListAsync();
    }

    public async Task<TodoTask?> GetByIdAsync(int id)
    {
        var task = await _applicationContext.Tasks.FindAsync(id);
        return await LoadTaskAsync(task);
    }

    public void Add(TodoTask task, TodoTask? parent = null)
    {
        if (parent is null)
        {
            _applicationContext.Tasks.Add(task);
        }
        else
        {
            parent.SubTasks.Add(task);
        }
    }

    public void Delete(TodoTask task)
    {
        _applicationContext.Tasks.Remove(task);
    }

    private async Task<TodoTask?> LoadTaskAsync(TodoTask? task)
    {
        if (task is not null) {
            await LoadTasksRecursive(task);
        }

        return task;

        async Task LoadTasksRecursive(TodoTask recursiveTask)
        {
            await _applicationContext.Entry(recursiveTask)
                .Collection(t => t.SubTasks)
                .Query()
                .Include(x => x.Tags)
                .LoadAsync();

            foreach (var t in recursiveTask.SubTasks)
            {
                await LoadTasksRecursive(t);
            }
        }
    }
}