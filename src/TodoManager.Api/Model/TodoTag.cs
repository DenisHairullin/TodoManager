namespace TodoManager.Api.Model;

public class TodoTag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public List<TodoTask> Tasks { get; set; } = new();
}