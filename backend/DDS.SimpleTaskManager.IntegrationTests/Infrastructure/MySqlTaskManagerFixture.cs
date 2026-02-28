
using DDS.SimpleTaskManager.API.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

using Testcontainers.MySql;

namespace DDS.SimpleTaskManager.IntegrationTests.Infrastructure;

public sealed class MySqlTaskManagerFixture : IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder("mysql:8.0.41")
        .WithDatabase("TaskManagerDb")
        .WithUsername("admin")
        .WithPassword("Admin123?")
        .Build();

    public TaskManagerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseMySQL(_mySqlContainer.GetConnectionString())
            .Options;

        return new TaskManagerDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.TaskItems.IgnoreQueryFilters().ExecuteDeleteAsync();
    }

    public async ValueTask InitializeAsync()
    {
        await _mySqlContainer.StartAsync();

        await using var dbContext = CreateDbContext();
        await dbContext.Database.MigrateAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _mySqlContainer.DisposeAsync();
    }
}