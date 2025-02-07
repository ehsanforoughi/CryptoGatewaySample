using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.UserCredit;

public interface IUserCreditDomainService
{
    //Task<Money> GetRealCredit(int userId, CurrencyType currencyType);
    //Task<Money> GetAvailableCredit(int userId, CurrencyType currencyType);
    Task<bool> EnsureEnoughCredit(int userId, Money value);
    //Task TakeOnCredit(int userId, Money value);
}