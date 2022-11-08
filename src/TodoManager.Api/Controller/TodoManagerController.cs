using Microsoft.AspNetCore.Mvc;
using TodoManager.Api.Dto;
using TodoManager.Api.Service;

namespace TodoManager.Api.Controller;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class TodoManagerController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoManagerController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    /// Returns all tasks
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Task list</response>
    [HttpGet("Tasks")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _todoService.GetAllTasksAsync();
        var result = tasks.AsDto();

        return Ok(result);
    }

    /// <summary>
    /// Returns task with specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Task with specified id</response>
    /// <response code="404">If task not found</response>
    [HttpGet("Tasks/{id:int}")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), 
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _todoService.GetTaskByIdAsync(id);

        return task is null ? NotFound() : Ok(task.AsDto());
    }

    /// <summary>
    /// Creates new task
    /// </summary>
    /// <param name="taskDto"></param>
    /// <returns></returns>
    /// <response code="201">Created task</response>
    /// <response code="400">If task validation failed</response>
    [HttpPost("Tasks")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 
        StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTask(TodoTaskDto taskDto)
    {
        var task = await _todoService.AddTaskAsync(taskDto.AsTask());

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, 
            task.AsDto());
    }

    /// <summary>
    /// Creates new subtask for specified task
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subTaskDto"></param>
    /// <returns></returns>
    /// <response code="201">Created subtask</response>
    /// <response code="400">If subtask validation failed</response>
    /// <response code="404">If task not found</response>
    [HttpPost("Tasks/{id:int}/Subtasks")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), 
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSubTask(int id, TodoTaskDto subTaskDto)
    {
        var parent = await _todoService.GetTaskByIdAsync(id);

        if (parent is null)
        {
            return NotFound();
        }

        var task = await _todoService.AddTaskAsync(subTaskDto.AsTask(), 
            parent);

        return CreatedAtAction(nameof(GetTaskById), 
            new { id = task.Id }, task.AsDto());
    }

    /// <summary>
    /// Deletes task
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">On task deletion completed</response>
    /// <response code="404">If task not found</response>
    [HttpDelete("Tasks/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), 
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _todoService.GetTaskByIdAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        await _todoService.DeleteTaskAsync(task);
        return Ok();
    }

    /// <summary>
    /// Returns all tags
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Tag list</response>
    [HttpGet("Tags")]
    [ProducesResponseType(typeof(TodoTagDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _todoService.GetAllTagsAsync();
        return Ok(tags.AsDto());
    }

    /// <summary>
    /// Creates new tag
    /// </summary>
    /// <param name="tagDto"></param>
    /// <returns></returns>
    /// <response code="200">On tag creation completed</response>
    /// <response code="400">If tag validation failed</response>
    /// <response code="409">If tag exists</response>
    [HttpPost("Tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTag(TodoTagDto tagDto)
    {
        if (await _todoService.GetTagByNameAsync(tagDto.Name) is not null)
        {
            return Conflict();
        }

        await _todoService.AddTagAsync(tagDto.AsTag());
        
        return Ok();
    }
}