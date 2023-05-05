using App.Commands;
using App.Configuration;
using App.Services.Phone;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Tests.Commands;

public class GenerateCommandTests
{
    [Theory]
    [InlineData("TN", null, 1)]
    [InlineData("FR", "fixed", 2)]
    [InlineData("BE", "mobile", 3)]
    public void Should_GenerateCommand_Return_Ok(string countryCode, string phoneType, int maxItems)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new GenerateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneType = phoneType,
            MaxItems = maxItems
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Theory]
    [InlineData("TN", null, 0)]
    [InlineData("FR", "fixed", -1)]
    [InlineData("BE", "mobile", 2000)]
    [InlineData("TN", "none", 1)]
    [InlineData("ZZ", "fixed", 2)]
    public void Should_GenerateCommand_Return_Ko(string countryCode, string phoneType, int maxItems)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new GenerateCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            PhoneType = phoneType,
            MaxItems = maxItems
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}