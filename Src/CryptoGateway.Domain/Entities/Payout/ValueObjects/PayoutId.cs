using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payout.ValueObjects;

public class PayoutId : Value<PayoutId>
{
    public int Value { get; internal set; }

    protected PayoutId() { }

    public PayoutId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(PayoutId), "Payout id cannot be empty");

        Value = value;
    }

    public static implicit operator int(PayoutId self) => self.Value;

    public static PayoutId NoPayoutId => new();
}