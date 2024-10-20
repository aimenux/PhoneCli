using App.Configuration;

namespace App.Extensions;

public static class StringExtensions
{
    public static bool IgnoreEquals(this string left, string right)
    {
        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
    
    public static bool IsValidPhoneType(this string phoneType)
    {
        return phoneType.IgnoreEquals(Constants.PhoneNumberTypes.Fixed)
               || phoneType.IgnoreEquals(Constants.PhoneNumberTypes.Mobile);
    }
    
    public static bool IsValidCountryCode(this string countryCode)
    {
        return Constants.SupportedCountryCodes.Contains(countryCode);
    }
}