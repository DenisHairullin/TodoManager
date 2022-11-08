namespace TodoManager.Api.Repository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection 
        services)
    {
        return services
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}