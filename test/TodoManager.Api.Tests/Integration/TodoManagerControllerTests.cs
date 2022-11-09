using System.Net;
using TodoManager.Api.Dto;

namespace TodoManager.Api.Tests.Integration;

[Parallelizable(ParallelScope.Children)]
public class TodoManagerControllerTest
{
    private readonly TodoManagerApplicationFactory _applicationFactory;

    public TodoManagerControllerTest()
    {
        _applicationFactory = new TodoManagerApplicationFactory();
    }

    [Test]
    public async Task GetAllTasksShouldOk()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "https://localhost/TodoManager/Tasks");

        var data = await response.Content.ReadFromJsonAsync<TodoTaskDto[]>();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(data, Is.Not.Null);
        Assert.That(data, Has.Length.EqualTo(2));
    }

    [Test]
    public async Task GetTaskByIdShouldOk()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "https://localhost/TodoManager/Tasks/1");

        var data = await response.Content.ReadFromJsonAsync<TodoTaskDto>();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(data, Is.Not.Null);
    }

    [Test]
    public async Task GetTaskByIdOfAbsentTaskShouldNotFound()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "https://localhost/TodoManager/Tasks/0");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task AddTaskShouldCreated()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1",
            TagNames = { "Tag 1", "Tag 2" }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddTaskShouldReturnTaskWithValidTags()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1",
            TagNames = { "Tag 1", "Tag 2" }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        var data = await response.Content.ReadFromJsonAsync<TodoTaskDto>();

        // Assert
        Assert.That(data?.TagNames.SequenceEqual(new[] { "Tag 1", "Tag 2" }), 
            Is.True);
    }

    [Test]
    public async Task AddTaskShouldReturnTaskWithValidLevels()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1",
            SubTasks = 
            {
                new TodoTaskDto
                {
                    Name = "New Task 2",
                    Description = "New Description 2"
                }
            }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        var data = await response.Content.ReadFromJsonAsync<TodoTaskDto>();

        // Assert
        Assert.That(data, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(data?.Level, Is.Zero);
            Assert.That(data?.SubTasks.FirstOrDefault()?.Level, Is.EqualTo(1));
        });
    }    

    [Test]
    public async Task AddTaskWithNullNameShouldBadRequest()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = null!,
            Description = "New Description 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddTaskWithAbsentTagShouldBadRequest()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Name 1",
            Description = "New Description 1",
            TagNames = { "Tag 3" }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddTaskWithAllowedDepthShouldCreated()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1",
            SubTasks = 
            {
                new TodoTaskDto
                {
                    Name = "New Task 2",
                    Description = "New Description 2"
                }
            }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddTaskWithNotAllowedDepthShouldBadRequest()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1",
            SubTasks = 
            {
                new TodoTaskDto
                {
                    Name = "New Task 2",
                    Description = "New Description 2",
                    SubTasks =
                    {
                        new TodoTaskDto
                        {
                            Name = "New Task 3",
                            Description = "New Description 3"
                        }
                    }
                }
            }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddSubTaskShouldCreated()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks/1/Subtasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddSubTaskShouldReturnTaskWithValidLevels()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks/1/Subtasks", 
            JsonContent.Create(task));

        var data = await response.Content.ReadFromJsonAsync<TodoTaskDto>();

        // Assert
        Assert.That(data, Is.Not.Null);
        Assert.That(data?.Level, Is.EqualTo(1));
    }

    [Test]
    public async Task AddSubTaskForAbsentTaskShouldNotFound()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks/0/Subtasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task AddSubTaskWithAllowedDepthShouldCreated()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks/1/Subtasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddSubTaskWithNotAllowedDepthShouldBadRequest()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var task = new TodoTaskDto
        {
            Name = "New Task 1",
            Description = "New Description 1",
            SubTasks = 
            {
                new TodoTaskDto
                {
                    Name = "New Task 2",
                    Description = "New Description 2"
                }
            }
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tasks/1/Subtasks", 
            JsonContent.Create(task));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }    

    [Test]
    public async Task DeleteTaskShouldOk()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.DeleteAsync(
            "https://localhost/TodoManager/Tasks/1");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteAbsentTaskShouldNotFound()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.DeleteAsync(
            "https://localhost/TodoManager/Tasks/0");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetAllTagsShouldOk()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "https://localhost/TodoManager/Tags");

        var data = await response.Content.ReadFromJsonAsync<TodoTagDto[]>();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(data, Is.Not.Null);
        Assert.That(data, Has.Length.EqualTo(2));
    }

    [Test]
    public async Task AddTagShouldOk()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var tag = new TodoTagDto
        {
            Name = "New Tag 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tags", 
            JsonContent.Create(tag));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task AddTagWithNullNameShouldBadRequest()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var tag = new TodoTagDto
        {
            Name = null!
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tags", 
            JsonContent.Create(tag));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddExistsTagShouldConflict()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();
        var tag = new TodoTagDto
        {
            Name = "Tag 1"
        };

        // Act
        var response = await client.PostAsync(
            "https://localhost/TodoManager/Tags", 
            JsonContent.Create(tag));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
}

