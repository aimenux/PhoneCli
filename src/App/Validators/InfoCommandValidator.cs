using App.Commands;
using FluentValidation;

namespace App.Validators;

public class InfoCommandValidator : AbstractValidator<InfoCommand>
{
    public InfoCommandValidator()
    {
        RuleFor(x => x.MaxItems)
            .InclusiveBetween(1, 1000);
    }
}