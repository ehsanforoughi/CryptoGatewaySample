using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class LastName : Value<LastName>
{
    // Satisfy the serialization requirements
    protected LastName() { }

    internal LastName(string lastname) => Value = lastname;

    public static LastName FromString(string lastname)
    {
        CheckValidity(lastname);
        return new LastName(lastname);
    }

    public static implicit operator string(LastName self) => self.Value;
    public string Value { get; internal set; }
    public static LastName NoLastName => new();

    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(LastName), "LastName cannot be empty");

        if (value.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(LastName), "LastName cannot be longer that 30 characters");
    }
}