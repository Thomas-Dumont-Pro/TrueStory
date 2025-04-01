using System.Net;
using System.Net.Http.Json;
using Application.Exceptions;
using Domain.Models;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class ItemRepository(IHttpClientFactory httpClient) : Application.Common.Models.ItemRepository
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("MockApi");

    public async Task<IEnumerable<Item>> GetItems(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync("objects", cancellationToken);

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<IEnumerable<Item>>(cancellationToken) ?? new List<Item>();
        return data;
    }

    public async Task<Item?> GetItem(string id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"objects/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new ItemNotFound();

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Item>(cancellationToken) ;
    }

    public async Task<Item> CreateItem(BaseItem item, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("objects", item, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Item>(cancellationToken) ?? throw new InvalidOperationException();
    }

    public async Task<Item> UpdateItem(Item item, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PatchAsJsonAsync("objects", item, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Item>(cancellationToken) ?? throw new InvalidOperationException();
    }

    public async Task DeleteItem(string id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync($"objects/{id}", cancellationToken);
        
        response.EnsureSuccessStatusCode();
    }
}