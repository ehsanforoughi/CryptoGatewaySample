using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class Password : Value<Password>
{
    // Satisfy the serialization requirements
    protected Password() { }

    internal Password(string password) => Value = password;

    public static Password FromString(string password)
    {
        CheckValidity(password);
        return new Password(password);
    }

    public static implicit operator string(Password self) => self.Value;
    public string Value { get; private set; }

    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(Password), "Password cannot be empty");

        if (value.Length < 8)
            throw new ArgumentOutOfRangeException(nameof(Password), "Password cannot be shorter than 8 characters");
    }
}