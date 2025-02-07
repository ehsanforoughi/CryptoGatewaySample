using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payout.ValueObjects;

public class PayoutDesc : Value<PayoutDesc>
{
    // Satisfy the serialization requirements
    protected PayoutDesc() { }

    internal PayoutDesc(string payoutDesc) => Value = payoutDesc;

    public static PayoutDesc FromString(string payoutDesc)
    {
        CheckValidity(payoutDesc);
        return new PayoutDesc(payoutDesc);
    }

    public static implicit operator string(PayoutDesc self) => self.Value;
    public string Value { get; internal set; }
    public static PayoutDesc NoPayoutDesc => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(PayoutDesc), "PayoutDesc cannot be empty");

        if (value.Length > 70)
            throw new ArgumentOutOfRangeException(nameof(PayoutDesc), "PayoutDesc cannot be longer that 70 characters");
    }
}