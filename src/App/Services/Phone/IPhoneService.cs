namespace App.Services.Phone;

public interface IPhoneService
{
    IEnumerable<PhoneNumber> Generate(PhoneParameters parameters);
    bool TryValidate(PhoneParameters parameters, out PhoneNumber phoneNumber);
}