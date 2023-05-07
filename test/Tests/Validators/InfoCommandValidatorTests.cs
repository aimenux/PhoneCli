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
    [ClassData(typeof(Keywords))]
    public void InfoCommand_Should_Be_Valid(string[] keywords)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            KeyWords = keywords
        };
        
        var validator = new InfoCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
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