﻿using Application.Commands;
using Application.Common.Models;
using Domain.Models;
using Moq;

namespace ApplicationTest.Commands;

public class CreateItemTest
{
    [Fact]
    public async Task CreateItemHandler_Should_Create_Item_Successfully()
    {
        // Arrange
        var item = new Item
        {
            Id = "TestId",
            Name = "Test Item",
            Data = new { description = "Test Description" }
        };
        var mockRepository = new Mock<ItemRepository>();
        mockRepository.Setup(repo => repo.CreateItem(It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var handler = new CreateItemHandler(mockRepository.Object);
        var command = new CreateItem(item);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        mockRepository.Verify(repo => repo.CreateItem(It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(item.Name, result.Name);
        Assert.Equal(item.Data, result.Data);
        Assert.Equal(item.Id, result.Id);
    }

    [Fact]
    public async Task CreateItemHandler_Should_Fail_When_Request_Fail()
    {
        // Arrange
        var mockRepository = new Mock<ItemRepository>();
        mockRepository
            .Setup(itemRepository => itemRepository.CreateItem(It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()))
            .Throws<HttpRequestException>();
        var handler = new CreateItemHandler(mockRepository.Object);
        var command = new CreateItem(new ()
        {
            Name = String.Empty,
            Data = new { description = "Test Description" }
        });

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => handler.Handle(command, default));
    }
}
