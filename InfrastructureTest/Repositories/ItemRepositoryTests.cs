using System.Net;
using Domain.Models;
using Infrastructure.Repositories;
using Moq;
using Moq.Protected;

namespace InfrastructureTest.Repositories;

public class ItemRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly ItemRepository _itemRepository;
    
    public ItemRepositoryTests()
    {
        var factoryMock = new Mock<IHttpClientFactory>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        factoryMock.Setup(x => x.CreateClient(It.IsAny<String>()))
            .Returns(new HttpClient(_mockHttpMessageHandler.Object){BaseAddress = new Uri("https://localhost:5000")});
        _itemRepository = new ItemRepository(factoryMock.Object);
    }

    private void MockResponse(HttpResponseMessage message)
    {
        //HttpRequestMessage request, CancellationToken cancellationToken
        _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(message);
    }

    [Fact]
    public async Task GetAllItems_ShouldReturnAllItems()
    {
        // Arrange
        var items = new List<Item> { new Item { Id = "1", Name = "Item1" }, new Item { Id = "2", Name = "Item2" } };
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(items), System.Text.Encoding.UTF8, "application/json")
        };
        this.MockResponse(httpResponseMessage);

         // Act
         var result = await _itemRepository.GetItems(new CancellationToken());

        // Assert
        Assert.NotNull(result);

        var enumerable = result.ToList();
        Assert.Equal(2, enumerable.Count);
        Assert.Equal("Item1", enumerable[0].Name);
        Assert.Equal("Item2", enumerable[1].Name);
    }

    [Fact]
    public async Task GetItemById_ShouldReturnCorrectItem()
    {
        // Arrange
        var item = new Item { Id = "1", Name = "Item1" };
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(item), System.Text.Encoding.UTF8, "application/json")
        };
        this.MockResponse(httpResponseMessage);

        // Act
        var result = await _itemRepository.GetItem("1", new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Item1", result.Name);
    }

    [Fact]
    public async Task AddItem_ShouldAddItem()
    {
        // Arrange
        var item = new Item { Id = "1", Name = "NewItem" };
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(item), System.Text.Encoding.UTF8, "application/json")
        };
        this.MockResponse(httpResponseMessage);

        // Act
        var result = await _itemRepository.CreateItem(item, new CancellationToken());

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteItem_ShouldDeleteItem()
    {
        // Arrange
        var itemId = "1";

        this.MockResponse(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        await _itemRepository.DeleteItem(itemId, new CancellationToken());

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
