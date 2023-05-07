namespace App.Services.Phone;

public class PhoneParameters
{
    public string PhoneNumber { get; init; }
    public string CountryCode { get; init; }
    public string PhoneType { get; init; }
    public int MaxItems { get; init; }
}