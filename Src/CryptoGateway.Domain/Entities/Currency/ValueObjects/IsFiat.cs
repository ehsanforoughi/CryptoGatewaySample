using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class IsFiat : Value<IsFiat>
{
    // Satisfy the serialization requirements
    protected IsFiat() { }

    internal IsFiat(bool isFiat) => Value = isFiat;

    public static IsFiat FromBoolean(bool isFiat)
    {
        return new IsFiat(isFiat);
    }

    public static implicit operator bool(IsFiat self) => self.Value;
    public bool Value { get; internal set; }
    public static IsFiat NoFiat => new(false);
}