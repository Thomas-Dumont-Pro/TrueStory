using System.Net;
using System.Text;
using Domain.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ApiTest.Controller;

public class ItemControllerTests
{
    private readonly ApiWebFactory _factory = new();

    [Test]
    public async Task GetItems_ReturnsOkResult_WithItems()
    {
        // Arrange
        var result = await _factory.CreateClient().GetAsync("Item");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await result.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<List<Item>>(content);
        items.Should().NotBeEmpty();
    }

    [Test]
    public async Task GetItem_ReturnsOkResult_WithItem()
    {
        // Arrange
        var result = await _factory.CreateClient().GetAsync("Item/3");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await result.Content.ReadAsStringAsync();
        var item = JsonConvert.DeserializeObject<Item>(content);
        item.Should().BeOfType<Item>();
        item.Id.Should().Be("3");
    }

    protected string ItemId = null!;

    [Test, Order(0)]
    public async Task CreateItem_ReturnsCreatedResult_WithItem()
    {
        // Arrange
        var newItem = new BaseItem { Name = "NewItem" };
        var content = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");

        // Act
        var result = await _factory.CreateClient().PostAsync("Item", content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseContent = await result.Content.ReadAsStringAsync();
        var createdItem = JsonConvert.DeserializeObject<Item>(responseContent);
        ItemId = createdItem!.Id;
        createdItem.Should().BeOfType<Item>();
        createdItem.Name.Should().Be("NewItem");
    }

    [Test, Order(1)]
    public async Task UpdateItem_ReturnsAccepted()
    {
        // Arrange
        var updatedItem = new Item { Id = this.ItemId, Name = "UpdatedItem", Data = new { Description = "Test" } };
        var content = new StringContent(JsonConvert.SerializeObject(updatedItem), Encoding.UTF8, "application/json");

        // Act
        var result = await _factory.CreateClient().PatchAsync($"Item/{this.ItemId}", content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    [Test, Order(2)]
    public async Task DeleteItem_ReturnsNoContentResult()
    {
        // Act
        var result = await _factory.CreateClient().DeleteAsync($"Item/{this.ItemId}");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
