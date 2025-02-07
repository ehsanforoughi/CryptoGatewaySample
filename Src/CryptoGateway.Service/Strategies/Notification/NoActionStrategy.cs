using CryptoGateway.Framework;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Service.Strategies.Notification.Sender;
using CryptoGateway.Service.Strategies.Notification.Creation;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification
{
    public class NoActionStrategy : INotificationCreationStrategy, INotificationSenderStrategy
    {
        public Domain.Entities.Notification.Notification Creation(int userId, string receiver, NotificationActionType actionType,
            string token1, string token2 = null, string token3 = null)
        {
            throw new NotImplementedException();
        }

        public void Send(Domain.Entities.Notification.Notification notification, 
            IUnitOfWork unitOfWork, IConfiguration configuration)
        {
        }
    }
}