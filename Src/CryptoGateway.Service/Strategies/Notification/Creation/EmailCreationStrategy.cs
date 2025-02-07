using Bat.Core;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.Creation;

public class EmailCreationStrategy : INotificationCreationStrategy
{
    public Domain.Entities.Notification.Notification Creation(int userId, string receiver, 
        NotificationActionType actionType, string token1, string token2 = null, string token3 = null)
    {
        if (!receiver.IsEmail())
            throw new Exception(ServiceMessages.InvalidEntityIdByName.Fill("email", receiver));

        var text = NotificationText.FromString(token1.Trim());

        var notification = Domain.Entities.Notification.Notification.Create(userId, NotificationType.Email,
            actionType, PriorityType.Medium, Receiver.FromString(receiver), text);

        if (!string.IsNullOrWhiteSpace(token2)) notification.AddNextTokenValue(token2);
        if (!string.IsNullOrWhiteSpace(token2) && !string.IsNullOrWhiteSpace(token3))
            notification.AddNextTokenValue(token3);

        return notification;
    }
}