using Application.Commands;
using Application.Common.Models;
using Domain.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Commands;

public class CreateItemTest
{
    [Test]
    public async Task CreateItemHandler_Should_Create_Item_Successfully()
    {
        // Arrange
        var item = new Item
        {
            Id = "TestId",
            Name = "Test Item",
            Data = new { description = "Test Description" }
        };
        var mockRepository = new Mock<IItemRepository>();
        mockRepository.Setup(repo => repo.CreateItem(It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var handler = new CreateItemHandler(mockRepository.Object);
        var command = new CreateItem(item);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        mockRepository.Verify(repo => repo.CreateItem(It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Name.Should().Be(item.Name);
        string? description = result.Data?.description;
        description.Should().NotBeEmpty().And.BeEquivalentTo(item.Data?.description);
        result.Id.Should().Be(item.Id);
    }

    [Test]
    public async Task CreateItemHandler_Should_Fail_When_Request_Fail()
    {
        // Arrange
        var mockRepository = new Mock<IItemRepository>();
        mockRepository
            .Setup(itemRepository => itemRepository.CreateItem(It.IsAny<BaseItem>(), It.IsAny<CancellationToken>()))
            .Throws<HttpRequestException>();
        var handler = new CreateItemHandler(mockRepository.Object);
        var command = new CreateItem(new()
        {
            Name = String.Empty,
            Data = new { description = "Test Description" }
        });

        // Act
        Func<Task> act = async () => await handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
