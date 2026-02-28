using DDS.SimpleTaskManager.Core.Entities;
using DDS.SimpleTaskManager.Core.Enums;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Domain.TaskItems;

public sealed class TaskItem : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TaskItemStatus Status { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime DueDate { get; private set; }

    private TaskItem() { }

    private TaskItem(
        string title,
        string? description,
        Priority priority,
        DateTime dueDate) : this()
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        Status = TaskItemStatus.Backlog;
    }

    public static Result<TaskItem> Create(
        string title,
        string? description,
        Priority priority,
        DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Fail<TaskItem>(TaskItemError.TitleIsRequired);
            
        if (title.Length > TaskItemConstraints.TitleMaxLength)
            return Result.Fail<TaskItem>(TaskItemError.TitleIsTooLong);

        if (description?.Length > TaskItemConstraints.DescriptionMaxLength)
            return Result.Fail<TaskItem>(TaskItemError.DescriptionIsTooLong);

        if (!Enum.IsDefined(priority))
            return Result.Fail<TaskItem>(TaskItemError.PriorityIsInvalid(priority.ToString()));

        if (dueDate.Date < DateTime.UtcNow.Date)
            return Result.Fail<TaskItem>(TaskItemError.DueDateIsInvalid);

        var taskItem =
            new TaskItem(
                title.Trim(),
                description?.Trim(),
                priority,
                dueDate);

        return Result.Ok(taskItem);
    }

    public void ToggleCompletion()
    {
        if (Status == TaskItemStatus.Backlog)
            Status = TaskItemStatus.Completed;
        else
            Status = TaskItemStatus.Backlog;
    }
}