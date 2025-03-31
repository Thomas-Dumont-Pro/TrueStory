using Application.Commands;
using Application.Common.Models;
using Domain.Models;
using Moq;

namespace ApplicationTest.Commands;

public class DeleteItemTest
{
    private readonly Mock<ItemRepository> _repositoryMock;
    private readonly DeleteItemHandler _handler;

    public DeleteItemTest()
    {
        _repositoryMock = new Mock<ItemRepository>();
        _handler = new DeleteItemHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteItem_Should_Remove_Item_From_Repository()
    {
        // Arrange  
        _repositoryMock.Setup(repo => repo.DeleteItem(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Act  
        await _handler.Handle(new DeleteItem("itemId"), CancellationToken.None);

        // Assert  
        _repositoryMock.Verify(repo => repo.DeleteItem(It.IsAny<string>(),It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteItem_Should_Throw_Exception_If_HttpRequest_fail()
    {
        // Arrange  
        var itemId = "Test Id";
        _repositoryMock.Setup(repo => repo.DeleteItem(It.IsAny<string>(),It.IsAny<CancellationToken>())).Throws<HttpRequestException>();

        // Act & Assert  
        await Assert.ThrowsAsync<HttpRequestException>(() => _handler.Handle(new DeleteItem(itemId), CancellationToken.None));
    }
}
