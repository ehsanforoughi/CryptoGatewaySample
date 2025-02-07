using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;

namespace CryptoGateway.Domain.Entities.PayIn;

public interface IPayInDomainService
{
    Task ApplyPayInTransaction(PayInExternalId payInExternalId, CustodyAccountId custodyAccountId, int userId, 
        CustomerId customerId, Money value, TxId txId, IBlockedCredit blockedCredit);
}