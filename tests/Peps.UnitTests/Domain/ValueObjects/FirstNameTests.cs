using FluentAssertions;
using Peps.Domain.Exceptions;
using Peps.Domain.ValueObjects;

namespace Peps.UnitTests.Domain.ValueObjects;

public class FirstNameTests
{
    [Fact]
    public void Constructor_Should_CreateFirstName_When_Valid()
    {
        var firstName = new FirstName("John");
        firstName.Value.Should().Be("John");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_ValueIsNullOrWhitespace(string? input)
    {
        Action act = () => new FirstName(input!);
        act.Should().Throw<DomainException>()
           .WithMessage("First name cannot be empty.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_TooLong()
    {
        var longName = new string('A', 51); // exceeds 50 chars
        Action act = () => new FirstName(longName);
        act.Should().Throw<DomainException>()
           .WithMessage("First name cannot be more than 50 characters long.");
    }

    [Fact]
    public void Equals_Should_ReturnTrue_ForSameNames()
    {
        var name1 = new FirstName("John");
        var name2 = new FirstName("John");

        name1.Equals(name2).Should().BeTrue();
        name1.Should().Be(name2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_ForDifferentNames()
    {
        var name1 = new FirstName("John");
        var name2 = new FirstName("Jane");

        name1.Equals(name2).Should().BeFalse();
        name1.Should().NotBe(name2);
    }

    [Fact]
    public void GetHashCode_Should_BeSame_ForEqualNames()
    {
        var name1 = new FirstName("John");
        var name2 = new FirstName("John");

        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }

    [Fact]
    public void ToString_Should_ReturnValue()
    {
        var firstName = new FirstName("John");
        firstName.ToString().Should().Be("John");
    }
}
