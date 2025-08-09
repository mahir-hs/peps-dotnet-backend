using Peps.Domain.Enums;
using Peps.Domain.Exceptions;
using Peps.Domain.ValueObjects;

namespace Peps.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public Role Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string PasswordHash { get; private set; } = default!;
    public string? ProfilePictureUrl { get; private set; }
    private User() { }
    public User(Guid id, FirstName firstName, LastName lastName, Email email, PhoneNumber phoneNumber, Role role, string passwordHash)
    {
        Id = id != Guid.Empty ? id : throw new DomainException("User ID cannot be empty.");
        FirstName = firstName ?? throw new DomainException("First name is required.");
        LastName = lastName ?? throw new DomainException("Last name is required.");
        Email = email ?? throw new DomainException("Email is required.");
        PhoneNumber = phoneNumber ?? throw new DomainException("Phone number is required.");
        Role = role;
        PasswordHash = !string.IsNullOrWhiteSpace(passwordHash) ? passwordHash : throw new DomainException("Password hash cannot be empty.");

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }


    public void ChangePhoneNumber(PhoneNumber newPhoneNumber) => PhoneNumber = newPhoneNumber ?? throw new DomainException("Phone number cannot be empty.");

    public void ChangeEmail(Email newEmail) => Email = newEmail ?? throw new DomainException("Email cannot be empty.");

    public void ChangePassword(string newPasswordHash) =>
        PasswordHash = !string.IsNullOrWhiteSpace(newPasswordHash)
            ? newPasswordHash
            : throw new DomainException("Password hash cannot be empty.");
    public string FullName => $"{FirstName} {LastName}";

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void UpdateProfilePicture(string? url) => ProfilePictureUrl = url;
    public bool IsProvider() => Role == Role.Provider;
    public bool IsRequester() => Role == Role.Requester;
    public bool IsAdmin() => Role == Role.Admin;
}