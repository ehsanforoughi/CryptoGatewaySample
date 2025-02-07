using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.Creation;

public interface INotificationCreationStrategy
{
    Domain.Entities.Notification.Notification Creation(int userId, string receiver, 
        NotificationActionType actionType, string token1, string token2 = null, string token3 = null);
}