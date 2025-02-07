using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class FirstName : Value<FirstName>
{
    // Satisfy the serialization requirements
    protected FirstName() { }

    internal FirstName(string firstname) => Value = firstname;

    public static FirstName FromString(string firstname)
    {
        CheckValidity(firstname);
        return new FirstName(firstname);
    }

    public static implicit operator string(FirstName self) => self.Value;
    public string Value { get; internal set; }
    public static FirstName NoFirstName => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(FirstName), "FirstName cannot be empty");

        if (value.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(FirstName), "FirstName cannot be longer that 30 characters");
    }
}