using Application.Common.Models;
using Application.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Queries;

public sealed class GetItem(string id) : IRequest<Item>
{
    public string Id { get; init; } = id;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class GetItemHandler(ItemRepository itemRepository)
    : IRequestHandler<GetItem, Item>
{
    public async Task<Item> Handle(GetItem request, CancellationToken cancellationToken)
    => await itemRepository.GetItem(request.Id, cancellationToken)?? throw new ItemNotFound();
}