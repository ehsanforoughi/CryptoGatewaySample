using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;
using CryptoGateway.Domain.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoGateway.Domain.Entities.Notification;

public class Notification : AggregateRoot<NotificationId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors
    protected Notification() {}
    #endregion

    #region Events

    public static Notification Create(int userId, NotificationType type, NotificationActionType actionType,
        PriorityType priorityType, Receiver receiver, NotificationText text)
    {
        var notification = new Notification();
        notification.Apply(new NotificationEvents.V1.NotificationCreated
        {
            UserId = userId,
            Type = (byte)type,
            ActionType = (byte)actionType,
            PriorityType = (byte)priorityType,
            Receiver = receiver,
            Text = text
        });

        return notification;
    }

    public void AddNextTokenValue(string nextToken) 
        => Text = NotificationText.FromString(Text + $"|{nextToken.Trim()}");

    public void SetSenderResult(IsSent isSent, IsSuccess isSuccess, SendStatus sendStatus)
    {
        IsSent = isSent;
        TryCount = TryCount.FromByte((byte)(TryCount + 1));
        SentAt = DateTime.Now;
        IsSuccess = isSuccess;
        SendStatus = sendStatus;
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case NotificationEvents.V1.NotificationCreated e:
                UserId = e.UserId;
                Type = (NotificationType)e.Type;
                ActionType = (NotificationActionType)e.ActionType;
                PriorityType = (PriorityType)e.PriorityType;
                Receiver = Receiver.FromString(e.Receiver);
                Text = NotificationText.FromString(e.Text);
                TryCount = TryCount.FromByte(0);
                IsSent = IsSent.FromBoolean(false);
                IsSuccess = IsSuccess.FromBoolean(false);
                break;
        }
    }

    protected override void EnsureValidState() {}
    #endregion

    #region Properties
    public NotificationId NotificationId { get; private set; }
    [ForeignKey(nameof(UserId))]
    public User.User User { get; private set; }
    public int UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationActionType ActionType { get; private set; }
    public PriorityType PriorityType { get; private set; } = PriorityType.Low;
    public TryCount TryCount { get; private set; }
    public IsSent IsSent { get; private set; }
    public IsSuccess? IsSuccess { get; private set; }
    public DateTime? SentAt { get; private set; }
    public SendStatus SendStatus { get; private set; }
    public Receiver Receiver { get; private set; }
    public NotificationText Text { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; } 
    #endregion
}