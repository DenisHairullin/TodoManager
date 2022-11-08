using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using TodoManager.Api.Model;
using TodoManager.Api.Dto;
using TodoManager.Api.Service;

namespace TodoManager.Api.Controller;

public static class TodoManagerControllerExtensions
{
    public static IEnumerable<TodoTaskDto> AsDto(
        this IEnumerable<TodoTask> tasks)
    {
        return tasks.Select(x => new TodoTaskDto
        { 
            Id = x.Id,
            Level = x.Level,
            Name = x.Name, 
            Description = x.Description, 
            Created = x.Created.ToShortDateString(), 
            SubTasks = x.SubTasks.AsDto().ToList(),
            TagNames = x.Tags.Select(y => y.Name).ToList()
        });
    }

    public static TodoTaskDto AsDto(this TodoTask task)
    {
        return new TodoTaskDto
        {
            Id = task.Id,
            Level = task.Level,
            Name = task.Name, 
            Description = task.Description, 
            Created = task.Created.ToShortDateString(), 
            SubTasks = task.SubTasks.AsDto().ToList(),
            TagNames = task.Tags.Select(x => x.Name).ToList()
        };
    }

    public static IEnumerable<TodoTask> AsTask(
        this IEnumerable<TodoTaskDto> dtos)
    {
        return dtos.Select(x => new TodoTask()
        {
            Name = x.Name,
            Description = x.Description, 
            SubTasks = x.SubTasks.AsTask().ToList(),
            Tags = x.TagNames.Select(y => new TodoTag { Name = y }).ToList()
        });
    }

    public static TodoTask AsTask(this TodoTaskDto dto)
    {
        return new TodoTask
        {
            Name = dto.Name,
            Description = dto.Description, 
            SubTasks = dto.SubTasks.AsTask().ToList(),
            Tags = dto.TagNames.Select(x => new TodoTag { Name = x }).ToList()
        };
    }

    public static IEnumerable<TodoTagDto> AsDto(this IEnumerable<TodoTag> tags)
    {
        return tags.Select(x => new TodoTagDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    public static TodoTagDto AsDto(this TodoTag tag)
    {
        return new TodoTagDto
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }

    public static IEnumerable<TodoTag> AsTag(this IEnumerable<TodoTagDto> dtos)
    {
        return dtos.Select(x => new TodoTag
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    public static TodoTag AsTag(this TodoTagDto dto)
    {
        return new TodoTag
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

    public static IServiceCollection AddTodoManagerProblemDetails(
        this IServiceCollection services)
    {
        return services
            .AddProblemDetails(o =>
            {
                o.IncludeExceptionDetails = (c, _) => c.RequestServices
                    .GetRequiredService<IHostEnvironment>().IsDevelopment();

                o.Map<TodoValidationException>(e =>
                {
                    var p = new ValidationProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Bad request",
                        Type = "https://tools.ietf.org/html/rfc7231#section-"
                            + "6.5.1",
                        Detail = e.Message
                    };

                    foreach (var error in e.Errors)
                    {
                        p.Errors[error.Key] = error.Value;
                    }

                    return p;
                });
            });
    }
}