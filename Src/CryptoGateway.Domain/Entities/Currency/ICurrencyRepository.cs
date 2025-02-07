using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency;

public interface ICurrencyRepository : IRepository<Currency, CurrencyId>
{
    Task<Currency> Load(CurrencyType currencyType);
}