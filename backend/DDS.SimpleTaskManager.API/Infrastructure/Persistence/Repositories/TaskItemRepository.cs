using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Models.Pagination;

using Microsoft.EntityFrameworkCore;

namespace DDS.SimpleTaskManager.API.Infrastructure.Persistence.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly TaskManagerDbContext _context;

    public TaskItemRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResult<TaskItem>> GetAllAsync(TaskItemQueryFilter queryFilter, CancellationToken cancellationToken = default)
    {
        var query = _context.TaskItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(queryFilter.Title))
            query = query.Where(ti => ti.Title.Contains(queryFilter.Title!));

        if (queryFilter.Status.HasValue)
            query = query.Where(ti => ti.Status.Equals(queryFilter.Status.Value));

        if (queryFilter.Priority.HasValue)
            query = query.Where(ti => ti.Priority.Equals(queryFilter.Priority.Value));

        if (queryFilter.IsActive.HasValue)
            query = query.Where(ti => ti.IsActive == queryFilter.IsActive.Value);

        query = queryFilter.IsDescending.HasValue && queryFilter.IsDescending.Value
            ? query.OrderByDescending(ti => ti.CreatedAt)
            : query.OrderBy(ti => ti.CreatedAt);

        return await query
            .GetPagedAsync(
                queryFilter.Page,
                queryFilter.PageSize,
                cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .FirstOrDefaultAsync(ti => ti.Id == id, cancellationToken);
    }

    public void Create(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
    }

    public void Update(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
    }
}