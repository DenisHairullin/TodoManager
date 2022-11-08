using System.Linq.Expressions;
using TodoManager.Api.Model;
using TodoManager.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoManager.Api.Repository;

public class TagRepository : ITagRepository
{
    private readonly ApplicationContext _applicationContext;

    public TagRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IEnumerable<TodoTag>> GetAllAsync()
    {
        return await _applicationContext.Tags.ToListAsync();
    }

    public async Task<TodoTag?> GetByNameAsync(string name)
    {
        return await _applicationContext.Tags.Where(x => x.Name == name)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<TodoTag>> GetByConditionAsync(
        Expression<Func<TodoTag, bool>> condition)
    {
        return await _applicationContext.Tags.Where(condition).ToListAsync();
    }

    public void Add(TodoTag tag)
    {
        _applicationContext.Tags.Add(tag);
    }
}