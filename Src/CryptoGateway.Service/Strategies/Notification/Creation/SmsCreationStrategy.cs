using Bat.Core;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.Creation;

public class SmsCreationStrategy : INotificationCreationStrategy
{
    public Domain.Entities.Notification.Notification Creation(int userId, string receiver, 
        NotificationActionType actionType, string token1, string token2 = null, string token3 = null)
    {
        if (!long.TryParse(receiver, out _))
            throw new Exception(ServiceMessages.InvalidByName.Fill(receiver));

        var text = NotificationText.FromString(token1.Trim());

        var notification = Domain.Entities.Notification.Notification.Create(userId, NotificationType.Sms,
            actionType, PriorityType.Medium, Receiver.FromString(receiver[0] != '0' ? $"0{receiver}" : receiver), text);

        if (!string.IsNullOrWhiteSpace(token2)) notification.AddNextTokenValue(token2);
        if (!string.IsNullOrWhiteSpace(token2) && !string.IsNullOrWhiteSpace(token3))
            notification.AddNextTokenValue(token3);

        return notification;
    }
}