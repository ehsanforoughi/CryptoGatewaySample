using System.ComponentModel;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public enum NotificationType : byte
{
    [Description("پیامک")]
    Sms = 1,

    [Description("ایمیل")]
    Email = 2,

    [Description("پوش")]
    Push = 3,
}