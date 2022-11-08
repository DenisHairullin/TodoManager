using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TodoManager.Api.Dto;

public class TodoTaskDto
{
    /// <summary>
    /// Task identifier
    /// </summary>
    /// <example>1</example>
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }

    /// <summary>
    /// Task nesting level
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public int Level { get; set; }

    /// <summary>
    /// Task name
    /// </summary>
    /// <example>Some task</example>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Task description
    /// </summary>
    /// <example>Some description</example>
    public string? Description { get; set; }

    /// <summary>
    /// Task creation date
    /// </summary>
    [SwaggerSchema(ReadOnly = true, Format = "date")]
    public string? Created { get; set; }

    /// <summary>
    /// List of subtasks
    /// </summary>
    /// <example>"[]"</example>
    [Required]
    public List<TodoTaskDto> SubTasks { get; set; } = new();
    
    /// <summary>
    /// List of tags
    /// </summary>
    /// <example>"[]"</example>
    [Required]
    public List<string> TagNames { get; set; } = new();
}