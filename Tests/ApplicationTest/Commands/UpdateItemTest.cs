using Application.Commands;
using Application.Common.Models;
using Domain.Models;
using Moq;

namespace ApplicationTest.Commands;

public class UpdateItemTest
{
    private readonly Mock<ItemRepository> _itemRepositoryMock;
    private readonly UpdateItemHandler _handler;

    public UpdateItemTest()
    {
        _itemRepositoryMock = new Mock<ItemRepository>();
        _handler = new UpdateItemHandler(_itemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateItem_WhenItemExists()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        var existingItem = new Item { Id = itemId.ToString(), Name = "Old Item" };
        var updateItemCommand = new UpdateItem(existingItem) ;

        _itemRepositoryMock.Setup(repo => repo.UpdateItem(It.IsAny<Item>(),It.IsAny<CancellationToken>())).ReturnsAsync(existingItem);

        // Act
        var stillExistingItem = await _handler.Handle(updateItemCommand, CancellationToken.None);

        // Assert
        _itemRepositoryMock.Verify(repo => repo.UpdateItem(It.IsAny<Item>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(existingItem, stillExistingItem);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenExceptionHappend()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        var existingItem = new Item { Id = itemId.ToString(), Name = "Old Item" };
        var updateItemCommand = new UpdateItem(existingItem);

        _itemRepositoryMock.Setup(repo => repo.UpdateItem(It.IsAny<Item>(), It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(updateItemCommand, CancellationToken.None));
    }
}
