using Application.Common.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands;

public sealed class DeleteItem(string id) : IRequest
{
    public string Id { get; init; } = id;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class DeleteItemHandler(ItemRepository itemRepository)
    : IRequestHandler<DeleteItem>
{
    public async Task Handle(DeleteItem request, CancellationToken cancellationToken)
        => await itemRepository.DeleteItem(request.Id, cancellationToken);
}