using CryptoGateway.Domain.Entities.CurrencySpotPrice;
using CryptoGateway.Domain.Entities.CurrencySpotPrice.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class CurrencySpotPriceRepository : ICurrencySpotPriceRepository
{
    public Task<CurrencySpotPrice> Load(CurrencySpotPriceId id)
    {
        throw new NotImplementedException();
    }

    public void Add(CurrencySpotPrice entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(CurrencySpotPriceId id)
    {
        throw new NotImplementedException();
    }
}