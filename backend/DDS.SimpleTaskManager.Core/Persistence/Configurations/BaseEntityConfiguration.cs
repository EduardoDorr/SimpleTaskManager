using DDS.SimpleTaskManager.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDS.SimpleTaskManager.Core.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.IsActive)
               .IsRequired();

        builder.HasIndex(b => b.IsActive);

        builder.Property(b => b.IsDeleted)
               .IsRequired();

        builder.HasIndex(b => b.IsDeleted);

        builder.HasQueryFilter(b => !b.IsDeleted);

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.HasIndex(b => b.CreatedAt);

        builder.Property(b => b.UpdatedAt);
    }
}