using App.Services.Phone;
using FluentAssertions;

namespace Tests.Services;

public class PhoneServiceTests
{
    [Theory]
    [InlineData("fr", null, 1)]
    [InlineData("be", null, 2)]
    [InlineData("tn", null, 3)]
    [InlineData("fr", "fixed", 1)]
    [InlineData("be", "fixed", 2)]
    [InlineData("tn", "fixed", 3)]
    [InlineData("fr", "mobile", 1)]
    [InlineData("be", "mobile", 2)]
    [InlineData("tn", "mobile", 3)]
    public void Should_Generate_Phone_Numbers(string countryCode, string phoneType, int maxItems)
    {
        // arrange
        var parameters = new PhoneParameters
        {
            CountryCode = countryCode,
            PhoneType = phoneType,
            MaxItems = maxItems
        };
        
        var service = new PhoneService();

        // act
        var phoneNumbers = service.Generate(parameters);

        // assert
        phoneNumbers.Should().NotBeNullOrEmpty().And.HaveCount(maxItems);
    }
    
    [Theory]
    [InlineData("50298873", "TN", true)]
    [InlineData("785412563", "FR", true)]
    [InlineData("+21650298873", null, true)]
    [InlineData("+33785412563", null, true)]
    [InlineData("xyz", null, false)]
    [InlineData("123", null, false)]
    [InlineData("x21650298873", null, false)]
    [InlineData("x+21650298873", null, false)]
    public void Should_Validate_Phone_Numbers(string phoneNumber, string countryCode, bool expectedIsValid)
    {
        // arrange
        var parameters = new PhoneParameters
        {
            CountryCode = countryCode,
            PhoneNumber = phoneNumber
        };
        
        var service = new PhoneService();

        // act
        var isValid = service.TryValidate(parameters, out var number);

        // assert
        isValid.Should().Be(expectedIsValid);
        if (!isValid) return;
        number.Should().NotBeNull();
        number.NationalNumber.Should().NotBeNullOrEmpty();
        number.CountryCode.Should().NotBeNullOrEmpty();
        number.PhoneType.Should().NotBeNullOrEmpty();
    }
}