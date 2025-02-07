using CryptoGateway.Framework;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;
using CryptoGateway.Service.Strategies.Notification.SmsGateway;

namespace CryptoGateway.Service.Strategies.Notification.Sender;

public class SmsSenderStrategy : INotificationSenderStrategy
{
    public void Send(Domain.Entities.Notification.Notification notification, 
        IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        KaveNegarAdapter adapter = new KaveNegarAdapter();
        var sendResult = adapter.SendAsync(notification.Receiver, notification.Text).Result;

        notification.SetSenderResult(IsSent.FromBoolean(sendResult.Result), IsSuccess.FromBoolean(sendResult.Result),
            SendStatus.FromString(sendResult.Message));

        unitOfWork.Commit();
    }
}