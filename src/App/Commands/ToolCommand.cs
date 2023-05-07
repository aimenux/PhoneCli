﻿using App.Configuration;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;
using static App.Extensions.PathExtensions;

namespace App.Commands;

[Command(Name = Settings.Cli.UsageName, Description = $"\n{Settings.Cli.Description}")]
[Subcommand(typeof(InfoCommand), typeof(GenerateCommand), typeof(ValidateCommand))]
public class ToolCommand : AbstractCommand
{
    public ToolCommand(IConsoleService consoleService, IOptions<Settings> options) : base(consoleService, options)
    {
    }

    [Option("-s|--settings", "Show settings information.", CommandOptionType.NoValue)]
    public bool ShowSettings { get; init; }
    
    [Option("-v|--version", "Show version information.", CommandOptionType.NoValue)]
    public bool ShowVersion { get; init; }

    protected override void Execute(CommandLineApplication app)
    {
        if (ShowSettings)
        {
            var settingFile = GetSettingFilePath();
            var userSecretsFile = Settings.Cli.UserSecretsFile;
            ConsoleService.RenderSettingsFile(settingFile);
            ConsoleService.RenderUserSecretsFile(userSecretsFile);
        }
        else if (ShowVersion)
        {
            ConsoleService.RenderVersion(Settings.Cli.Version);
        }
        else
        {
            ConsoleService.RenderTitle(Settings.Cli.FriendlyName);
            app.ShowHelp();
        }
    }
}