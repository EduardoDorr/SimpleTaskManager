using DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;
using DDS.SimpleTaskManager.API.Domain.TaskItems;

using FluentAssertions;

namespace DDS.SimpleTaskManager.UnitTests.Application.TaskItems.GetTaskItems;

public class GetTaskItemsQueryValidatorTests
{
    private readonly GetTaskItemsQueryValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_ShouldFail_WhenPageIsLessThanOrEqualToZero()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var query = new GetTaskItemsQuery(new TaskItemQueryFilter(Page: 0, PageSize: 10));

        // Act
        var result = await _validator.ValidateAsync(query, cancellationToken);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorCode == "Pagination.PageMustBeGreaterThanZero");
    }

    [Fact]
    public async Task ValidateAsync_ShouldFail_WhenPageSizeIsLessThanOrEqualToZero()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var query = new GetTaskItemsQuery(new TaskItemQueryFilter(Page: 1, PageSize: 0));

        // Act
        var result = await _validator.ValidateAsync(query, cancellationToken);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorCode == "Pagination.PageSizeMustBeGreaterThanZero");
    }

    [Fact]
    public async Task ValidateAsync_ShouldFail_WhenPageSizeIsGreaterThan1000()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var query = new GetTaskItemsQuery(new TaskItemQueryFilter(Page: 1, PageSize: 1001));

        // Act
        var result = await _validator.ValidateAsync(query, cancellationToken);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorCode == "Pagination.PageSizeMustBeLessThanOrEqualTo1000");
    }

    [Fact]
    public async Task ValidateAsync_ShouldSucceed_WhenPaginationIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var query = new GetTaskItemsQuery(new TaskItemQueryFilter(Page: 1, PageSize: 25));

        // Act
        var result = await _validator.ValidateAsync(query, cancellationToken);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}