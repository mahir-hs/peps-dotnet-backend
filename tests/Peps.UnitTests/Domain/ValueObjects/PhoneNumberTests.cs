using FluentAssertions;
using Peps.Domain.Exceptions;
using Peps.Domain.ValueObjects;

namespace Peps.UnitTests.Domain.ValueObjects;

public class PhoneNumberTests
{
    [Fact]
    public void Constructor_Should_CreatePhoneNumber_When_Valid()
    {
        var phoneNumber = new PhoneNumber("2025550123", "US");
        phoneNumber.Value.Should().Be("+12025550123");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_NumberIsNullOrWhitespace(string? input)
    {
        Action act = () => new PhoneNumber(input!, "US");
        act.Should().Throw<DomainException>()
           .WithMessage("Phone number cannot be empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_CountryCodeIsNullOrWhitespace(string? countryCode)
    {
        Action act = () => new PhoneNumber("2025550123", countryCode!);
        act.Should().Throw<DomainException>()
           .WithMessage("Country code cannot be empty.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_InvalidNumber()
    {
        Action act = () => new PhoneNumber("123", "US"); // too short to be valid
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid phone number.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_InvalidFormat()
    {
        Action act = () => new PhoneNumber("INVALID_NUMBER", "US");
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid phone number format.");
    }

    [Fact]
    public void Equals_Should_ReturnTrue_ForSameNumber()
    {
        var num1 = new PhoneNumber("2025550123", "US");
        var num2 = new PhoneNumber("2025550123", "US");

        num1.Equals(num2).Should().BeTrue();
        num1.Should().Be(num2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_ForDifferentNumbers()
    {
        var num1 = new PhoneNumber("2025550123", "US");
        var num2 = new PhoneNumber("2025550199", "US");

        num1.Equals(num2).Should().BeFalse();
        num1.Should().NotBe(num2);
    }

    [Fact]
    public void GetHashCode_Should_BeSame_ForEqualNumbers()
    {
        var num1 = new PhoneNumber("2025550123", "US");
        var num2 = new PhoneNumber("2025550123", "US");

        num1.GetHashCode().Should().Be(num2.GetHashCode());
    }

    [Fact]
    public void ToString_Should_ReturnFormattedValue()
    {
        var number = new PhoneNumber("2025550123", "US");
        number.ToString().Should().Be("+12025550123");
    }
}
