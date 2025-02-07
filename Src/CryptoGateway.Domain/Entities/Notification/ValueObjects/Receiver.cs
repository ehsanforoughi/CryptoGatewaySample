using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class Receiver : Value<Receiver>
{
    // Satisfy the serialization requirements
    protected Receiver() { }

    internal Receiver(string receiver) => Value = receiver;

    public static Receiver FromString(string receiver)
    {
        return new Receiver(receiver);
    }

    public static implicit operator string(Receiver self) => self.Value;
    public string Value { get; internal set; }
    public static Receiver NoReceiver => new();
}