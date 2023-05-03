using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace App.Commands;

[Command("Generate", FullName = "Generate phone number(s)", Description = "Generate phone number(s).")]
public class GenerateCommand : AbstractCommand
{
    private readonly IPhoneService _phoneService;

    public GenerateCommand(
        IConsoleService consoleService,
        IPhoneService phoneService,
        IOptions<Settings> options) : base(
        consoleService,
        options)
    {
        _phoneService = phoneService ?? throw new ArgumentNullException(nameof(phoneService));
        CountryCode = Settings.DefaultCountryCode;
        PhoneType = Settings.DefaultPhoneType;
        MaxItems = Settings.DefaultMaxItems;
    }
    
    [Option("-c|--country", "Country code", CommandOptionType.SingleValue)]
    public string CountryCode { get; init; }
    
    [Option("-t|--type", "Phone type", CommandOptionType.SingleValue)]
    public string PhoneType { get; init; }

    [Option("-m|--max", "Max items", CommandOptionType.SingleValue)]
    public int MaxItems { get; init; }

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new PhoneParameters
        {
            CountryCode = CountryCode,
            PhoneType = PhoneType,
            MaxItems = MaxItems
        };
        
        ConsoleService.RenderStatus(() =>
        {
            var phoneNumbers = _phoneService.Generate(parameters);
            ConsoleService.RenderPhoneNumbers(parameters, phoneNumbers);
        });
    }
}