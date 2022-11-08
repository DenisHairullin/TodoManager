using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoManager.Api.Data;
public class ApplicationContextFactory : 
    IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        return new ApplicationContext(
            new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(
                    new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build()
                        .GetConnectionString("Default")
                )
                .Options
        );
    }
}