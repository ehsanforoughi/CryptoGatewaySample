using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public class NotificationText : Value<NotificationText>
{
    // Satisfy the serialization requirements
    protected NotificationText() { }

    internal NotificationText(string notificationText) => Value = notificationText;

    public static NotificationText FromString(string notificationText)
    {
        return new NotificationText(notificationText);
    }

    public static implicit operator string(NotificationText self) => self.Value;
    public string Value { get; internal set; }
    public static NotificationText NoNotificationText => new();
}