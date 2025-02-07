using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class IsActive : Value<IsActive>
{
    // Satisfy the serialization requirements
    protected IsActive() { }

    internal IsActive(bool isActive) => Value = isActive;

    public static IsActive FromBoolean(bool isActive)
    {
        return new IsActive(isActive);
    }

    public static implicit operator bool(IsActive self) => self.Value;
    public bool Value { get; internal set; }
    public static IsActive NotActive => new(false);
}