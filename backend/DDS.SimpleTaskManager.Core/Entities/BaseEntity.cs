namespace DDS.SimpleTaskManager.Core.Entities;

public abstract class BaseEntity
{
    public long Id { get; protected set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; }
    public bool IsDeleted { get; protected set; }

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        IsDeleted = false;
    }

    public virtual void Activate()
        => IsActive = true;

    public virtual void Deactivate()
        => IsActive = false;

    public virtual void SetUpdatedAtDate(DateTime updatedAtDate)
        => UpdatedAt = updatedAtDate;

    public void Delete()
        => IsDeleted = true;
}