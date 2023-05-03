using App.Commands;
using App.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace Tests.Validators;

public class ValidationErrorTests
{
    [Fact]
    public void Should_Get_OptionName()
    {
        // arrange
        var validationFailure = new ValidationFailure(nameof(ValidateCommand.PhoneNumber), "Required option");
        var validationError = ValidationError.New<ValidateCommand>(validationFailure);

        // act
        var optionName = validationError.OptionName();

        // assert
        optionName.Should().NotBeNullOrWhiteSpace();
        optionName.Should().Be("-n|--number");
    }
}