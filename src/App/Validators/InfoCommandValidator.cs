using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class InfoCommandValidator : AbstractValidator<InfoCommand>
{
    public InfoCommandValidator()
    {
        RuleFor(x => x.CountryCode)
            .Must(x => x.IsValidCountryCode())
            .When(x => !string.IsNullOrEmpty(x.CountryCode));
        
        RuleFor(x => x.MaxItems)
            .InclusiveBetween(1, 1000);
    }
}