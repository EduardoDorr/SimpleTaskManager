using DDS.SimpleTaskManager.Core.Entities;
using DDS.SimpleTaskManager.Core.Models.Pagination;

namespace DDS.SimpleTaskManager.Core.Persistence.Repositories;

public interface IReadableRepository<TEntity> where TEntity : BaseEntity
{
    Task<PaginationResult<TEntity>> GetAllAsync(BaseQueryFilter queryFilter, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}