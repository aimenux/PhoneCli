[![.NET](https://github.com/aimenux/PhoneCli/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/PhoneCli/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/vpre/PhoneCli)](https://www.nuget.org/packages/PhoneCli/)

# PhoneCli
```
A net global tool helping to generate and validate phone numbers
```

> In this repo, i m building a global tool that allows to generate and validate phone numbers.
>
> The tool is based on multiple sub commands :
> - Use sub command `Generate` to generate phone numbers
> - Use sub command `Validate` to validate phone numbers
> - Use sub command `Info` to get country/calling phone codes
>
> 
> To install global tool from [nuget source](https://www.nuget.org/packages/PhoneCli), type command :
> - For stable version : `dotnet tool install -g PhoneCli --ignore-failed-sources`
> - For prerelease version : `dotnet tool install -g PhoneCli --version "*-*" --ignore-failed-sources`
>
>
> To install global tool from a local source path, type command :
> - `dotnet tool install -g --configfile .\nugets\local.config PhoneCli --version "*-*" --ignore-failed-sources`
>
> 
> To uninstall global tool, type command :
> - `dotnet tool uninstall -g PhoneCli`
>
>
> To run global tool, type commands :
> - `PhoneCli -h` to show help
> - `PhoneCli -s` to show settings
> - `PhoneCli Generate` to generate phone numbers
> - `PhoneCli Validate` to validate phone numbers
> - `PhoneCli Info` to get country/calling phone codes
> 
> 
> ![PhoneCli](Screenshots/PhoneCli.png)
>

**`Tools`** : net 6.0/7.0/8.0, command-line, spectre-console, libphonenumber, fluent-validation, fluent-assertions, xunit