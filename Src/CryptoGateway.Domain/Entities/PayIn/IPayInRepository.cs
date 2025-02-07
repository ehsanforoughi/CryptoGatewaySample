using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.PayIn;

public interface IPayInRepository : IRepository<PayIn, PayInId>
{
    Task<bool> Exists(TxId txId);
}