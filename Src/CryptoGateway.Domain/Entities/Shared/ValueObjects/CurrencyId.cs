using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class CurrencyId : Value<CurrencyId>
{
    public int Value { get; internal set; }

    protected CurrencyId() { }

    public CurrencyId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(CurrencyId), "Currency id cannot be empty");

        Value = value;
    }

    public static implicit operator int(CurrencyId self) => self.Value;

    public static CurrencyId NoCurrency => new();
}