using App.Commands;
using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using App.Validators;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Tests.Validators;

public class ValidateCommandValidatorTests
{
    [Theory]
    [InlineData("tn", "50298873")]
    [InlineData("tn", "+21650298873")]
    [InlineData(null, "+21650298873")]
    [InlineData("fr", "785412563")]
    [InlineData("fr", "+33785412563")]
    [InlineData(null, "+33785412563")]
    public void ValidateCommand_Should_Be_Valid(string countryCode, string phoneNumber)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new ValidateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneNumber = phoneNumber
        };
        
        var validator = new ValidateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("tn", null)]
    [InlineData("be", "")]
    [InlineData("fr", " ")]
    [InlineData("tnz", "+21650298873")]
    [InlineData("frs", "+33785412563")]
    public void ValidateCommand_Should_Not_Be_Valid(string countryCode, string phoneNumber)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new ValidateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneNumber = phoneNumber
        };
        
        var validator = new ValidateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }    
}