using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TodoManager.Api.Data;
using TodoManager.Api.Repository;
using TodoManager.Api.Service;
using TodoManager.Api.Controller;

// Configure application builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo Manager",
        Description = "Simple ToDo Manager"
    });

    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

    o.EnableAnnotations();
});
builder.Services.AddDbContext<ApplicationContext>(o => o
    .UseSqlite(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddRepositories();
builder.Services.AddTodoService(builder.Configuration);
builder.Services.AddTodoManagerProblemDetails();

builder.Host.UseSerilog((_, l) => l
    .ReadFrom.Configuration(builder.Configuration)
);

// Build, configure and start application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Drop database in development environment
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<ApplicationContext>();
        
        if (context is not null)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}

app.UseProblemDetails();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

// For tests
public partial class Program { }