using FluentAssertions;
using Peps.Domain.Entities;
using Peps.Domain.Enums;
using Peps.Domain.Exceptions;
using Peps.Domain.ValueObjects;

namespace Peps.UnitTests.Domain.Entities;

public class UserTests
{
    private readonly Guid _validId = Guid.NewGuid();
    private readonly FirstName _validFirstName = new("John");
    private readonly LastName _validLastName = new("Doe");
    private readonly Email _validEmail = new("john.doe@example.com");
    private readonly PhoneNumber _validPhoneNumber = new("2025550125", "US");
    private readonly string _validPasswordHash = "hashedpassword";

    [Fact]
    public void Constructor_Should_CreateUser_WithValidInputs()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash
        );

        user.Id.Should().Be(_validId);
        user.FirstName.Should().Be(_validFirstName);
        user.LastName.Should().Be(_validLastName);
        user.Email.Should().Be(_validEmail);
        user.PhoneNumber.Should().Be(_validPhoneNumber);
        user.Role.Should().Be(Role.Requester);
        user.PasswordHash.Should().Be(_validPasswordHash);
        user.IsActive.Should().BeTrue();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        user.FullName.Should().Be($"{_validFirstName} {_validLastName}");
    }

    [Fact]
    public void Constructor_Should_Throw_When_IdIsEmpty()
    {
        Action act = () =>
        {
            new User(Guid.Empty,
                     _validFirstName,
                     _validLastName,
                     _validEmail,
                     _validPhoneNumber,
                     Role.Admin,
                     _validPasswordHash);
        };

        act.Should().Throw<DomainException>().WithMessage("User ID cannot be empty.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_FirstNameIsNull()
    {
        Action act = () =>
        {
            new User(_validId,
                     null!,
                     _validLastName,
                     _validEmail,
                     _validPhoneNumber,
                     Role.Admin,
                     _validPasswordHash);
        };

        act.Should().Throw<DomainException>().WithMessage("First name is required.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_LastNameIsNull()
    {
        Action act = () =>
        {
            new User(_validId,
                     _validFirstName,
                     null!,
                     _validEmail,
                     _validPhoneNumber,
                     Role.Admin,
                     _validPasswordHash);
        };

        act.Should().Throw<DomainException>().WithMessage("Last name is required.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_EmailIsNull()
    {
        Action act = () =>
        {
            new User(_validId,
                     _validFirstName,
                     _validLastName,
                     null!,
                     _validPhoneNumber,
                     Role.Admin,
                     _validPasswordHash);
        };

        act.Should().Throw<DomainException>().WithMessage("Email is required.");
    }

    [Fact]
    public void Constructor_Should_Throw_When_PhoneNumberIsNull()
    {
        Action act = () =>
        {
            new User(_validId,
                     _validFirstName,
                     _validLastName,
                     _validEmail,
                     null!,
                     Role.Admin,
                     _validPasswordHash);
        };

        act.Should().Throw<DomainException>().WithMessage("Phone number is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_PasswordHashIsInvalid(string? password)
    {
        Action act = () =>
        {
            new User(_validId,
                     _validFirstName,
                     _validLastName,
                     _validEmail,
                     _validPhoneNumber,
                     Role.Admin,
                     password!);
        };
        act.Should().Throw<DomainException>().WithMessage("Password hash cannot be empty.");
    }

    [Fact]
    public void ChangePhoneNumber_Should_UpdatePhoneNumber()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        var newPhone = new PhoneNumber("2025560125", "US");

        user.ChangePhoneNumber(newPhone);

        user.PhoneNumber.Should().Be(newPhone);
    }

    [Fact]
    public void ChangePhoneNumber_Should_Throw_When_Null()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        Action act = () => user.ChangePhoneNumber(null!);

        act.Should().Throw<DomainException>().WithMessage("Phone number cannot be empty.");
    }

    [Fact]
    public void ChangeEmail_Should_UpdateEmail()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        var newEmail = new Email("new.email@example.com");

        user.ChangeEmail(newEmail);

        user.Email.Should().Be(newEmail);
    }

    [Fact]
    public void ChangeEmail_Should_Throw_When_Null()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        Action act = () => user.ChangeEmail(null!);

        act.Should().Throw<DomainException>().WithMessage("Email cannot be empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ChangePassword_Should_Throw_Error_When_Invalid(string? newPassword)
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        Action act = () => user.ChangePassword(newPassword!);

        act.Should().Throw<DomainException>().WithMessage("Password hash cannot be empty.");
    }

    [Fact]
    public void Activate_Should_SetIsActiveTrue()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        user.Deactivate();

        user.IsActive.Should().BeFalse();

        user.Activate();

        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveFalse()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        user.Deactivate();

        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void UpdateProfilePicture_Should_UpdateUrl()
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            Role.Requester,
            _validPasswordHash);

        user.UpdateProfilePicture("https://example.com/pic.png");

        user.ProfilePictureUrl.Should().Be("https://example.com/pic.png");
    }

    [Theory]
    [InlineData(Role.Provider, true, false, false)]
    [InlineData(Role.Requester, false, true, false)]
    [InlineData(Role.Admin, false, false, true)]
    public void RoleHelpers_Should_ReturnExpectedResults(Role role, bool isProvider, bool isRequester, bool isAdmin)
    {
        var user = new User(
            _validId,
            _validFirstName,
            _validLastName,
            _validEmail,
            _validPhoneNumber,
            role,
            _validPasswordHash);

        user.IsProvider().Should().Be(isProvider);
        user.IsRequester().Should().Be(isRequester);
        user.IsAdmin().Should().Be(isAdmin);
    }

    [Fact]
    public void ChangePassword_Should_UpdatePassword_When_Valid()
    {
        var user = new User(_validId, _validFirstName, _validLastName, _validEmail, _validPhoneNumber, Role.Requester, _validPasswordHash);
        var newPasswordHash = "newhashedpassword";
        user.ChangePassword(newPasswordHash);
        user.PasswordHash.Should().Be(newPasswordHash);
    }

}