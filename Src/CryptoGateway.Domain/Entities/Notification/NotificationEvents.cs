namespace CryptoGateway.Domain.Entities.Notification;

public static class NotificationEvents
{
    public static class V1
    {
        public class NotificationCreated
        {
            public int UserId { get; set; }
            public byte Type { get; set; }
            public byte ActionType { get; set; }
            public byte PriorityType { get; set; }
            public string Receiver { get; set; }
            public string Text { get; set; }
        }
    }
}