using Application.Common.Enums;
using Application.Common.Models;
using Application.Queries;
using Domain.Models;
using Moq;

namespace ApplicationTest.Queries;

public class GetItemsWithPaginationTest
{
    private readonly Mock<ItemRepository> _itemRepositoryMock;
    private readonly GetItemsWithPaginationHandler _handler;

    public GetItemsWithPaginationTest()
    {
        _itemRepositoryMock = new Mock<ItemRepository>();
        _handler = new GetItemsWithPaginationHandler(_itemRepositoryMock.Object);
    }

    [Theory]
    [InlineData(0, 10, SortOrder.Ascending,2)]
    [InlineData(1, 10, SortOrder.Ascending, 0)]
    [InlineData(0, 1, SortOrder.Ascending, 1)]
    public async Task Handle_ShouldReturnPaginatedItems(int pageNumber,int pageSize, SortOrder order,int countExpected)
    {
        // Arrange
        var items = new List<Item>
            {
                new () { Id = "1", Name = "Item1" },
                new () { Id = "2", Name = "Item2" }
            };
        _itemRepositoryMock.Setup(repo => repo.GetItems( It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        var query = new GetItemsWithPagination{PageNumber = pageNumber, PageSize = pageSize, SortOrder = order };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var enumerable = result.ToList();
        Assert.Equal(countExpected, enumerable.Count);
    }
}
