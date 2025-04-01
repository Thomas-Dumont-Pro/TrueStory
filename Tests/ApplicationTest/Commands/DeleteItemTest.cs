using Application.Commands;
using Application.Common.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Commands;

public class DeleteItemTest
{
    private readonly Mock<IItemRepository> _repositoryMock;
    private readonly DeleteItemHandler _handler;

    public DeleteItemTest()
    {
        _repositoryMock = new Mock<IItemRepository>();
        _handler = new DeleteItemHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task DeleteItem_Should_Remove_Item_From_Repository()
    {
        // Arrange  
        _repositoryMock.Setup(repo => repo.DeleteItem(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Act  
        await _handler.Handle(new DeleteItem("itemId"), CancellationToken.None);

        // Assert  
        _repositoryMock.Verify(repo => repo.DeleteItem(It.IsAny<string>(),It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeleteItem_Should_Throw_Exception_If_HttpRequest_fail()
    {
        // Arrange  
        var itemId = "Test Id";
        _repositoryMock.Setup(repo => repo.DeleteItem(It.IsAny<string>(), It.IsAny<CancellationToken>())).Throws<HttpRequestException>();

        // Act
        Func<Task> act = async () => await _handler.Handle(new DeleteItem(itemId), CancellationToken.None);

        // Assert  
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
