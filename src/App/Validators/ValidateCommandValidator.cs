using App.Commands;
using FluentValidation;

namespace App.Validators;

public class ValidateCommandValidator : AbstractValidator<ValidateCommand>
{
    public ValidateCommandValidator()
    {
        RuleFor(x => x.CountryCode)
            .NotEmpty();
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}