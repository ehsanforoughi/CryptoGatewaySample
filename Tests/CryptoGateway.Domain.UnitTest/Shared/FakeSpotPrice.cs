using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.Shared;

public class FakeSpotPrice : ISpotPriceProvider
{
    
    public async Task<Money> FindSpotPrice(CurrencyType from, CurrencyType to)
    {
        var fiatCurrencies = new List<CurrencyType> { CurrencyType.IRR, CurrencyType.USD };
        var cryptoCurrencies = new List<CurrencyType> { CurrencyType.USDT, CurrencyType.TRX };
        //if (!fiatCurrencies.Contains(from) || !cryptoCurrencies.Contains(to))
        //    throw new ArgumentException($"From {from} should be FIAT and to {to} should be Crypto");

        var rnd = new Random();
        var usdtPrice = rnd.Next(600000, 650000);
        const decimal trxUsdt = 0.1224M;
        switch (from, to)
        {
            case (CurrencyType.None, CurrencyType.USDT):
                return Money.FromNotClearAmount(CurrencyType.USDT);
            case (CurrencyType.IRR, CurrencyType.USDT):
                return Money.FromDecimal(usdtPrice, to);
            case (CurrencyType.TRX, CurrencyType.IRR):
                return Money.FromDecimal(usdtPrice * trxUsdt, to);
            case (CurrencyType.TRX, CurrencyType.USDT):
                return Money.FromDecimal(trxUsdt, to);
            case (CurrencyType.USDT, CurrencyType.USDT):
                return Money.FromDecimal(1, to);
            case (CurrencyType.USDT, CurrencyType.IRR):
                return Money.FromDecimal(usdtPrice, to);
            case (CurrencyType.USD, CurrencyType.USDT):
                return Money.FromDecimal(1, to);
            default:
                throw new ArgumentException($"Can not find spot price from {from} to {to}");
        }
    }
}