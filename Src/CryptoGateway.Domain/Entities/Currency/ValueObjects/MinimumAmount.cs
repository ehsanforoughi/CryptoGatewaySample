using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class MinimumAmount : Value<MinimumAmount>
{
    // Satisfy the serialization requirements
    protected MinimumAmount() { }

    internal MinimumAmount(decimal minimumAmount) => Value = minimumAmount;

    public static MinimumAmount FromDecimal(decimal minimumAmount)
    {
        CheckValidity(minimumAmount);
        return new MinimumAmount(minimumAmount);
    }

    public static implicit operator decimal(MinimumAmount self) => self.Value;
    public decimal Value { get; internal set; }
    public static MinimumAmount NoMinimumAmount => new();
    private static void CheckValidity(decimal value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(MinimumAmount), "MinimumAmount cannot be empty");
    }
}