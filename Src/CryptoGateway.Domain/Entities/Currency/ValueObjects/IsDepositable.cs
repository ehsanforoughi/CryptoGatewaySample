using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class IsDepositable : Value<IsDepositable>
{
    // Satisfy the serialization requirements
    protected IsDepositable() { }

    internal IsDepositable(bool isDepositable) => Value = isDepositable;

    public static IsDepositable FromBoolean(bool isDepositable)
    {
        return new IsDepositable(isDepositable);
    }

    public static implicit operator bool(IsDepositable self) => self.Value;
    public bool Value { get; internal set; }
    public static IsDepositable NotDepositable => new(false);
}