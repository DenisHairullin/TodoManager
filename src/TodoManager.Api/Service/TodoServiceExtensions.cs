namespace TodoManager.Api.Service;

public static class TodoServiceExtensions
{
    public static IServiceCollection AddTodoService(this IServiceCollection 
        services, IConfiguration configuration)
    {
        return services
            .Configure<TodoServiceOptions>(configuration
                .GetSection("TodoService"))
            .AddScoped<ITodoService, TodoService>();
    }
}