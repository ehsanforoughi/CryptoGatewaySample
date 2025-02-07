using CryptoGateway.Domain.Entities.Currency;
using CryptoGateway.Domain.Entities.Currency.ValueObjects;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.WebApi.Infrastructure;

public class FixedCurrencyLookup : ICurrencyLookup
{
    private readonly List<Currency> _currencies;
    public FixedCurrencyLookup()
    {
        _currencies = new List<Currency>
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

    public Currency FindCurrency(CurrencyType currencyType)
    {
        var currency = _currencies.FirstOrDefault(x => x.Type == currencyType);
        return currency ?? Currency.None;
    }

    public IEnumerable<Currency> AllActiveCurrencies()
    {
        return _currencies.Where(x => x.IsActive);
    }
}