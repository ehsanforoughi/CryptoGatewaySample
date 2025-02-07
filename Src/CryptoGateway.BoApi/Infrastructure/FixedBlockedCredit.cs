using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.UserCredit;

namespace CryptoGateway.BoApi.Infrastructure;

public class FixedBlockedCredit : IBlockedCredit
{
    private readonly IPayoutRepository _payoutRepository;
    public FixedBlockedCredit(IPayoutRepository payoutRepository)
    {
        _payoutRepository = payoutRepository;
    }

    public Money Calculate(int userId, CurrencyType currencyType)
    {
        var payouts = _payoutRepository.Load(userId, currencyType).Result;

        var blockedCredit = Money.FromDecimal(0, currencyType);
        if (payouts is not null && payouts.Any())
            blockedCredit =
                Money.FromDecimal(payouts.Where(x => x.State.Equals(ApprovingState.Created))
                    .Sum(x => x.Value.Amount), currencyType);

        return blockedCredit;
    }

    public async Task<Money> CalculateAsync(int userId, CurrencyType currencyType)
    {
        var payouts = await _payoutRepository.Load(userId, currencyType);

        var blockedCredit = Money.FromDecimal(0, currencyType);
        if (payouts is not null && payouts.Any())
            blockedCredit =
                Money.FromDecimal(payouts.Where(x => x.State.Equals(ApprovingState.Created))
                                         .Sum(x => x.Value.Amount), currencyType);

        return blockedCredit;
    }
}