using DDS.SimpleTaskManager.Core.Entities;
using DDS.SimpleTaskManager.Core.Enums;

namespace DDS.SimpleTaskManager.API.Domain.TaskItems;

public sealed record TaskItemQueryFilter(
    int Page = 1,
    int PageSize = 10,
    string? Title = null,
    TaskItemStatus? Status = null,
    Priority? Priority = null,
    bool? IsDescending = false,
    bool? IsActive = null)
    : BaseQueryFilter(Page, PageSize, IsDescending, IsActive);