using DDS.SimpleTaskManager.Core.Entities;

namespace DDS.SimpleTaskManager.Core.Persistence.Repositories;

public interface IUpdatableRepository<in TEntity> where TEntity : BaseEntity
{
    void Update(TEntity entity);
}