using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.CurrencySpotPrice.ValueObjects;

public class CurrencySpotPriceId : Value<CurrencySpotPriceId>
{
    public int Value { get; internal set; }

    protected CurrencySpotPriceId() { }

    public CurrencySpotPriceId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(CurrencySpotPriceId), "Currency id cannot be empty");

        Value = value;
    }

    public static implicit operator int(CurrencySpotPriceId self) => self.Value;

    public static CurrencySpotPriceId NoCurrency => new();
}