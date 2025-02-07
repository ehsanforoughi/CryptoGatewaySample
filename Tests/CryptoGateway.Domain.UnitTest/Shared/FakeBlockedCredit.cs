using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.UserCredit;

namespace CryptoGateway.Domain.UnitTest.Shared;

public class FakeBlockedCredit : IBlockedCredit
{
    public Money Calculate(int userId, CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.IRR:
                return Money.FromDecimal(0, CurrencyType.IRR);
            case CurrencyType.USDT:
                return Money.FromDecimal(0, CurrencyType.USDT);
            default:
                throw new ArgumentException("Currency type is invalid");
        }
    }

    public async Task<Money> CalculateAsync(int userId, CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.IRR:
                return Money.FromDecimal(1000, CurrencyType.IRR);
            case CurrencyType.USDT:
                return Money.FromDecimal(10, CurrencyType.USDT);
            default:
                throw new ArgumentException("Currency type is invalid");
        }
    }
}