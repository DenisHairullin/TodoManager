using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TodoManager.Api.Dto;

public class TodoTagDto
{
    /// <summary>
    /// Tag identifier
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }
    
    /// <summary>
    /// Tag name
    /// </summary>
    /// <example>Some name</example>
    [Required]
    public string Name { get; set; } = null!;
}