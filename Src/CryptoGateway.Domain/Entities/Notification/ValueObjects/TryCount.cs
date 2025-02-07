using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class TryCount : Value<TryCount>
{
    // Satisfy the serialization requirements
    protected TryCount() { }

    internal TryCount(byte tryCount) => Value = tryCount;

    public static TryCount FromByte(byte tryCount)
    {
        return new TryCount(tryCount);
    }

    public static implicit operator byte(TryCount self) => self.Value;
    public byte Value { get; internal set; }
    public static TryCount NoTryCount => new();
}