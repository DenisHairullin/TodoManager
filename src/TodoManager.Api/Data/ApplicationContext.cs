using Microsoft.EntityFrameworkCore;
using TodoManager.Api.Model;

namespace TodoManager.Api.Data;

public class ApplicationContext : DbContext
{
    public DbSet<TodoTask> Tasks { get; set; } = null!;
    public DbSet<TodoTag> Tags { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoTask>()
            .HasOne(s => s.ParentTask)
            .WithMany(t => t.SubTasks)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TodoTask>()
            .Property(x => x.Created)
                .HasColumnType("Date");

        modelBuilder.Entity<TodoTag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        TodoTask t1 = new () { Id = 1, Name = "n1", Description = "d1", 
            Created = DateTime.Now, Level = 0 };
        TodoTask t2 = new () { Id = 2, Name = "n2", Description = "d2", 
            Created = DateTime.Now, ParentTaskId = 1, Level = 1 };
        TodoTask t3 = new () { Id = 3, Name = "n3", Description = "d3", 
            Created = DateTime.Now, ParentTaskId = 2, Level = 2 };
        TodoTask t4 = new () { Id = 4, Name = "n4", Description = "d4", 
            Created = DateTime.Now, ParentTaskId = 3, Level = 3 };

        TodoTag g1 = new () { Id = 1, Name = "Tag1" };
        TodoTag g2 = new () { Id = 2, Name = "Tag2" };

        modelBuilder.Entity<TodoTask>()
            .HasData(t1, t2, t3, t4);

        modelBuilder.Entity<TodoTag>()
            .HasData(g1, g2);

        modelBuilder.Entity<TodoTask>()
            .HasMany(t => t.Tags)
            .WithMany(t => t.Tasks)
            .UsingEntity(e =>
                e.HasData(
                    new { TagsId = 1, TasksId = 1 },
                    new { TagsId = 2, TasksId = 1 },
                    new { TagsId = 2, TasksId = 2 },
                    new { TagsId = 1, TasksId = 3 },
                    new { TagsId = 2, TasksId = 4 }
                ));
    }
}