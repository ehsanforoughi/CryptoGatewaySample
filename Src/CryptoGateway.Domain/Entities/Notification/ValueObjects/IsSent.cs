using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class IsSent : Value<IsSent>
{
    // Satisfy the serialization requirements
    protected IsSent() { }

    internal IsSent(bool isSent) => Value = isSent;

    public static IsSent FromBoolean(bool isSent)
    {
        return new IsSent(isSent);
    }

    public static implicit operator bool(IsSent self) => self.Value;
    public bool Value { get; internal set; }
    public static IsSent NotSent => new(false);
}