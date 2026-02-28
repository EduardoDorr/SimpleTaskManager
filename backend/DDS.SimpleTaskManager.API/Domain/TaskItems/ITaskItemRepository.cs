using DDS.SimpleTaskManager.Core.Models.Pagination;
using DDS.SimpleTaskManager.Core.Persistence.Repositories;

namespace DDS.SimpleTaskManager.API.Domain.TaskItems;

public interface ITaskItemRepository
    : ICreatableRepository<TaskItem>,
      IUpdatableRepository<TaskItem>
{
    Task<PaginationResult<TaskItem>> GetAllAsync(TaskItemQueryFilter queryFilter, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}