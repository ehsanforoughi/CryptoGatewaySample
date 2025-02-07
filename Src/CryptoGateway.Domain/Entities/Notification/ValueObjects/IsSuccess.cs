using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class IsSuccess : Value<IsSuccess>
{
    // Satisfy the serialization requirements
    protected IsSuccess() { }

    internal IsSuccess(bool isSuccess) => Value = isSuccess;

    public static IsSuccess FromBoolean(bool isSuccess)
    {
        return new IsSuccess(isSuccess);
    }

    public static implicit operator bool(IsSuccess self) => self.Value;
    public bool Value { get; internal set; }
    public static IsSuccess NotSuccess => new(false);
}