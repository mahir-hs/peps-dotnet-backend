using FluentAssertions;
using Peps.Domain.Exceptions;
using Peps.Domain.ValueObjects;

namespace Peps.UnitTests.Domain.ValueObjects;

public class LastNameTests
{
    [Fact]
    public void Constructor_Should_CreateLastName_When_Valid()
    {
        var lastName = new LastName("Jack");
        lastName.Value.Should().Be("Jack");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_ValueIsNullOrWhitespace(string? input)
    {
        Action act = () => new LastName(input!);
        act.Should().Throw<DomainException>()
           .WithMessage("Last name cannot be empty.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_TooLong()
    {
        var longName = new string('B', 51);
        Action act = () => new LastName(longName);
        act.Should().Throw<DomainException>()
           .WithMessage("Last name cannot be more than 50 characters long.");
    }

    [Fact]
    public void Equals_Should_ReturnTrue_ForSameNames()
    {
        var name1 = new LastName("Doe");
        var name2 = new LastName("Doe");

        name1.Equals(name2).Should().BeTrue();
        name1.Should().Be(name2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_ForDifferentNames()
    {
        var name1 = new LastName("Doe");
        var name2 = new LastName("Smith");

        name1.Equals(name2).Should().BeFalse();
        name1.Should().NotBe(name2);
    }

    [Fact]
    public void GetHashCode_Should_BeSame_ForEqualNames()
    {
        var name1 = new LastName("Doe");
        var name2 = new LastName("Doe");

        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }

    [Fact]
    public void ToString_Should_ReturnValue()
    {
        var lastName = new LastName("Doe");
        lastName.ToString().Should().Be("Doe");
    }
}