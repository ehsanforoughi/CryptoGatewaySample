using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;

namespace CryptoGateway.Domain.Entities.PayIn;

public class PayIn : AggregateRoot<PayInId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors
    protected PayIn() { }
    #endregion

    #region Events
    public static PayIn Create(PayInExternalId payInExternalId, int userId, CustomerId customerId, Money value, TxId txId)
    {
        var payIn = new PayIn();
        payIn.Apply(new PayInEvents.V1.PayInCreated
        {
            PayInExternalId = payInExternalId,
            UserId = userId,
            CustomerId = customerId,
            TxId = txId,
            //CustomerContact = customerContact,
            Value = value.Amount,
            CurrencyType = value.CurrencyType.ToString()
        });

        return payIn;
    }

    public void AssignCustodyAccount(CustodyAccount.CustodyAccount custodyAccount)
    {
        CustodyAccount = custodyAccount;
    }

    public void SetCommission(Money value, decimal commissionPercentage, decimal commissionFixedValue)
    {
        Commission = Commission.FromDecimal(commissionPercentage, commissionFixedValue, value.CurrencyType);
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case PayInEvents.V1.PayInCreated e:
                PayInExternalId = new PayInExternalId(e.PayInExternalId);
                UserId = e.UserId;
                CustomerId = new CustomerId(e.CustomerId);
                TxId = TxId.FromString(e.TxId);
                Value = Money.FromDecimal(e.Value, e.CurrencyType.Str2Enum<CurrencyType>());
                break;
        }
    }

    protected override void EnsureValidState()
    {
    }
    #endregion

    #region Properties
    public PayInId PayInId { get; private set; }
    public PayInExternalId PayInExternalId { get; private set; }
    public int UserId { get; private set; }
    public User.User User { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Money Value { get; private set; }
    public CustodyAccount.CustodyAccount CustodyAccount { get; private set; }
    public TxId TxId { get; private set; }
    public Commission Commission { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    #endregion
}