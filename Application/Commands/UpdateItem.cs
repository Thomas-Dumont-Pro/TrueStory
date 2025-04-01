using Application.Common.Models;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Commands;

public sealed class UpdateItem(string id, PartialItem item) : IRequest<Item>
{
    public string Id { get; init; } = id;
    public PartialItem Item { get; init; } = item;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class UpdateItemHandler(IItemRepository itemRepository)
    : IRequestHandler<UpdateItem, Item>
{
    public async Task<Item> Handle(UpdateItem request, CancellationToken cancellationToken)
        => await itemRepository.UpdateItem(request.Id, request.Item, cancellationToken);
}

// ReSharper disable once UnusedMember.Global - Used by ValidationBehavior
// Here we are not using the BaseItemValidator because we can modify the item partially.
public class UpdateItemValidator : AbstractValidator<UpdateItem>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}