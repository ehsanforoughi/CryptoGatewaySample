using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class CurrencyCode : Value<CurrencyCode>
{
    // Satisfy the serialization requirements
    protected CurrencyCode() { }
    protected CurrencyCode(string code) => Value = code.ToUpper();

    public static CurrencyCode FromString(string code)
    {
        CheckValidity(code);
        return new CurrencyCode(code);
    }

    public static implicit operator string(CurrencyCode self) => self.Value;
    public string Value { get; internal set; }
    public static CurrencyCode NoCode => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(CurrencyCode), "Currency code cannot be empty");

        if (value.Length > 10)
            throw new ArgumentOutOfRangeException(nameof(CurrencyCode), "Currency code cannot be longer that 10 characters");
    }
}