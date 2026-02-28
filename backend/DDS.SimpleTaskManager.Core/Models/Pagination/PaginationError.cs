using DDS.SimpleTaskManager.Core.Results.Errors;

namespace DDS.SimpleTaskManager.Core.Models.Pagination;

public static class PaginationError
{
    public static Error PageMustBeGreaterThanZero =>
        new("Pagination.PageMustBeGreaterThanZero", "Page must be greater than zero.", ErrorType.Validation);

    public static Error PageSizeMustBeGreaterThanZero =>
        new("Pagination.PageSizeMustBeGreaterThanZero", "Page size must be greater than zero.", ErrorType.Validation);

    public static Error PageSizeMustBeLessThanOrEqualTo1000 =>
        new("Pagination.PageSizeMustBeLessThanOrEqualTo1000", "Page size must be less than or equal to 1000.", ErrorType.Validation);
}