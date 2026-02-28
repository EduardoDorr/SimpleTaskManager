using DDS.SimpleTaskManager.Core.Results.FluentValidation;

using FluentValidation;

namespace DDS.SimpleTaskManager.Core.Models.Pagination;

public class PaginationParametersValidator<TPagination> : AbstractValidator<TPagination>
    where TPagination : PaginationParameters
{
    public PaginationParametersValidator()
    {
        RuleFor(p => p.Page)
            .GreaterThan(0)
            .WithError(PaginationError.PageMustBeGreaterThanZero);

        RuleFor(p => p.PageSize)
            .GreaterThan(0)
            .WithError(PaginationError.PageSizeMustBeGreaterThanZero)
            .LessThanOrEqualTo(1000)
            .WithError(PaginationError.PageSizeMustBeLessThanOrEqualTo1000);
    }
}