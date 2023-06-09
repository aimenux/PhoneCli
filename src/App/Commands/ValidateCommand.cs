using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace App.Commands;

[Command("Validate", FullName = "Validate phone number(s)", Description = "Validate phone number(s).")]
public class ValidateCommand : AbstractCommand
{
    private readonly IPhoneService _phoneService;

    public ValidateCommand(
        IConsoleService consoleService,
        IPhoneService phoneService,
        IOptions<Settings> options) : base(
        consoleService,
        options)
    {
        _phoneService = phoneService ?? throw new ArgumentNullException(nameof(phoneService));
        CountryCode = Settings.DefaultCountryCode;
    }
    
    [Option("-c|--country", "Country code", CommandOptionType.SingleValue)]
    public string CountryCode { get; init; }

    [Option("-n|--number", "Phone number", CommandOptionType.SingleValue)]
    public string PhoneNumber { get; init; }

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new PhoneParameters
        {
            CountryCode = CountryCode,
            PhoneNumber = PhoneNumber
        };
        
        ConsoleService.RenderStatus(() =>
        {
            if (!_phoneService.TryValidate(parameters, out var phoneNumber))
            {
                ConsoleService.RenderProblem($"{parameters.PhoneNumber} is not valid");
                return;
            }

            ConsoleService.RenderPhoneNumber(parameters, phoneNumber);
        });
    }
}