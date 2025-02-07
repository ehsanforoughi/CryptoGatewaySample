using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class CurrencyNameFa : Value<CurrencyNameFa>
{
    // Satisfy the serialization requirements
    protected CurrencyNameFa() { }
    protected CurrencyNameFa(string nameFa) => Value = nameFa;

    public static CurrencyNameFa FromString(string nameFa)
    {
        CheckValidity(nameFa);
        return new CurrencyNameFa(nameFa);
    }

    public static implicit operator string(CurrencyNameFa self) => self.Value;
    public string Value { get; internal set; }
    public static CurrencyNameFa NoNameFa => new();

    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(CurrencyNameFa), "Currency persian name cannot be empty");

        if (value.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(CurrencyNameFa),
                "Currency persian name cannot be longer that 50 characters");
    }
}