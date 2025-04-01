using Domain.Models;

namespace Application.Common.Validators;

using FluentValidation;

public class BaseItemValidator : AbstractValidator<BaseItem>
{
    public BaseItemValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}