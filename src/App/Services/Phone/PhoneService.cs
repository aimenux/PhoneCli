using System.Text.RegularExpressions;
using PhoneNumbers;

namespace App.Services.Phone;

public class PhoneService : IPhoneService
{
    private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+|00)?(\d+)$", RegexOptions.Compiled);
    
    private static readonly PhoneNumberUtil PhoneNumberHelper = PhoneNumberUtil.GetInstance();

    public static readonly HashSet<string> SupportedCountryCodes = PhoneNumberHelper.GetSupportedRegions();

    public IEnumerable<PhoneNumber> Generate(PhoneParameters parameters)
    {
        var numbers = Enumerable
            .Range(1, 2 * parameters.MaxItems)
            .Select(_ => GeneratePhoneNumber(parameters))
            .Where(x => x != null)
            .Take(parameters.MaxItems);
        return numbers;
    }

    public bool TryValidate(PhoneParameters parameters, out PhoneNumber phoneNumber)
    {
        phoneNumber = null;
        
        if (!PhoneNumberRegex.IsMatch(parameters.PhoneNumber)) return false;
        var number = ParsePhoneNumber(parameters);
        if (number is null) return false;
        
        var phoneNumberType = PhoneNumberHelper.GetNumberType(number);
        if (!IsValidPhoneNumberType(phoneNumberType)) return false;
        var phoneType = PhoneMapper.MapPhoneNumberType(phoneNumberType);
        var countryCode = PhoneNumberHelper.GetRegionCodeForNumber(number);
        
        phoneNumber = new PhoneNumber
        {
            PhoneType = phoneType,
            CountryCode = countryCode,
            CallingCode = number.CountryCode,
            NationalNumber = number.NationalNumber.ToString()
        };
        
        return phoneNumber != null;
    }

    private static PhoneNumber GeneratePhoneNumber(PhoneParameters parameters)
    {
        var phoneType = PhoneMapper.MapPhoneNumberType(parameters.PhoneType);
        var countryCode = PhoneMapper.MapPhoneCountryCode(parameters.CountryCode);
        var number = PhoneNumberHelper.GetExampleNumberForType(countryCode, phoneType);
        if (number is null) return null;
        return new PhoneNumber
        {
            CountryCode = countryCode,
            CallingCode = number.CountryCode,
            PhoneType = PhoneMapper.MapPhoneNumberType(phoneType),
            NationalNumber = number.NationalNumber.ToString()
        };
    }
    
    private static PhoneNumbers.PhoneNumber ParsePhoneNumber(PhoneParameters parameters)
    {
        try
        {
            var countryCode = string.IsNullOrWhiteSpace(parameters.CountryCode)
                ? null
                : parameters.CountryCode.ToUpper();
            
            return PhoneNumberHelper.Parse(parameters.PhoneNumber, countryCode);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool IsValidPhoneNumberType(PhoneNumberType phoneNumberType)
    {
        return phoneNumberType is PhoneNumberType.MOBILE or PhoneNumberType.FIXED_LINE;
    }
}