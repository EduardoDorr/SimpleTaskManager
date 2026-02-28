using DDS.SimpleTaskManager.Core.Entities;

namespace DDS.SimpleTaskManager.Core.Persistence.Repositories;

public interface IDeletableRepository<in TEntity> where TEntity : BaseEntity
{
    void Delete(TEntity entity);
}