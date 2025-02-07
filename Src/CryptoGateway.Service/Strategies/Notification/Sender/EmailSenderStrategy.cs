using Bat.Core;
using CryptoGateway.Framework;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Infrastructure.EmailGateway;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.Sender;

public class EmailSenderStrategy : INotificationSenderStrategy
{
    public void Send(Domain.Entities.Notification.Notification notification,
        IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        var sendResult = PakatEmailGatewayAdapter.Send(configuration, notification.ActionType.ToString(), 
            notification.Receiver, notification.ActionType.GetDescription(), notification.Text);

        notification.SetSenderResult(IsSent.FromBoolean(sendResult.Result), IsSuccess.FromBoolean(sendResult.Result), 
            SendStatus.FromString(sendResult.Message));

        unitOfWork.Commit();
    }
}