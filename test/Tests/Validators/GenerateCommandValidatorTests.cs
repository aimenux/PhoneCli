using App.Commands;
using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using App.Validators;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Tests.Validators;

public class GenerateCommandValidatorTests
{
    [Theory]
    [InlineData("tn", null, 1)]
    [InlineData("be", "fixed", 2)]
    [InlineData("fr", "mobile", 3)]
    public void GenerateCommand_Should_Be_Valid(string countryCode, string phoneType, int maxItems)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new GenerateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneType = phoneType,
            MaxItems = maxItems
        };
        
        var validator = new GenerateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("tn", "hybrid", 1)]
    [InlineData("bel", "fixed", 2)]
    [InlineData("fr", "mobile", 0)]
    [InlineData("es", "mobile", -1)]
    [InlineData("it", "mobile", 1001)]
    public void GenerateCommand_Should_Not_Be_Valid(string countryCode, string phoneType, int maxItems)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new GenerateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneType = phoneType,
            MaxItems = maxItems
        };
        
        var validator = new GenerateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }    
}