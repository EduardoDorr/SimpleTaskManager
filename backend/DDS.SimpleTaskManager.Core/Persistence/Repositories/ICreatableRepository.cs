using DDS.SimpleTaskManager.Core.Entities;

namespace DDS.SimpleTaskManager.Core.Persistence.Repositories;

public interface ICreatableRepository<in TEntity> where TEntity : BaseEntity
{
    void Create(TEntity entity);
}