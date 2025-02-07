using CryptoGateway.Framework;
using Microsoft.Extensions.Configuration;

namespace CryptoGateway.Service.Strategies.Notification.Sender;

public interface INotificationSenderStrategy
{
    void Send(Domain.Entities.Notification.Notification notification,
        IUnitOfWork unitOfWork, IConfiguration configuration);
}