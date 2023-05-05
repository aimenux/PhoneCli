using App.Commands;
using App.Configuration;
using App.Services.Phone;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Tests.Commands;

public class ValidateCommandTests
{
    [Theory]
    [InlineData("TN", "30010123")]
    [InlineData("FR", "612345678")]
    [InlineData("TN", "+21630010123")]
    [InlineData("FR", "+33612345678")]
    public void Should_ValidateCommand_Return_Ok(string countryCode, string phoneNumber)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new ValidateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneNumber = phoneNumber
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Theory]
    [InlineData("TN", "")]
    [InlineData("FR", " ")]
    [InlineData("BE", null)]
    [InlineData("ZZ", "30010123")]
    public void Should_ValidateCommand_Return_Ko(string countryCode, string phoneNumber)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new ValidateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneNumber = phoneNumber
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}