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

        if (string.IsNullOrWhiteSpace(parameters.PhoneNumber)) return false;
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

        var (fixedNumber, prefixNumber) = FixNationalNumber(number.NationalNumber, countryCode, parameters.PhoneType);
        var randomNumber = RandomizeNationalNumber(fixedNumber, countryCode, parameters.PhoneType);
        var nationalNumber = $"{prefixNumber}{randomNumber}";
        
        return new PhoneNumber
        {
            CountryCode = countryCode,
            CallingCode = number.CountryCode,
            NationalNumber = nationalNumber,
            PhoneType = PhoneMapper.MapPhoneNumberType(phoneType)
        };
    }
    
    private static PhoneNumbers.PhoneNumber ParsePhoneNumber(PhoneParameters parameters)
    {
        var countryCode = string.IsNullOrWhiteSpace(parameters.CountryCode)
            ? null
            : parameters.CountryCode.ToUpper();

        return ParsePhoneNumber(parameters.PhoneNumber, countryCode);
    }
    
    private static PhoneNumbers.PhoneNumber ParsePhoneNumber(string phoneNumber, string countryCode)
    {
        try
        {
            return PhoneNumberHelper.Parse(phoneNumber, countryCode);
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private static (ulong fixedNumber, string prefixNumber) FixNationalNumber(ulong nationalNumber, string countryCode, string phoneType)
    {
        return (countryCode?.ToUpper(), phoneType?.ToUpper()) switch
        {
            ("TN", "FIXED") => (73000000, ""),
            ("IT", "FIXED") => (30000000, "0"),
            _ => (nationalNumber, "")
        };
    }
    
    private static string RandomizeNationalNumber(ulong nationalNumber, string countryCode, string phoneType)
    {
        var randomNationalNumber = RandomNationalNumber(nationalNumber);
        var number = ParsePhoneNumber(randomNationalNumber, countryCode);
        return number is null ? nationalNumber.ToString() : randomNationalNumber;
    }

    private static string RandomNationalNumber(ulong nationalNumber)
    {
        var maxValue = (long) nationalNumber / 1000;
        var seed = (ulong)Random.Shared.NextInt64(maxValue);
        var result = nationalNumber + seed;
        return result.ToString();
    }

    private static bool IsValidPhoneNumberType(PhoneNumberType phoneNumberType)
    {
        return phoneNumberType is PhoneNumberType.MOBILE or PhoneNumberType.FIXED_LINE;
    }
}