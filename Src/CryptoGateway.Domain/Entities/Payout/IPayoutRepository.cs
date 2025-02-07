using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Payout.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.Payout;

public interface IPayoutRepository : IRepository<Payout, PayoutId>
{
    Task<List<Payout>> Load(int userId, CurrencyType currencyType);
}