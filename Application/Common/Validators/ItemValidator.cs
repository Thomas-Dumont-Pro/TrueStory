using Domain.Models;
using FluentValidation;

namespace Application.Common.Validators;


// ReSharper disable once UnusedMember.Global 
public class ItemValidator : AbstractValidator<Item>
{
    public ItemValidator()
    {
        RuleFor(item => item).SetValidator(new BaseItemValidator());
        RuleFor(item => item.Id).NotEmpty().WithMessage("Id is required.");
    }
}