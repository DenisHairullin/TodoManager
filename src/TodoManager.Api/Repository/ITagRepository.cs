using System.Linq.Expressions;
using TodoManager.Api.Model;

namespace TodoManager.Api.Repository;

public interface ITagRepository
{
    Task<IEnumerable<TodoTag>> GetAllAsync();
    Task<TodoTag?> GetByNameAsync(string name);
    Task<IEnumerable<TodoTag>> GetByConditionAsync(
        Expression<Func<TodoTag, bool>> condition);
    void Add(TodoTag tag);
}