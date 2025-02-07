using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class MinimumWithdraw : Value<MinimumWithdraw>
{
    // Satisfy the serialization requirements
    protected MinimumWithdraw() { }

    internal MinimumWithdraw(decimal minimumWithdraw) => Value = minimumWithdraw;

    public static MinimumWithdraw FromDecimal(decimal minimumWithdraw)
    {
        CheckValidity(minimumWithdraw);
        return new MinimumWithdraw(minimumWithdraw);
    }

    public static implicit operator decimal(MinimumWithdraw self) => self.Value;
    public decimal Value { get; internal set; }
    public static MinimumWithdraw NoMinimumWithdraw => new();
    private static void CheckValidity(decimal value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(MinimumWithdraw), "MinimumWithdraw cannot be empty");
    }
}