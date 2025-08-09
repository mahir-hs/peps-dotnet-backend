using Peps.Domain.Exceptions;
using PhoneNumbers;

namespace Peps.Domain.ValueObjects;

public class PhoneNumber : IEquatable<PhoneNumber>
{
    private static readonly PhoneNumberUtil PhoneNumberUtil = PhoneNumberUtil.GetInstance();

    public string Value { get; private set; } = default!;
    private PhoneNumber() { }

    public PhoneNumber(string number, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("Phone number cannot be empty.");

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException("Country code cannot be empty.");

        try
        {
            var parsedNumber = PhoneNumberUtil.Parse(number, countryCode);

            if (!PhoneNumberUtil.IsValidNumber(parsedNumber))
                throw new DomainException("Invalid phone number.");

            Value = PhoneNumberUtil.Format(parsedNumber, PhoneNumberFormat.E164);
        }
        catch (NumberParseException)
        {
            throw new DomainException("Invalid phone number format.");
        }
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }
    public override bool Equals(object? obj) => obj is PhoneNumber other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
    public static bool operator ==(PhoneNumber? left, PhoneNumber? right) => Equals(left, right);

    public static bool operator !=(PhoneNumber? left, PhoneNumber? right) => !Equals(left, right);

}