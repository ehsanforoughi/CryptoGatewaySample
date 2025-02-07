using System.ComponentModel;

namespace CryptoGateway.Service.Strategies.Notification;

public enum NotificationProvider
{
    [Description("آسانک")]
    Asanak = 1,

    [Description("پیامک لاین")]
    LinePayamak = 2,

    [Description("نیاز پرداز")]
    NiazPardaz = 3,

    [Description("کاوه نگار")]
    Kavenegar = 4
}