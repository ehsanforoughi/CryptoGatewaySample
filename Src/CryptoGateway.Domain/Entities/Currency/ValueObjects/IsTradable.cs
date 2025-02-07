using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class IsTradable : Value<IsTradable>
{
    // Satisfy the serialization requirements
    protected IsTradable() { }

    internal IsTradable(bool isTradable) => Value = isTradable;

    public static IsTradable FromBoolean(bool isTradable)
    {
        return new IsTradable(isTradable);
    }

    public static implicit operator bool(IsTradable self) => self.Value;
    public bool Value { get; internal set; }
    public static IsTradable NotTradable => new(false);
}