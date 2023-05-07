using App.Configuration;
using App.Services.Console;
using App.Services.Phone;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace App.Commands;

[Command("Info", FullName = "Get infos about country/calling codes", Description = "Get infos about country/calling codes.")]
public class InfoCommand : AbstractCommand
{
    private readonly IPhoneService _phoneService;
    
    public InfoCommand(
        IConsoleService consoleService,
        IPhoneService phoneService,
        IOptions<Settings> options) : base(
        consoleService,
        options)
    {
        _phoneService = phoneService ?? throw new ArgumentNullException(nameof(phoneService));
    }

    [Option("-f|--filter", "Filter keywords", CommandOptionType.MultipleValue)]
    public string[] KeyWords { get; init; } = Array.Empty<string>();
    
    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new PhoneParameters
        {
            KeyWords = KeyWords
        };
        
        ConsoleService.RenderStatus(() =>
        {
            var phoneCodes = _phoneService.GetPhoneCodes(parameters);
            ConsoleService.RenderPhoneCodes(parameters, phoneCodes);
        });
    }
}