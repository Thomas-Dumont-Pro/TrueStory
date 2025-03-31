using Application.Common.Models;
using Application.Exceptions;
using Application.Queries;
using Domain.Models;
using Moq;

namespace ApplicationTest.Queries;

public class GetItemTest
{
    private readonly Mock<ItemRepository> _itemRepositoryMock;
    private readonly GetItemHandler _handler;

    public GetItemTest()
    {
        _itemRepositoryMock = new Mock<ItemRepository>();
        _handler = new GetItemHandler(_itemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnItem_WhenItemExists()
    {
        // Arrange
        var item = new Item { Id = Guid.NewGuid().ToString(), Name = "Test Item" };
        _itemRepositoryMock.Setup(repo => repo.GetItem(It.IsAny<string>(),It.IsAny<CancellationToken>())).ReturnsAsync(item);
        var query = new GetItem(item.Id) ;

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(item.Id, result.Id);
        Assert.Equal(item.Name, result.Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenItemDoesNotExist()
    {
        // Arrange
        var itemId = Guid.NewGuid().ToString();
        _itemRepositoryMock.Setup(repo => repo.GetItem(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Item?)null);
        var query = new GetItem(itemId) ;

        // Act & Assert
        await Assert.ThrowsAsync<ItemNotFound>(()=> _handler.Handle(query, CancellationToken.None));
    }
}
