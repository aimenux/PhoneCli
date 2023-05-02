using PhoneNumbers;

namespace App.Services.Phone;

public class PhoneService : IPhoneService
{
    private static readonly PhoneNumberUtil PhoneNumberHelper = PhoneNumberUtil.GetInstance();

    public static readonly HashSet<string> SupportedCountryCodes = PhoneNumberHelper.GetSupportedRegions();

    public Task<IEnumerable<PhoneNumber>> GenerateAsync(PhoneParameters parameters, CancellationToken cancellationToken)
    {
        var numbers = Enumerable
            .Range(1, 2 * parameters.MaxItems)
            .Select(_ => GeneratePhoneNumber(parameters))
            .Where(x => x != null)
            .Take(parameters.MaxItems);
        return Task.FromResult(numbers);
    }

    public Task<PhoneNumber> ValidateAsync(PhoneParameters parameters, CancellationToken cancellationToken)
    {
        var countryCode = string.IsNullOrEmpty(parameters.CountryCode) 
            ? null 
            : parameters.CountryCode.ToUpper();
        var number = ParsePhoneNumber(parameters.PhoneNumber, countryCode);
        var numberType = PhoneNumberHelper.GetNumberType(number);
        var phoneNumber = number is null
            ? null
            : new PhoneNumber
            {
                CountryCode = countryCode,
                CallingCode = number.CountryCode,
                PhoneType = PhoneMapper.MapPhoneNumberType(numberType),
                NationalNumber = number.NationalNumber.ToString()
            };
        return Task.FromResult(phoneNumber);
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
}