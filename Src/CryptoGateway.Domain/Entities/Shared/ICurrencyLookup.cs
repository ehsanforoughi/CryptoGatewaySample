using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.Shared;

public interface ICurrencyLookup
{
    Currency.Currency FindCurrency(CurrencyType currencyType);
    IEnumerable<Currency.Currency> AllActiveCurrencies();
}