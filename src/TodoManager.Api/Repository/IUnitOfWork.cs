namespace TodoManager.Api.Repository;

public interface IUnitOfWork
{
    ITaskRepository Tasks { get; }
    ITagRepository Tags { get; }
    Task SaveChangesAsync();
}