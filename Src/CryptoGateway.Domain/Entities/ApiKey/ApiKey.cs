using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.Domain.Entities.ApiKey;

public class ApiKey : AggregateRoot<ApiKeyId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors
    protected ApiKey() {}
    #endregion

    #region Events

    public static ApiKey Generate(int userId, KeyValue keyValue)
    {
        var apiKey = new ApiKey();
        apiKey.Apply(new ApiKeyEvents.V1.ApiKeyGenerated
        {
            UserId = userId,
            KeyValue = keyValue
        });

        return apiKey;
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case ApiKeyEvents.V1.ApiKeyGenerated e:
                UserId = e.UserId;
                KeyValue = KeyValue.FromString(e.KeyValue);
                break;
        }
    }

    protected override void EnsureValidState()
    {
    }
    #endregion

    #region Properties

    public ApiKeyId ApiKeyId { get; private set; }

    public User.User User { get; private set; }
    public int UserId { get; private set; }

    public KeyValue KeyValue { get; private set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    public bool IsDeleted { get; set; } 
    #endregion
}