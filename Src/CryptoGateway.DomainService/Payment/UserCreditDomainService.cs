using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.DomainService.Payment;

public class UserCreditDomainService : IUserCreditDomainService
{
    private readonly IUserRepository _repository;
    private readonly IBlockedCredit _blockedCredit;
    public UserCreditDomainService(IUserRepository repository, 
        IBlockedCredit availableCredit)
    {
        _repository = repository;
        _blockedCredit = availableCredit;
    }

    //public async Task<Money> GetRealCredit(int userId, CurrencyType currencyType)
    //{
    //    var user = await _repository.Load(new UserId(userId));

    //    if (user is null)
    //        throw new InvalidDataException("The userId is invalid");

    //    return user.GetUserCredit(currencyType).RealCredit;
    //}

    //public async Task<Money> GetAvailableCredit(int userId, CurrencyType currencyType)
    //{
    //    var user = await _repository.Load(new UserId(userId));

    //    if (user is null)
    //        throw new InvalidDataException("The userId is invalid");

    //    var blockedCredit = await _blockedCredit.CalculateAsync(new UserId(userId), currencyType);
    //    return user.GetUserCredit(currencyType).RealCredit - blockedCredit;
    //}

    public async Task<bool> EnsureEnoughCredit(int userId, Money value)
    {
        var user = await _repository.Load(userId);

        if (user is null)
            throw new InvalidDataException("The userId is invalid");

        return await user.GetUserCredit(value.CurrencyType).EnsureEnoughCredit(value, _blockedCredit);
    }

    //public async Task TakeOnCredit(int userId, Money value)
    //{
    //    var user = await _repository.Load(new UserId(userId));

    //    if (user is null)
    //        throw new InvalidDataException("The userId is invalid");

    //    user.GetUserCredit(value.CurrencyType).TakeOnCredit(value);
    //}
}