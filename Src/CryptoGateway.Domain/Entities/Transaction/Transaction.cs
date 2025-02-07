using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.Domain.Entities.Transaction;

public class Transaction : ChildEntity<TransactionId>,
    IEntity, ISoftDeleteProperty, IInsertDateProperty, IModifyDateProperty
{
    #region Constructors

    private readonly string _currencyType;
    protected Transaction()
    {
    }
    public Transaction(Action<object> applier, string currencyType) : base(applier)
    {
        _currencyType = currencyType;
    }
    #endregion

    #region Events

    protected override void When(object @event)
    {
        switch (@event)
        {
            case UserCreditEvents.V1.CreditTransactionAdded e:
                Type = e.TransactionType.Str2Enum<TransType>();
                ActionType = e.TransactionActionType.Str2Enum<TransActionType>();
                Value = Money.FromString(e.Amount, _currencyType);
                Balance = Money.FromString(e.Balance, _currencyType);
                ReferenceName = ReferenceName.FromString(e.TransactionActionType);
                ReferenceNumber = ReferenceNumber.FromInteger(0);
                break;
            case UserCreditEvents.V1.DebitTransactionAdded e:
                Type = e.TransactionType.Str2Enum<TransType>();
                ActionType = e.TransactionActionType.Str2Enum<TransActionType>();
                Value = Money.FromString(e.Amount, _currencyType);
                Balance = Money.FromString(e.Balance, _currencyType);
                ReferenceName = ReferenceName.FromString(e.TransactionActionType);
                ReferenceNumber = ReferenceNumber.FromInteger(0);
                break;
            case UserCreditEvents.V1.CommissionTransactionAdded e:
                Type = TransType.Debit;
                ActionType = TransActionType.Commission;
                Value = Money.FromString(e.FinalCommissionValue, _currencyType);
                Balance = Money.FromString(e.Balance, _currencyType);
                ReferenceName = ReferenceName.FromString(TransActionType.Commission.ToString());
                ReferenceNumber = ReferenceNumber.FromInteger(0);
                break;
        }
    }
    #endregion

    #region Properties

    public TransactionId TransactionId { get; private set; }
    public Money Value { get; private set; }
    public Money Balance { get; private set; }
    public ReferenceName ReferenceName { get; private set; }
    public ReferenceNumber ReferenceNumber { get; private set; }
    public TransType Type { get; private set; }
    public TransActionType ActionType { get; private set; }
    public TransactionNote Note { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    #endregion
}