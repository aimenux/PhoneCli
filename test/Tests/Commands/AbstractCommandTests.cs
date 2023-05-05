using App.Commands;
using App.Configuration;
using App.Services.Console;
using App.Validators;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Tests.Commands;

public class AbstractCommandTests
{
    [Fact]
    public void Given_Invalid_Inputs_Then_Should_Throw_Exception()
    {
        // arrange
        // act
        var func1 = () => new FakeCommand(null, Options.Create(new Settings()));
        var func2 = () => new FakeCommand(new FakeConsoleService(), null);

        // assert
        func1.Should().Throw<ArgumentNullException>();
        func2.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void Should_Return_Ok()
    {
        // arrange
        var app = new CommandLineApplication();
        var service = new FakeConsoleService();
        var options = Options.Create(new Settings());
        var command = new FakeCommand(service, options)
        {
            Job = () => Task.CompletedTask
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Fact]
    public void Should_Return_Ko()
    {
        // arrange
        var app = new CommandLineApplication();
        var service = new FakeConsoleService();
        var options = Options.Create(new Settings());
        var command = new FakeCommand(service, options)
        {
            Job = () => throw new Exception("some error has occurred")
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
    
    private class FakeCommand : AbstractCommand
    {
        public Func<Task> Job { get; init; }

        public FakeCommand(IConsoleService consoleService, IOptions<Settings> options) : base(consoleService, options)
        {
        }

        protected override void Execute(CommandLineApplication app)
        {
            Job.Invoke();
        }

        protected override bool HasValidOptionsAndArguments(out ValidationErrors validationErrors)
        {
            validationErrors = ValidationErrors.New<FakeCommand>();
            return true;
        }
    }
}