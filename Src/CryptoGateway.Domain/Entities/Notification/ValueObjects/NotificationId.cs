using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class NotificationId : Value<NotificationId>
{
    public int Value { get; internal set; }

    protected NotificationId() { }

    public NotificationId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(NotificationId), "Bank account id cannot be empty");

        Value = value;
    }

    public static implicit operator int(NotificationId self) => self.Value;

    public static NotificationId NoNotificationId => new();
}