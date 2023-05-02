using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class GenerateCommandValidator : AbstractValidator<GenerateCommand>
{
    public GenerateCommandValidator()
    {
        RuleFor(x => x.CountryCode)
            .Must(x => x.IsValidCountryCode())
            .When(x => !string.IsNullOrEmpty(x.CountryCode));

        RuleFor(x => x.PhoneType)
            .Must(x => x.IsValidPhoneType())
            .When(x => !string.IsNullOrEmpty(x.PhoneType));
        
        RuleFor(x => x.MaxItems)
            .InclusiveBetween(1, 1000);
    }
}