using App.Extensions;
using PhoneNumbers;

namespace App.Services.Phone;

public class PhoneService : IPhoneService
{
    private static readonly PhoneNumberUtil PhoneNumberHelper = PhoneNumberUtil.GetInstance();

    public static readonly HashSet<string> SupportedCountryCodes = PhoneNumberHelper.GetSupportedRegions();

    public Task<IEnumerable<PhoneNumber>> GenerateAsync(PhoneParameters parameters, CancellationToken cancellationToken)
    {
        var count = 2 * parameters.MaxItems;
        var numbers = Enumerable
            .Range(1, count)
            .Select(_ => GeneratePhoneNumber(parameters.CountryCode, parameters.PhoneType))
            .Where(x => x != null && (x.IsFixed() || x.IsMobile()))
            .Take(parameters.MaxItems);
        return Task.FromResult(numbers);
    }

    public Task<PhoneNumber> ValidateAsync(PhoneParameters parameters, CancellationToken cancellationToken)
    {
        var countryCode = parameters.CountryCode.IgnoreEquals("Any") 
            ? null 
            : parameters.CountryCode.ToUpper();
        var number = PhoneNumberHelper.Parse(parameters.PhoneNumber, countryCode);
        var phoneNumber = number is null
            ? null
            : new PhoneNumber
            {
                CountryCode = countryCode,
                CallingCode = number.CountryCode,
                PhoneType = GetPhoneNumberType(number),
                NationalNumber = number.NationalNumber.ToString()
            };
        return Task.FromResult(phoneNumber);
    }

    private static PhoneNumber GeneratePhoneNumber(string code, string type)
    {
        var countryCode = GetPhoneCountryCode(code);
        var phoneType = GetPhoneNumberType(type);
        var number = PhoneNumberHelper.GetExampleNumberForType(countryCode, phoneType);
        if (number is null) return null;
        return new PhoneNumber
        {
            CountryCode = countryCode,
            CallingCode = number.CountryCode,
            PhoneType = GetPhoneNumberType(number),
            NationalNumber = number.NationalNumber.ToString()
        };
    }

    private static string GetPhoneCountryCode(string countryCode)
    {
        return countryCode.IgnoreEquals("Any") 
            ? GetRandomPhoneCountryCode() 
            : countryCode.ToUpper();
    }

    private static PhoneNumberType GetPhoneNumberType(string phoneType)
    {
        return phoneType.ToLower() switch
        {
            "fixed" => PhoneNumberType.FIXED_LINE,
            "mobile" => PhoneNumberType.MOBILE,
            "any" => GetRandomPhoneType(),
            _ => throw new ArgumentOutOfRangeException(nameof(phoneType), $"Unexpected value {phoneType}")
        };
    }

    private static string GetPhoneNumberType(PhoneNumbers.PhoneNumber phoneNumber)
    {
        var numberType = PhoneNumberHelper.GetNumberType(phoneNumber);
        return numberType switch
        {
            PhoneNumberType.FIXED_LINE => "Fixed",
            PhoneNumberType.MOBILE => "Mobile",
            PhoneNumberType.FIXED_LINE_OR_MOBILE => "Any",
            _ => throw new ArgumentOutOfRangeException(nameof(numberType), $"Unexpected value {numberType}")
        };
    }
    
    private static PhoneNumberType GetRandomPhoneType()
    {
        var random = Random.Shared.Next(1000);
        return random % 2 == 0
            ? PhoneNumberType.MOBILE
            : PhoneNumberType.FIXED_LINE;
    }
    
    private static string GetRandomPhoneCountryCode()
    {
        var index = Random.Shared.Next(SupportedCountryCodes.Count);
        return SupportedCountryCodes.ElementAt(index);
    }
}