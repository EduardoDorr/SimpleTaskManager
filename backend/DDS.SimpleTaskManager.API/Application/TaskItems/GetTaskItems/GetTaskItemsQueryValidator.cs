using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Models.Pagination;

using FluentValidation;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;

public class GetTaskItemsQueryValidator : AbstractValidator<GetTaskItemsQuery>
{
    public GetTaskItemsQueryValidator()
    {
        RuleFor(r => r.QueryFilter)
            .SetValidator(new PaginationParametersValidator<TaskItemQueryFilter>());
    }
}