using TodoManager.Api.Data;

namespace TodoManager.Api.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _applicationContext;
    public ITaskRepository Tasks { get; }
    public ITagRepository Tags { get; }

    public UnitOfWork(ApplicationContext applicationContext, 
        ITaskRepository taskRepository, ITagRepository tagRepository)
    {
        _applicationContext = applicationContext;
        Tasks = taskRepository;
        Tags = tagRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _applicationContext.SaveChangesAsync();
    }
}