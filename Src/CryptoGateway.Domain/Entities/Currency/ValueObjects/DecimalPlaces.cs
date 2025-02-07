using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class DecimalPlaces : Value<DecimalPlaces>
{
    // Satisfy the serialization requirements
    protected DecimalPlaces() { }

    internal DecimalPlaces(byte decimalPlaces) => Value = decimalPlaces;

    public static DecimalPlaces FromByte(byte decimalPlaces)
    {
        CheckValidity(decimalPlaces);
        return new DecimalPlaces(decimalPlaces);
    }

    public static implicit operator byte(DecimalPlaces self) => self.Value;
    public byte Value { get; internal set; }
    public static DecimalPlaces NoDecimalPlaces => new();
    private static void CheckValidity(byte value)
    {
        if (value < 0)
            throw new ArgumentNullException(nameof(DecimalPlaces), "DecimalPlaces cannot be empty");
    }
}