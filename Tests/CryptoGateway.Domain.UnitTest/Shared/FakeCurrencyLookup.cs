using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Currency.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.Shared;

public class FakeCurrencyLookup : ICurrencyLookup
{
    private readonly List<Entities.Currency.Currency> _currencies;
    public FakeCurrencyLookup()
    {
        _currencies = new List<Entities.Currency.Currency>
        {
            new(CurrencyType.IRR, DecimalPlaces.FromByte(0),
                IsActive.FromBoolean(true), IsFiat.FromBoolean(true)),
            new(CurrencyType.USDT, DecimalPlaces.FromByte(6),
                IsActive.FromBoolean(true), IsFiat.FromBoolean(false)),
            new(CurrencyType.USD, DecimalPlaces.FromByte(2),
                IsActive.FromBoolean(true), IsFiat.FromBoolean(true)),
            new(CurrencyType.TRX, DecimalPlaces.FromByte(6),
                IsActive.FromBoolean(true), IsFiat.FromBoolean(false))
        };
    }
    public Entities.Currency.Currency FindCurrency(CurrencyType currencyType)
    {
        var currency = _currencies.FirstOrDefault(x => x.Type == currencyType);
        return currency ?? Entities.Currency.Currency.None;
    }

    public IEnumerable<Entities.Currency.Currency> AllActiveCurrencies()
    {
        return _currencies.Where(x => x.IsActive);
    }
}