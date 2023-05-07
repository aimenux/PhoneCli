using App.Commands;
using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using App.Validators;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Tests.Validators;

public class InfoCommandValidatorTests
{
    [Theory]
    [InlineData("FR", 1)]
    [InlineData("BE", 2)]
    [InlineData("TN", 3)]
    [InlineData(null, 4)]
    [InlineData("", 100)]
    public void InfoCommand_Should_Be_Valid(string countryCode, int maxItems)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            MaxItems = maxItems
        };
        
        var validator = new InfoCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("#", 1)]
    [InlineData("FR", 0)]
    [InlineData("BE", -1)]
    [InlineData("TN", 1001)]
    public void InfoCommand_Should_Not_Be_Valid(string countryCode, int maxItems)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            MaxItems = maxItems
        };
        
        var validator = new InfoCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}