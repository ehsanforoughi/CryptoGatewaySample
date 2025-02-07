using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.DomainService.ExternalWebServices.RamzPlus;

namespace CryptoGateway.BoApi.Infrastructure;

public class FixedSpotPrice : ISpotPriceProvider
{
    private readonly IRamzPlusPublicApi _ramzPlusPublicApi;
    public FixedSpotPrice(IRamzPlusPublicApi ramzPlusPublicApi)
    {
        _ramzPlusPublicApi = ramzPlusPublicApi;
    }
    public async Task<Money> FindSpotPrice(CurrencyType from, CurrencyType to)
    {
        var tetherMarket = await _ramzPlusPublicApi.GetTetherMarket();
        if (tetherMarket == null || tetherMarket.IsSuccessful == false) throw new ArgumentException($"Tether Market has a problem");

        switch (from, to)
        {
            case (CurrencyType.IRR, CurrencyType.USDT):
                return Money.FromDecimal(tetherMarket.Result.SellPrice, to);
            //case (CurrencyType.TRX, CurrencyType.IRR):
            //    return Money.FromDecimal(usdtPrice * trxUsdt, to);
            //case (CurrencyType.TRX, CurrencyType.USDT):
            //    return Money.FromDecimal(trxUsdt, to);
            case (CurrencyType.USDT, CurrencyType.USDT):
                return Money.FromDecimal(1, to);
            //case (CurrencyType.USDT, CurrencyType.IRR):
            //    return Money.FromDecimal(usdtPrice, to);
            case (CurrencyType.USD, CurrencyType.USDT):
                return Money.FromDecimal(1, to);
            default:
                throw new ArgumentException($"Cannot find spot price from {from} to {to}");
        }
    }
}