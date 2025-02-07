using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.Creation;

public class NotificationCreationFactory
{
    public static INotificationCreationStrategy GetStrategy(NotificationType type)
    {
        return type switch
        {
            NotificationType.Sms => new SmsCreationStrategy(),
            NotificationType.Email => new EmailCreationStrategy(),
            //NotificationType.Push => new SavePushStrategy(),
            //NotificationType.TelegramBot => new SaveTelegramStrategy(),
            _ => new NoActionStrategy(),
        };
    }
}