using Bat.Core;

namespace CryptoGateway.Service.Strategies.Notification.SmsGateway;

public interface ISmsGatewayAdapter
{
    Task<IResponse<bool>> SendAsync(string receiver, string text, bool isFlash = false);
    Task<IResponse<bool>> SendByTemplateAsync(Domain.Entities.Notification.Notification notification, bool isFlash = false);
}