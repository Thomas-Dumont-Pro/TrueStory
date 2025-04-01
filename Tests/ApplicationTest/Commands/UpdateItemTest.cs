using Application.Commands;
using Application.Common.Models;
using Domain.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

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

    [Test, Order(0)]
    public async Task Handle_ShouldUpdateItem_WhenItemExists()
    {
        // Arrange
        var itemId = Guid.NewGuid().ToString();

        var existingItem = new Item { Id = itemId, Name = "Old Item" };
        var updateItemCommand = new UpdateItem(itemId, existingItem);

        _itemRepositoryMock.Setup(repo => repo.UpdateItem(It.IsAny<string>(),It.IsAny<BaseItem>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingItem);

        // Act
        var stillExistingItem = await _handler.Handle(updateItemCommand, CancellationToken.None);

        // Assert
        _itemRepositoryMock.Verify(repo => repo.UpdateItem(It.IsAny<string>(), It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()), Times.Once);
        stillExistingItem.Should().BeEquivalentTo(existingItem);
    }

    [Test, Order(1)]
    public async Task Handle_ShouldThrowException_WhenExceptionHappend()
    {
        // Arrange
        var itemId = Guid.NewGuid().ToString();

        var existingItem = new Item { Id = itemId, Name = "Old Item" };
        var updateItemCommand = new UpdateItem(itemId, existingItem);

        _itemRepositoryMock.Setup(repo => repo.UpdateItem(It.IsAny<string>(), It.IsAny<BaseItem>(), It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();

        // Act & Assert
        await _handler.Invoking(h => h.Handle(updateItemCommand, CancellationToken.None))
                      .Should().ThrowAsync<InvalidOperationException>();
    }
}
