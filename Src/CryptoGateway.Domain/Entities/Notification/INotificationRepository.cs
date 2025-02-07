using CryptoGateway.Domain.Entities.Notification.ValueObjects;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Notification;

public interface INotificationRepository : IRepository<Notification, NotificationId>
{
}