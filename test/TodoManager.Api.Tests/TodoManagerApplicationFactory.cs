using System.Linq.Expressions;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoManager.Api.Data;
using TodoManager.Api.Model;
using TodoManager.Api.Repository;
using TodoManager.Api.Service;

namespace TodoManager.Api.Tests;

public class TodoManagerApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureTestServices(s => s
            .RemoveAll<ApplicationContext>()
            .RemoveAll<ITagRepository>()
            .RemoveAll<ITaskRepository>()
            .Replace(ServiceDescriptor.Scoped(typeof(IUnitOfWork),
                _ => GetUnitOfWork()))
            .Configure<TodoServiceOptions>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["MaxTaskDepth"] = "1"
                })
                .Build()
            )
        );
    }

    private static IUnitOfWork GetUnitOfWork()
    {
        var tasks = GetSeedingTasks();
        var tags = GetSeedingTags();

        var taskRepositoryMock = A.Fake<ITaskRepository>();
        var tagRepositoryMock = A.Fake<ITagRepository>();
        var unitOfWorkMock = A.Fake<IUnitOfWork>();

        A.CallTo(() => taskRepositoryMock.GetAllAsync()).Returns(tasks);
        A.CallTo(() => taskRepositoryMock.GetByIdAsync(A<int>.Ignored))
            .ReturnsLazily((int x) => tasks.SingleOrDefault(y => y.Id == x));

        A.CallTo(() => tagRepositoryMock.GetAllAsync()).Returns(tags);
        A.CallTo(() => tagRepositoryMock.GetByConditionAsync(
            A<Expression<Func<TodoTag, bool>>>.Ignored))
                .ReturnsLazily((Expression<Func<TodoTag, bool>> x) => 
                    tags.Where(x.Compile()).ToList());
        A.CallTo(() => tagRepositoryMock.GetByNameAsync(A<string>.Ignored))
            .ReturnsLazily((string x) => tags.SingleOrDefault(
                y => y.Name == x));

        A.CallTo(() => unitOfWorkMock.Tasks).Returns(taskRepositoryMock);
        A.CallTo(() => unitOfWorkMock.Tags).Returns(tagRepositoryMock);

        return unitOfWorkMock;
    }

    private static List<TodoTask> GetSeedingTasks()
    {
        return new List<TodoTask>
        { 
            new()
            {
                Id = 1,
                Level = 0,
                Name = "Name 1",
                Description = "Description 1",
                Created = DateTime.Now
            },
            new()
            {
                Id = 2,
                Level = 1,
                Name = "Name 2",
                Description = "Description 2",
                Created = DateTime.Now
            }
        };
    }

    private static List<TodoTag> GetSeedingTags()
    {
        return new List<TodoTag>
        {
            new()
            {
                Id = 1,
                Name = "Tag 1"
            },
            new()
            {
                Id = 2,
                Name = "Tag 2"
            }
        };
    }
}