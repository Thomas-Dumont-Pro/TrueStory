using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Commands;

public sealed class DeleteItem(string id) : IRequest
{
    public string Id { get; init; } = id;
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class DeleteItemHandler(IItemRepository itemRepository)
    : IRequestHandler<DeleteItem>
{
    public async Task Handle(DeleteItem request, CancellationToken cancellationToken)
        => await itemRepository.DeleteItem(request.Id, cancellationToken);
}

// ReSharper disable once UnusedMember.Global - Used by ValidationBehavior
public class DeleteItemValidator : AbstractValidator<DeleteItem>
{
    public DeleteItemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
