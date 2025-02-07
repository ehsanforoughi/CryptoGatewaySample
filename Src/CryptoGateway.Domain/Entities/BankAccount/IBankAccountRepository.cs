using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount;

public interface IBankAccountRepository : IRepository<BankAccount, BankAccountId>
{
    Task<bool> Exists(int userId, CardNumber cardNumber);
}