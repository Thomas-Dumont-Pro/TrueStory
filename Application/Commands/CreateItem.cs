using Application.Common.Models;
using Application.Exceptions;
using Application.Queries;
using Domain.Models;
using MediatR;

namespace Application.Commands;

public sealed class CreateItem(BaseItem item) : IRequest<Item>
{
    public BaseItem Item { get; init; } = item;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class CreateItemHandler(ItemRepository itemRepository)
    : IRequestHandler<CreateItem, Item>
{
    public async Task<Item> Handle(CreateItem request, CancellationToken cancellationToken)
        => await itemRepository.CreateItem(request.Item, cancellationToken);
}