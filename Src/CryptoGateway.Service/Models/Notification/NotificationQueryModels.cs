namespace CryptoGateway.Service.Models.Notification;

public static class NotificationQueryModels
{
    public class GetNotifications
    {
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetNotSentNotifications
    {
        public int TryCount { get; set; }
    }
}