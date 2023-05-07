using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.Json;
using App.Configuration;
using App.Services.Phone;
using App.Validators;
using Spectre.Console;

namespace App.Services.Console;

[ExcludeFromCodeCoverage]
public class ConsoleService : IConsoleService
{
    public ConsoleService()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
    }

    public void RenderTitle(string text)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new FigletText(text));
        AnsiConsole.WriteLine();
    }

    public void RenderVersion(string version)
    {
        var text = $"{Settings.Cli.FriendlyName} V{version}";
        RenderText(text, Color.White);
    }

    public void RenderProblem(string text) => RenderText(text, Color.Red);

    public void RenderText(string text, Color color)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Markup($"[bold {color}]{text}[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }

    public void RenderSettingsFile(string filepath)
    {
        var name = Path.GetFileName(filepath);
        var json = File.ReadAllText(filepath);
        var formattedJson = GetFormattedJson(json);
        var header = new Rule($"[yellow]({name})[/]");
        header.Centered();
        var footer = new Rule($"[yellow]({filepath})[/]");
        footer.Centered();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine(formattedJson);
        AnsiConsole.Write(footer);
        AnsiConsole.WriteLine();
    }
    
    public void RenderUserSecretsFile(string filepath)
    {
        if (!OperatingSystem.IsWindows()) return;
        if (!File.Exists(filepath)) return;
        if (!GetYesOrNoAnswer("display user secrets", false)) return;
        RenderSettingsFile(filepath);
    }

    public void RenderException(Exception exception) => RenderAnyException(exception);

    public static void RenderAnyException<T>(T exception) where T : Exception
    {
        const ExceptionFormats formats = ExceptionFormats.ShortenTypes
                                         | ExceptionFormats.ShortenPaths
                                         | ExceptionFormats.ShortenMethods;

        AnsiConsole.WriteLine();
        AnsiConsole.WriteException(exception, formats);
        AnsiConsole.WriteLine();
    }
    
    public void RenderStatus(Action action)
    {
        var spinner = RandomSpinner();

        AnsiConsole.Status()
            .Start("Work is in progress ...", ctx =>
            {
                ctx.Spinner(spinner);
                action.Invoke();
            });
    }

    public bool GetYesOrNoAnswer(string text, bool defaultAnswer)
    {
        if (AnsiConsole.Confirm($"Do you want to [u]{text}[/] ?", defaultAnswer)) return true;
        AnsiConsole.WriteLine();
        return false;
    }

    public void RenderValidationErrors(ValidationErrors validationErrors)
    {
        var count = validationErrors.Count;

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[red][bold]{count} error(s)[/][/]")
            .AddColumn(new TableColumn("[u]Name[/]").Centered())
            .AddColumn(new TableColumn("[u]Message[/]").Centered())
            .Caption("[grey][bold]Invalid options/arguments[/][/]");

        foreach (var error in validationErrors)
        {
            var failure = error.Failure;
            var name = $"[bold]{error.OptionName()}[/]";
            var reason = $"[tan]{failure.ErrorMessage}[/]";
            table.AddRow(ToMarkup(name), ToMarkup(reason));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void RenderPhoneNumber(PhoneParameters parameters, PhoneNumber phoneNumber)
    {
        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[green][bold]{parameters.PhoneNumber} is valid[/][/]")
            .AddColumn(new TableColumn("[u]CountryCode[/]").Centered())
            .AddColumn(new TableColumn("[u]PhoneType[/]").Centered())
            .AddColumn(new TableColumn("[u]PhoneNumber[/]").Centered());

        table.AddRow(
            ToMarkup(phoneNumber.CountryCode),
            ToMarkup(phoneNumber.PhoneType),
            ToMarkup(phoneNumber.ToString()));

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine(); 
    }

    public void RenderPhoneNumbers(PhoneParameters parameters, IEnumerable<PhoneNumber> phoneNumbers)
    {
        var count = parameters.MaxItems;
        
        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[yellow][bold]{count} phone number(s)[/][/]")
            .AddColumn(new TableColumn("[u]#[/]").Centered())
            .AddColumn(new TableColumn("[u]CountryCode[/]").Centered())
            .AddColumn(new TableColumn("[u]PhoneType[/]").Centered())
            .AddColumn(new TableColumn("[u]PhoneNumber[/]").Centered());

        var index = 1;
        foreach (var phoneNumber in phoneNumbers)
        {
            table.AddRow(
                IndexMarkup(index++),
                ToMarkup(phoneNumber.CountryCode),
                ToMarkup(phoneNumber.PhoneType),
                ToMarkup(phoneNumber.ToString()));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();        
    }

    public void RenderPhoneCodes(PhoneParameters parameters, IEnumerable<PhoneCode> phoneCodes)
    {
        var items = phoneCodes
            .OrderBy(x => x.CountryCode)
            .ToList();

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[yellow][bold]{items.Count} phone code(s)[/][/]")
            .AddColumn(new TableColumn("[u]#[/]").Centered())
            .AddColumn(new TableColumn("[u]CountryCode[/]").Centered())
            .AddColumn(new TableColumn("[u]CallingCode[/]").Centered());

        var index = 1;
        foreach (var item in items)
        {
            table.AddRow(
                IndexMarkup(index++),
                ToMarkup(item.CountryCode),
                ToMarkup(item.CallingCode.ToString()));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static string GetFormattedJson(string json)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        using var document = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(document, options);
    }

    private static Spinner RandomSpinner() 
    {
        var values = typeof(Spinner.Known)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.PropertyType == typeof(Spinner))
            .Select(x => (Spinner)x.GetValue(null))
            .ToArray();

        var index = Random.Shared.Next(values.Length);
        var value = (Spinner)values.GetValue(index);
        return value;
    }

    private static Markup ToMarkup(string text)
    {
        try
        {
            return new Markup(text ?? string.Empty);
        }
        catch
        {
            return ErrorMarkup;
        }
    }

    private static readonly Markup ErrorMarkup = new(Emoji.Known.CrossMark);

    private static Markup IndexMarkup(int index) => ToMarkup($"[dim]{index:D4}[/]");
}