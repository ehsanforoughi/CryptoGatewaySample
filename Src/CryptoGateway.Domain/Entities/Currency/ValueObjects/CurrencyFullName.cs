using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class CurrencyFullName : Value<CurrencyFullName>
{
    // Satisfy the serialization requirements
    protected CurrencyFullName() { }
    protected CurrencyFullName(string fullName) => Value = fullName;

    public static CurrencyFullName FromString(string fullName)
    {
        CheckValidity(fullName);
        return new CurrencyFullName(fullName);
    }

    public static implicit operator string(CurrencyFullName self) => self.Value;
    public string Value { get; internal set; }
    public static CurrencyFullName NoFullName => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(CurrencyFullName), "Currency full name cannot be empty");

        if (value.Length > 50)
            throw new ArgumentOutOfRangeException(nameof(CurrencyFullName), "Currency full name cannot be longer that 50 characters");
    }
}