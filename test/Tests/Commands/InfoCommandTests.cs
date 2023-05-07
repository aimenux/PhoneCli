using App.Commands;
using App.Configuration;
using App.Services.Phone;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Tests.Commands;

public class InfoCommandTests
{
    [Theory]
    [InlineData("FR", 1)]
    [InlineData("BE", 2)]
    [InlineData("TN", 3)]
    [InlineData(null, 4)]
    [InlineData("", 100)]
    public void Should_InfoCommand_Return_Ok(string countryCode, int maxItems)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            MaxItems = maxItems
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Theory]
    [InlineData("#", 1)]
    [InlineData("FR", 0)]
    [InlineData("BE", -1)]
    [InlineData("TN", 1001)]
    public void Should_InfoCommand_Return_Ko(string countryCode, int maxItems)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            CountryCode = countryCode,
            MaxItems = maxItems
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }    
}