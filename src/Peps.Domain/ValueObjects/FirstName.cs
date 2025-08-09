using Peps.Domain.Exceptions;

namespace Peps.Domain.ValueObjects;

public sealed class FirstName : IEquatable<FirstName>
{
    public string Value { get; private set; } = default!;

    private FirstName() { }

    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("First name cannot be empty.");

        value = value.Trim();

        if (value.Length < 1)
            throw new DomainException("First name must be at least 1 character long.");

        if (value.Length > 50)
            throw new DomainException("First name cannot be more than 50 characters long.");

        if (HasInvalidCharacters(value))
            throw new DomainException("First name contains invalid characters.");

        Value = value;
    }

    private static bool HasInvalidCharacters(string value) =>
        value.Any(char.IsControl);

    public bool Equals(FirstName? other) =>
        other is not null && Value.Equals(other.Value, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is FirstName other && Equals(other);

    public override int GetHashCode() =>
        Value.GetHashCode(StringComparison.Ordinal);

    public override string ToString() => Value;

    public static bool operator ==(FirstName left, FirstName right) => Equals(left, right);
    public static bool operator !=(FirstName left, FirstName right) => !Equals(left, right);

    public static implicit operator string(FirstName firstName) => firstName.Value;
    public static explicit operator FirstName(string value) => new(value);
}