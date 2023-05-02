using App.Configuration;
using App.Extensions;
using PhoneNumbers;

namespace App.Services.Phone;

public static class PhoneMapper
{
    public static string MapPhoneNumberType(PhoneNumberType phoneType)
    {
        return phoneType switch
        {
            PhoneNumberType.FIXED_LINE => Constants.FixedType,
            PhoneNumberType.MOBILE => Constants.MobileType,
            _ => throw new ArgumentOutOfRangeException(nameof(phoneType), $"Unexpected value {phoneType}")
        };
    }
    
    public static PhoneNumberType MapPhoneNumberType(string phoneType)
    {
        if (string.IsNullOrEmpty(phoneType))
        {
            return GetRandomPhoneType();
        }

        if (phoneType.IgnoreEquals(Constants.FixedType))
        {
            return PhoneNumberType.FIXED_LINE;
        }
        
        if (phoneType.IgnoreEquals(Constants.MobileType))
        {
            return PhoneNumberType.MOBILE;
        }

        throw new ArgumentOutOfRangeException(nameof(phoneType), $"Unexpected value {phoneType}");
    }
    
    public static string MapPhoneCountryCode(string countryCode)
    {
        return string.IsNullOrEmpty(countryCode) 
            ? GetRandomPhoneCountryCode() 
            : countryCode.ToUpper();
    }

    private static string GetRandomPhoneCountryCode()
    {
        var supportedCountryCodes = PhoneService.SupportedCountryCodes;
        var index = Random.Shared.Next(supportedCountryCodes.Count);
        return supportedCountryCodes.ElementAt(index);
    }
    
    private static PhoneNumberType GetRandomPhoneType()
    {
        var random = Random.Shared.Next(1000);
        return random % 2 == 0
            ? PhoneNumberType.MOBILE
            : PhoneNumberType.FIXED_LINE;
    }
}