namespace App.Services.Phone;

public interface IPhoneService
{
    Task<IEnumerable<PhoneNumber>> GenerateAsync(PhoneParameters parameters, CancellationToken cancellationToken = default);
    Task<PhoneNumber> ValidateAsync(PhoneParameters parameters, CancellationToken cancellationToken = default);
}