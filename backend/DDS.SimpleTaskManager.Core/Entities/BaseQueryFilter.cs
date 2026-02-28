using DDS.SimpleTaskManager.Core.Models.Pagination;

namespace DDS.SimpleTaskManager.Core.Entities;

public record BaseQueryFilter(
    int Page = 1,
    int PageSize = 10,
    bool? IsDescending = false,
    bool? IsActive = null)
    : PaginationParameters(Page, PageSize);