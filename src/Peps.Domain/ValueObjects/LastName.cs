using Peps.Domain.Exceptions;

namespace Peps.Domain.ValueObjects;

public sealed class LastName : IEquatable<LastName>
{
    public string Value { get; private set; } = default!;

    private LastName() { }

    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Last name cannot be empty.");

        value = value.Trim();

        if (value.Length < 1)
            throw new DomainException("Last name must be at least 1 character long.");

        if (value.Length > 50)
            throw new DomainException("Last name cannot be more than 50 characters long.");

        if (HasInvalidCharacters(value))
            throw new DomainException("Last name contains invalid characters.");

        Value = value;
    }

    private static bool HasInvalidCharacters(string value) =>
        value.Any(char.IsControl);

    public bool Equals(LastName? other) =>
        other is not null && Value.Equals(other.Value, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is LastName other && Equals(other);

    public override int GetHashCode() =>
        Value.GetHashCode(StringComparison.Ordinal);

    public override string ToString() => Value;

    public static bool operator ==(LastName left, LastName right) => Equals(left, right);
    public static bool operator !=(LastName left, LastName right) => !Equals(left, right);

    public static implicit operator string(LastName lastName) => lastName.Value;
    public static explicit operator LastName(string value) => new(value);
}