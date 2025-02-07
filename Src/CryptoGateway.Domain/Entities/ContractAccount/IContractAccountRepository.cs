using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.Domain.Entities.ContractAccount;

public interface IContractAccountRepository : IRepository<ContractAccount, ContractAccountId>
{
    Task<bool> Exists(TxId txId);
}