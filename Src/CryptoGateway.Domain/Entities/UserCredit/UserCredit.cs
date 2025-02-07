using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;
using CryptoGateway.Domain.Entities.UserCredit.ValueObjects;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.Domain.Entities.UserCredit;

public class UserCredit : Entity<UserCreditId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    private IBlockedCredit _blockedCredit;
    #region Constructors
    protected UserCredit() { }

    public UserCredit(Action<object> applier) : base(applier)
    {
    }
    #endregion

    #region Events
    public void ChargeCredit(TransActionType actionType, decimal amount,
        decimal comPercentageAmount, decimal comFixedValueAmount, IBlockedCredit blockedCredit)
    {
        _blockedCredit = blockedCredit;
        Apply(new UserCreditEvents.V1.CreditTransactionAdded
        {
            TransactionType = TransType.Credit.ToString(),
            TransactionActionType = actionType.ToString(),
            Amount = amount,
            CurrencyType = GetCurrentCTypeStr(),
            Balance = RealCredit.Amount,
        });
        Apply(new UserCreditEvents.V1.CommissionTransactionAdded
        {
            TransactionType = TransType.Debit.ToString(),
            TransactionActionType = TransActionType.Commission.ToString(),
            Amount = amount,
            Balance = RealCredit.Amount,
            CurrencyType = GetCurrentCTypeStr(),
            ComPercentageAmount = comPercentageAmount,
            ComFixedValueAmount = comFixedValueAmount,
        });
    }
    public void TakeOnCredit(TransActionType actionType, decimal amount,
        decimal comPercentageAmount, decimal comFixedValueAmount, IBlockedCredit blockedCredit)
    {
        _blockedCredit = blockedCredit;
        Apply(new UserCreditEvents.V1.DebitTransactionAdded
        {
            TransactionType = TransType.Debit.ToString(),
            TransactionActionType = actionType.ToString(),
            Amount = amount,
            CurrencyType = GetCurrentCTypeStr(),
            Balance = RealCredit.Amount,
        });
        Apply(new UserCreditEvents.V1.CommissionTransactionAdded
        {
            TransactionType = TransType.Debit.ToString(),
            TransactionActionType = TransActionType.Commission.ToString(),
            Amount = amount,
            CurrencyType = GetCurrentCTypeStr(),
            Balance = RealCredit.Amount,
            ComPercentageAmount = comPercentageAmount,
            ComFixedValueAmount = comFixedValueAmount,
        });
    }

    protected override void When(object @event)
    {
        Transaction.Transaction transaction;
        switch (@event)
        {
            case UserEvents.V1.UserCreditAllocated e:
                Id = new UserCreditId(e.UserCreditId);
                RealCredit = Money.FromString(Money.ZeroValue, e.CurrencyCode);
                InsertDateMi = DateTime.Now;
                ModifyDateMi = DateTime.Now;
                IsDeleted = false;
                break;
            case UserCreditEvents.V1.CreditTransactionAdded e:
                Id = UserCreditId;
                RealCredit += Money.FromString(e.Amount, GetCurrentCTypeStr());
                transaction = new Transaction.Transaction(Apply, GetCurrentCTypeStr());
                ApplyToChildEntity(transaction, e);
                Transactions ??= new List<Transaction.Transaction>();
                Transactions.Add(transaction);
                ModifyDateMi = DateTime.Now;
                break;
            case UserCreditEvents.V1.DebitTransactionAdded e:
                Id = UserCreditId;
                if (!EnsureEnoughCredit(e.Amount))
                    throw new SystemException("Not enough credit");
                RealCredit -= Money.FromString(e.Amount, GetCurrentCTypeStr());
                transaction = new Transaction.Transaction(Apply, GetCurrentCTypeStr());
                ApplyToChildEntity(transaction, e);
                Transactions ??= new List<Transaction.Transaction>();
                Transactions.Add(transaction);
                ModifyDateMi = DateTime.Now;
                break;
            case UserCreditEvents.V1.CommissionTransactionAdded e:
                Id = UserCreditId;
                if (e is { ComPercentageAmount: 0, ComFixedValueAmount: 0 })
                    break;
                if (e.ComFixedValueAmount >= e.Amount)
                    break;
                var commissionValue = CalculateCommission(e.Amount, e.ComPercentageAmount, e.ComFixedValueAmount);
                e.FinalCommissionValue = commissionValue.Amount;
                if (!EnsureEnoughCredit(e.Amount))
                    throw new SystemException("Not enough credit");
                RealCredit -= commissionValue;
                transaction = new Transaction.Transaction(Apply, GetCurrentCTypeStr());
                ApplyToChildEntity(transaction, e);
                Transactions ??= new List<Transaction.Transaction>();
                Transactions.Add(transaction);
                ModifyDateMi = DateTime.Now;
                break;
        }
    }
    #endregion

    #region PrivateMethods

    private Money CalculateCommission(decimal amount, decimal comPercentageAmount, decimal comFixedValueAmount)
    {
        var ratio = Money.FromDecimal(0.01M, GetCurrentCType());
        var value = Money.FromString(amount, GetCurrentCTypeStr()).TruncateDecimal(6);
        var commission = Commission.FromDecimal(comPercentageAmount, comFixedValueAmount, GetCurrentCTypeStr());
        var percentageRatio = commission.Percentage * ratio;
        return (value * percentageRatio) + commission.FixedValue;
    }
    private bool EnsureEnoughCredit(decimal amount)
    {
        var value = Money.FromDecimal(amount, GetCurrentCType());
        return RealCredit - _blockedCredit.Calculate(UserId, RealCredit.CurrencyType) >= value;
    }

    private CurrencyType GetCurrentCType() => RealCredit.CurrencyType;
    private string GetCurrentCTypeStr() => RealCredit.CurrencyType.ToString();
    #endregion

    #region Public Methods
    public async Task<bool> EnsureEnoughCredit(Money amount, IBlockedCredit blockedCredit) =>
        (RealCredit - await blockedCredit.CalculateAsync(UserId, RealCredit.CurrencyType)) > amount;
    #endregion

    #region Properties
    public UserCreditId UserCreditId { get; private set; }
    public int UserId { get; private set; }
    public Money RealCredit { get; private set; }
    public List<Transaction.Transaction> Transactions { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}