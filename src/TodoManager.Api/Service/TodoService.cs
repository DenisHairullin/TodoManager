using Microsoft.Extensions.Options;
using TodoManager.Api.Model;
using TodoManager.Api.Repository;

namespace TodoManager.Api.Service;

public class TodoService : ITodoService
{
    private readonly int _maxTaskDepth;
    private readonly IUnitOfWork _unitOfWork;

    public TodoService(IUnitOfWork unitOfWork, IOptions<TodoServiceOptions> 
        options)
    {
        _unitOfWork = unitOfWork;
        _maxTaskDepth = options.Value.MaxTaskDepth;
    }

    public async Task<IEnumerable<TodoTask>> GetAllTasksAsync()
    {
        return await _unitOfWork.Tasks.GetAllAsync();
    }

    public async Task<TodoTask?> GetTaskByIdAsync(int id)
    {
        return await _unitOfWork.Tasks.GetByIdAsync(id);
    }

    public async Task<TodoTask> AddTaskAsync(TodoTask task, TodoTask? parent 
        = null)
    {
        var taskDepth = GetTaskDepth(task) + (parent?.Level + 1 ?? 0);

        if (taskDepth > _maxTaskDepth)
        {
            throw new TodoValidationException(
                $"Task depth violation ({_maxTaskDepth}): " + taskDepth);
        }

        var taskTags = GetTaskTags(task).ToList();
        var taskTagsNames = taskTags.Select(x => x.Name).Distinct().ToList();
        var existsTags = (await _unitOfWork.Tags.GetByConditionAsync(
            x => taskTagsNames.Contains(x.Name))).ToList();
        var absentTags = taskTags.ExceptBy(existsTags.Select(x => x.Name), 
            x => x.Name).ToList();

        if (absentTags.Any()) {
            throw new TodoValidationException
            {
                Errors =
                {
                    ["Tags"] = absentTags.Select(x => "Not found: " + x.Name)
                        .ToArray()
                }
            };
        }

        SetTaskLevels(task, parent?.Level + 1 ?? 0);
        SetTaskTags(task, existsTags);
        SetTaskCreated(task, DateTime.Now);

        _unitOfWork.Tasks.Add(task, parent);
        await _unitOfWork.SaveChangesAsync();

        return task;
    }

    public async Task DeleteTaskAsync(TodoTask task)
    {
        _unitOfWork.Tasks.Delete(task);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TodoTag>> GetAllTagsAsync()
    {
        return await _unitOfWork.Tags.GetAllAsync();
    }

    public async Task<TodoTag?> GetTagByNameAsync(string name)
    {
        return await _unitOfWork.Tags.GetByNameAsync(name);
    }

    public async Task<TodoTag> AddTagAsync(TodoTag tag)
    {
        _unitOfWork.Tags.Add(tag);
        await _unitOfWork.SaveChangesAsync();

        return tag;
    }

    private static void ProcessTaskRecursive(TodoTask task, 
        Action<TodoTask> action)
    {
        action(task);
        task.SubTasks.ForEach(x => ProcessTaskRecursive(x, action));
    }

    private static IEnumerable<TodoTag> GetTaskTags(TodoTask task)
    {
        IEnumerable<TodoTag> tags = new List<TodoTag>();
        ProcessTaskRecursive(task, x => tags = tags.Concat(x.Tags));

        return tags;
    }

    private static int GetTaskDepth(TodoTask? task)
    {
        return task is null ? 0 : GetTaskDepthRecursive(task);

        int GetTaskDepthRecursive(TodoTask recursiveTask)
        {
            var t = recursiveTask.SubTasks.FirstOrDefault(); 
            return t is null ? 0 : GetTaskDepthRecursive(t) + 1;
        }
    }

    private static void SetTaskLevels(TodoTask task, int start = 0)
    {
        SetTaskLevelsRecursive(task, start);

        void SetTaskLevelsRecursive(TodoTask recursiveTask, int level)
        {
            recursiveTask.Level = level;
            recursiveTask.SubTasks.ForEach(x => SetTaskLevelsRecursive(x, 
                level + 1));
        }
    }    
    
    private static void SetTaskTags(TodoTask task, IEnumerable<TodoTag> tags)
    {
        ProcessTaskRecursive(task, x => x.Tags = tags.IntersectBy(
            x.Tags.Select(y => y.Name), z => z.Name
        ).ToList());
    }

    private static void SetTaskCreated(TodoTask task, DateTime date)
    {
        ProcessTaskRecursive(task, x => x.Created = date);
    }
}