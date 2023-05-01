using System.Reflection;
using App.Commands;

namespace App.Configuration;

public sealed class Settings
{
    public static class Cli
    {
        public const string UsageName = @"PhoneCli";
        public const string FriendlyName = @"PhoneCli";
        public const string Description = @"A net global tool helping to generate and validate phone number(s).";
        public static readonly string UserSecretsFile = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Microsoft\UserSecrets\PhoneCli-UserSecrets\secrets.json";
        public static readonly string Version = GetInformationalVersion().Split("+").FirstOrDefault();
        
        private static string GetInformationalVersion()
        {
            return typeof(ToolCommand)
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;
        }
    }
    
    public static class ExitCode
    {
        public const int Ok = 0;
        public const int Ko = -1;
    }
    
    public static string GetDefaultWorkingDirectory()
    {
        const string defaultDirectory = @"C:\Logs";
        var directory = Directory.Exists(defaultDirectory) 
            ? defaultDirectory 
            : "./";
        return Path.GetFullPath(directory);
    }
    
    public int DefaultMaxItems { get; init; } = 30;
    public string DefaultPhoneType { get; init; } = "Any";
    public string DefaultCountryCode { get; init; } = "Any";
}