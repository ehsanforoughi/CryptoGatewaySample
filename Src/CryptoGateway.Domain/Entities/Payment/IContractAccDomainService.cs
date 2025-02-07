using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.Payment;

public interface IContractAccDomainService
{
    Task<ContractAccount.ContractAccount> GetLatestContractAccount(int userId, CustomerId customerId);
}