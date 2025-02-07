using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.UserCredit;

public interface IBlockedCredit
{
    Money Calculate(int userId, CurrencyType currencyType);
    Task<Money> CalculateAsync(int userId, CurrencyType currencyType);
}