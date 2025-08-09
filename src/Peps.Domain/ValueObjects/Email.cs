using Peps.Domain.Exceptions;

namespace Peps.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; private set; } = default!;
    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        value = value?.Trim() ?? throw new DomainException("Email cannot be empty.");

        if (!IsValidEmail(value))
            throw new DomainException("Invalid email format.");

        Value = value.ToLowerInvariant();
    }

    public bool Equals(Email? other) => other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is Email email && Equals(email);

    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public override string ToString() => Value;

    public static bool operator ==(Email left, Email right) => Equals(left, right);
    public static bool operator !=(Email left, Email right) => !Equals(left, right);

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string value) => new(value);

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
