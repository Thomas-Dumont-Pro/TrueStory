using Application.Common.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands;


public sealed class UpdateItem(Item item) : IRequest<Item>
{
    public Item Item { get; init; } = item;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class UpdateItemHandler(ItemRepository itemRepository)
    : IRequestHandler<UpdateItem, Item>
{
    public async Task<Item> Handle(UpdateItem request, CancellationToken cancellationToken)
        => await itemRepository.UpdateItem(request.Item, cancellationToken);
}