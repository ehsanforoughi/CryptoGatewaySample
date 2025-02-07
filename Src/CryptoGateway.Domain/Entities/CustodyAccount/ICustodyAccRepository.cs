using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;

namespace CryptoGateway.Domain.Entities.CustodyAccount;

public interface ICustodyAccRepository : IRepository<CustodyAccount, CustodyAccountId>
{
    Task<bool> Exists(int userId, CustomerId customerId);
    Task<CustodyAccount?> Load(CustodyAccExternalId custodyAccExternalId);
    Task<CustodyAccount?> Load(int userId, CustomerId customerId);
}