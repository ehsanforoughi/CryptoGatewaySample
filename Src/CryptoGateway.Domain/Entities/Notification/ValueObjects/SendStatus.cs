using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class SendStatus : Value<SendStatus>
{
    // Satisfy the serialization requirements
    protected SendStatus() { }

    internal SendStatus(string sendStatus) => Value = sendStatus;

    public static SendStatus FromString(string sendStatus)
    {
        return new SendStatus(sendStatus);
    }

    public static implicit operator string(SendStatus self) => self.Value;
    public string Value { get; internal set; }
    public static SendStatus NoSendStatus => new();
}