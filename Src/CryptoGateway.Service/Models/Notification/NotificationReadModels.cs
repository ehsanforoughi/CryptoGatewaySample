using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Models.Notification;

public static class NotificationReadModels
{
    public class NotificationListItem
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte NotifType { get; set; }
        public NotificationType NotificationType => NotifType.Byte2Enum<NotificationType>();
        public byte AType { get; set; }
        public NotificationActionType ActionType => AType.Byte2Enum<NotificationActionType>();
        public byte PType { get; set; }
        public PriorityType PriorityType => PType.Byte2Enum<PriorityType>();
        public byte TryCount { get; set; }
        public bool IsSent { get; set; }
        public bool IsSuccess { get; set; }
        public string SentAt { get; set; }
        public string SendStatus { get; set; }
        public string Receiver { get; set; }
        public string Text { get; set; }
        public string CreatedAt { get; set; }
        public string ExpiredAt { get; set; }
    }
}