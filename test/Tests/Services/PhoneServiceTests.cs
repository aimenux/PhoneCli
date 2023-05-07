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
    [InlineData("fr", "fixed", 100)]
    [InlineData("fr", "mobile", 100)]
    [InlineData("be", "fixed", 100)]
    [InlineData("be", "mobile", 100)]
    [InlineData("tn", "fixed", 100)]
    [InlineData("tn", "mobile", 100)]
    [InlineData("dz", "fixed", 100)]
    [InlineData("dz", "mobile", 100)]       
    [InlineData("ma", "fixed", 100)]
    [InlineData("ma", "mobile", 100)]    
    [InlineData("es", "fixed", 100)]
    [InlineData("es", "mobile", 100)]
    [InlineData("it", "fixed", 100)]
    [InlineData("it", "mobile", 100)]
    [InlineData("pt", "fixed", 100)]
    [InlineData("pt", "mobile", 100)]
    [InlineData("gb", "fixed", 100)]
    [InlineData("gb", "mobile", 100)]     
    public void Should_Generate_Valid_Phone_Numbers(string countryCode, string phoneType, int maxItems)
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
        var phoneNumbers = service.Generate(parameters).ToList();

        // assert
        phoneNumbers.Should().NotBeEmpty().And.HaveCount(maxItems);
        foreach (var phoneNumber in phoneNumbers)
        {
            var nationalNumber = phoneNumber.NationalNumber;
            var phoneParameters = new PhoneParameters
            {
                PhoneNumber = nationalNumber,
                CountryCode = countryCode
            };
            var isValid = service.TryValidate(phoneParameters, out _);
            isValid.Should().BeTrue($"{nationalNumber} is not valid");
        }
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
    
    [Theory]
    [ClassData(typeof(Keywords))]
    public void Should_Get_Phone_Codes(string[] keywords)
    {
        // arrange
        var parameters = new PhoneParameters
        {
            KeyWords = keywords,
            MaxItems = 10
        };
        
        var service = new PhoneService();

        // act
        var phoneCodes = service.GetPhoneCodes(parameters);

        // assert
        phoneCodes.Should().NotBeNullOrEmpty();
    }
    
    private class Keywords : TheoryData<string[]>
    {
        public Keywords()
        {
            Add(null);
            Add(Array.Empty<string>());
            Add(new[] { "fr" });
            Add(new[] { "fr", "be" });
        }
    }
}