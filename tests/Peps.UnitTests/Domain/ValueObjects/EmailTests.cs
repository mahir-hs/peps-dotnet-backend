using FluentAssertions;
using Peps.Domain.Exceptions;
using Peps.Domain.ValueObjects;

namespace Peps.UnitTests.Domain.ValueObjects;

public class EmailTests
{
    private readonly Email _validEmail = new("john.doe@example.com");

    [Fact]
    public void Constructor_Should_CreateEmail_WithValidValue()
    {
        var email = new Email(_validEmail);
        email.Should().Be(_validEmail);
    }

    [Fact]
    public void Constructor_Should_Throw_When_InvalidEmail()
    {
        Action act = () => new Email("invalid-email");
        act.Should().Throw<DomainException>().WithMessage("Invalid email format.");
    }

    [Theory]
    [InlineData("  JOHN.DOE@example.com  ", "john.doe@example.com")]
    [InlineData("Jane.Smith@EXAMPLE.COM", "jane.smith@example.com")]
    public void Constructor_Should_TrimAndNormalizeEmail(string input, string expected)
    {
        var email = new Email(input);
        email.Value.Should().Be(expected);
    }

    [Fact]
    public void Emails_Should_BeEqual_IgnoringCase()
    {
        var email1 = new Email("user@example.com");
        var email2 = new Email("USER@EXAMPLE.COM");

        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
        email1.Equals(email2).Should().BeTrue();
    }

    [Fact]
    public void Emails_Should_NotBeEqual_WhenDifferent()
    {
        var email1 = new Email("user1@example.com");
        var email2 = new Email("user2@example.com");

        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void ToString_Should_ReturnEmailValue()
    {
        var email = new Email("test@example.com");
        email.ToString().Should().Be("test@example.com");
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        Email email = new Email("abc@example.com");
        string emailString = email; 
        emailString.Should().Be("abc@example.com");
    }

    [Fact]
    public void ExplicitConversion_FromString_ShouldCreateEmail()
    {
        string emailStr = "def@example.com";
        Email email = (Email)emailStr;  
        email.Value.Should().Be(emailStr);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_EmailIsNullOrWhitespace(string? input)
    {
        Action act = () => new Email(input!);
        act.Should().Throw<DomainException>().WithMessage("Email cannot be empty.");
    }


}