using Application.Common.Models;
using Application.Common.Validators;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Commands;

public sealed class CreateItem(BaseItem item) : IRequest<Item>
{
    public BaseItem Item { get; init; } = item;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class CreateItemHandler(IItemRepository itemRepository)
    : IRequestHandler<CreateItem, Item>
{
    public async Task<Item> Handle(CreateItem request, CancellationToken cancellationToken)
        => await itemRepository.CreateItem(request.Item, cancellationToken);
}

// ReSharper disable once UnusedMember.Global - Used by ValidationBehavior
public class CreateItemValidator : AbstractValidator<CreateItem>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.Item).SetValidator(new BaseItemValidator());
    }
}