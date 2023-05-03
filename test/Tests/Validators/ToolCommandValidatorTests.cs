using App.Commands;
using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using App.Validators;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Tests.Validators;

public class ToolCommandValidatorTests
{
    [Fact]
    public void Should_Get_ValidationErrors_For_ToolCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var options = Options.Create(new Settings());
        var command = new ToolCommand(consoleService, options);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull().And.BeEmpty();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_GenerateCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());

        var command = new GenerateCommand(consoleService, phoneService, options);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull().And.BeEmpty();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_ValidateCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var phoneService = Substitute.For<IPhoneService>();
        var options = Options.Create(new Settings());

        var command = new ValidateCommand(consoleService, phoneService, options)
        {
            PhoneNumber = "123"
        };

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull().And.BeEmpty();
    }
}