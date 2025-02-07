using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class IsWithdrawable : Value<IsWithdrawable>
{
    // Satisfy the serialization requirements
    protected IsWithdrawable() { }

    internal IsWithdrawable(bool isWithdrawable) => Value = isWithdrawable;

    public static IsWithdrawable FromBoolean(bool isWithdrawable)
    {
        return new IsWithdrawable(isWithdrawable);
    }

    public static implicit operator bool(IsWithdrawable self) => self.Value;
    public bool Value { get; internal set; }
    public static IsWithdrawable NotWithdrawable => new(false);
}