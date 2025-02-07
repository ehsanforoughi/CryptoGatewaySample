using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.Sender;

public class NotificationSenderFactory
{
    public static INotificationSenderStrategy GetStrategy(NotificationType type)
    {
        return type switch
        {
            NotificationType.Sms => new SmsSenderStrategy(),
            //NotificationType.Push => new SendPushStrategy(),
            NotificationType.Email => new EmailSenderStrategy(),
            //NotificationType.TelegramBot => new SendTelegramStrategy(),
            _ => new NoActionStrategy(),
        };
    }
}