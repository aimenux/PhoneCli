using PhoneNumbers;

namespace App.Configuration;

public static class Constants
{
    public static class PhoneNumberTypes
    {
        public const string Fixed = "Fixed";
    
        public const string Mobile = "Mobile";
    }
    
    public static readonly HashSet<string> SupportedCountryCodes = new(PhoneNumberUtil.GetInstance().GetSupportedRegions(), StringComparer.OrdinalIgnoreCase);
}