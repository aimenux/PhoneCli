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
    [ClassData(typeof(Keywords))]
    public void Should_InfoCommand_Return_Ok(string[] keywords)
    {
        // arrange
        var options = Options.Create(new Settings());
        
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var phoneService = new PhoneService();
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            KeyWords = keywords
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    private class Keywords : TheoryData<string[]>
    {
        public Keywords()
        {
            Add(null);
            Add(Array.Empty<string>());
            Add(new[] { "fr" });
            Add(new[] { "fr", "be" });
            Add(new[] { "xyz" });
        }
    }
}