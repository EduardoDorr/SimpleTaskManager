using DDS.SimpleTaskManager.API.Domain.TaskItems;

using Microsoft.EntityFrameworkCore;

namespace DDS.SimpleTaskManager.API.Infrastructure.Persistence;

public class TaskManagerDbContext : DbContext
{
    public const string Name = "TaskManagerDbConnection";
    public DbSet<TaskItem> TaskItems { get; set; }

    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerDbContext).Assembly);
    }
}