using Application.Common.Models;
using Application.Exceptions;
using Domain.Models;
using FluentValidation;
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

// ReSharper disable once UnusedMember.Global - Used by ValidationBehavior
public class GetItemValidator : AbstractValidator<GetItem>
{
    public GetItemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The identifier must not be empty.")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("The identifier contains invalid characters.");
    }
}