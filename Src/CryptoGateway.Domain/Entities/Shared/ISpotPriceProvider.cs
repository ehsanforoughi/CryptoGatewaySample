using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.Shared;

public interface ISpotPriceProvider
{
    Task<Money> FindSpotPrice(CurrencyType from, CurrencyType to);
}