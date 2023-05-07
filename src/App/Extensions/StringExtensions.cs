using App.Configuration;
using App.Services.Phone;

namespace App.Extensions;

public static class StringExtensions
{
    public static bool IgnoreEquals(this string left, string right)
    {
        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
    
    public static bool IgnoreContains(this string left, string right)
    {
        return left is not null && left.Contains(right, StringComparison.OrdinalIgnoreCase);
    }
    
    public static bool IsValidPhoneType(this string phoneType)
    {
        return phoneType.IgnoreEquals(Constants.FixedType)
               || phoneType.IgnoreEquals(Constants.MobileType);
    }
    
    public static bool IsValidCountryCode(this string countryCode)
    {
        return PhoneService.SupportedCountryCodes.Contains(countryCode, StringComparer.OrdinalIgnoreCase);
    }
}