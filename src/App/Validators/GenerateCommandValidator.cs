using App.Commands;
using FluentValidation;

namespace App.Validators;

public class GenerateCommandValidator : AbstractValidator<GenerateCommand>
{
    public GenerateCommandValidator()
    {
        RuleFor(x => x.CountryCode)
            .NotEmpty();
        
        RuleFor(x => x.PhoneType)
            .NotEmpty();
        
        RuleFor(x => x.MaxItems)
            .InclusiveBetween(1, 1000);
    }
}