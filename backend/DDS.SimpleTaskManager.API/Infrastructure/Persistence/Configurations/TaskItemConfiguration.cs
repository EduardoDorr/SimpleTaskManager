using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Persistence.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDS.SimpleTaskManager.API.Infrastructure.Persistence.Configurations;

internal class TaskItemConfiguration : BaseEntityConfiguration<TaskItem>
{
    public override void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        base.Configure(builder);

        builder.Property(ti => ti.Title)
               .HasMaxLength(TaskItemConstraints.TitleMaxLength)
               .IsRequired();

        builder.Property(ti => ti.Description)
               .HasMaxLength(TaskItemConstraints.DescriptionMaxLength);

        builder.Property(ti => ti.Status)
               .HasConversion<string>()
               .HasMaxLength(TaskItemConstraints.EnumMaxLength)
               .IsRequired();

        builder.HasIndex(ti => ti.Status);

        builder.Property(ti => ti.Priority)
               .HasConversion<string>()
               .HasMaxLength(TaskItemConstraints.EnumMaxLength)
               .IsRequired();

        builder.HasIndex(ti => ti.Priority);

        builder.Property(ti => ti.DueDate)
               .IsRequired();
    }
}