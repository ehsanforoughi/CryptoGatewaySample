using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class MinimumDeposit : Value<MinimumDeposit>
{
    // Satisfy the serialization requirements
    protected MinimumDeposit() { }

    internal MinimumDeposit(decimal minimumDeposit) => Value = minimumDeposit;

    public static MinimumDeposit FromDecimal(decimal minimumDeposit)
    {
        CheckValidity(minimumDeposit);
        return new MinimumDeposit(minimumDeposit);
    }

    public static implicit operator decimal(MinimumDeposit self) => self.Value;
    public decimal Value { get; internal set; }
    public static MinimumDeposit NoMinimumDeposit => new();
    private static void CheckValidity(decimal value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(MinimumDeposit), "MinimumDeposit cannot be empty");
    }
}