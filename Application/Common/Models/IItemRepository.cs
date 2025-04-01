using Domain.Models;

namespace Application.Common.Models;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetItems(CancellationToken cancellationToken);
    Task<Item?> GetItem(string id, CancellationToken cancellationToken);
    Task<Item> CreateItem(BaseItem item, CancellationToken cancellationToken);
    Task<Item> UpdateItem(string id, PartialItem item, CancellationToken cancellationToken);
    Task DeleteItem(string id, CancellationToken cancellationToken);
}