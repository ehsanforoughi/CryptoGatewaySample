using Microsoft.Extensions.DependencyInjection;
namespace CryptoGateway.Service.Strategies.Notification;
using CryptoGateway.Service.Strategies.Notification.SmsGateway;

public class NotificationGatewayFactory
{
    //public static ISmsGatewayAdapter GetGateway(IServiceProvider serviceProvider, NotificationProvider notificationProvider)
    //{
    //    return (notificationProvider switch
    //    {
    //        //NotificationProvider.Asanak => serviceProvider.GetService<AsanakAdapter>(),
    //        //NotificationProvider.LinePayamak => serviceProvider.GetService<LinePayamakAdapter>(),
    //        //NotificationProvider.NiazPardaz => serviceProvider.GetService<NiazPardazAdapter>(),
    //        NotificationProvider.Kavenegar => serviceProvider.GetService<KaveNegarAdapter>(),
    //        _ => throw new NotImplementedException()
    //    })!;
    //}
}