using Application.Common.Enums;
using Application.Common.Models;
using Application.Queries;
using Domain.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Queries;

public class GetItemsWithPaginationTest
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly GetItemsWithPaginationHandler _handler;

    public GetItemsWithPaginationTest()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _handler = new GetItemsWithPaginationHandler(_itemRepositoryMock.Object);
    }

    [TestCase(0, 10, SortOrder.Ascending, 2)]
    [TestCase(1, 10, SortOrder.Ascending, 0)]
    [TestCase(0, 1, SortOrder.Ascending, 1)]
    public async Task Handle_ShouldReturnPaginatedItems(int pageNumber, int pageSize, SortOrder order, int countExpected)
    {
        // Arrange
        var items = new List<Item>
            {
                new () { Id = "1", Name = "Item1" },
                new () { Id = "2", Name = "Item2" }
            };
        _itemRepositoryMock.Setup(repo => repo.GetItems(It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        var query = new GetItemsWithPagination { PageNumber = pageNumber, PageSize = pageSize, SortOrder = order };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var enumerable1 = result.ToList();
        enumerable1.Should().NotBeNull();
        var enumerable = enumerable1.ToList();
        enumerable.Count.Should().Be(countExpected);
    }
}
