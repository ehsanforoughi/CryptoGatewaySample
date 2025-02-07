using CryptoGateway.Domain.Entities.CurrencySpotPrice.ValueObjects;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.CurrencySpotPrice;

public interface ICurrencySpotPriceRepository : IRepository<CurrencySpotPrice, CurrencySpotPriceId>
{
}