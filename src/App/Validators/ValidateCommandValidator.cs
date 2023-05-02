using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class ValidateCommandValidator : AbstractValidator<ValidateCommand>
{
    public ValidateCommandValidator()
    {
        RuleFor(x => x.CountryCode)
            .Must(x => x.IsValidCountryCode())
            .When(x => !string.IsNullOrEmpty(x.CountryCode));
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}