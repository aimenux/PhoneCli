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
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1001)]
    public void InfoCommand_Should_Not_Be_Valid(int maxItems)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());
        var command = new InfoCommand(consoleService, phoneService, options)
        {
            MaxItems = maxItems
        };
        
        var validator = new InfoCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
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