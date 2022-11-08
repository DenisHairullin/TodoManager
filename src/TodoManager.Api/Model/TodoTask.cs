namespace TodoManager.Api.Model;

public class TodoTask
{
    public int Id { get; set; }

    public int Level { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Created { get; set; }

    public int? ParentTaskId { get; set; }

    public TodoTask? ParentTask { get; set; }

    public List<TodoTask> SubTasks { get; set; } = new();

    public List<TodoTag> Tags { get; set; } = new();
}