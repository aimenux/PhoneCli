using App.Configuration;
using App.Extensions;

namespace App.Services.Phone;

public class PhoneNumber
{
    public string CountryCode { get; init; }
    public string PhoneType { get; init; }
    public int CallingCode { get; init; }
    public string NationalNumber { get; init; }

    public bool IsFixed() => PhoneType.IgnoreEquals(Constants.FixedType);
    
    public bool IsMobile() => PhoneType.IgnoreEquals(Constants.MobileType);

    public override string ToString()
    {
        return $"(+{CallingCode}) {NationalNumber}";
    }
}