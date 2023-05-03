namespace App.Services.Phone;

public class PhoneNumber
{
    public string CountryCode { get; init; }
    public string PhoneType { get; init; }
    public int CallingCode { get; init; }
    public string NationalNumber { get; init; }

    public override string ToString()
    {
        return $"+{CallingCode}{NationalNumber}";
    }
}