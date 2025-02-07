using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;
using CryptoGateway.Domain.Entities.Payout.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Domain.Entities.Payout;

public class Payout : AggregateRoot<PayoutId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors
    protected Payout() { }
    #endregion

    #region Events
    public static Payout CreateFiatPayout(int userId, Money value, BankAccountId bankAccountId)
    {
        var payout = new Payout();
        payout.Apply(new PayoutEvents.V1.FiatRequestCreated
        {
            UserId = userId,
            Amount = value.Amount,
            CurrencyCode = value.CurrencyType.ToString(),
            BankAccountId = bankAccountId
        });

        return payout;
    }

    public static Payout CreateCryptoPayout(int userId, Money value, WalletId walletId)
    {
        var payout = new Payout();
        payout.Apply(new PayoutEvents.V1.CryptoRequestCreated
        {
            UserId = userId,
            Amount = value.Amount,
            CurrencyCode = value.CurrencyType.ToString(),
            WalletId = walletId
        });

        return payout;
    }

    public void Reject(PayoutId payoutId, int approvedBy, PayoutDesc desc)
    {
        Apply(new PayoutEvents.V1.PayoutRejected
        {
            PayoutId = payoutId,
            ApprovedBy = approvedBy,
            Desc = desc
        });
    }

    public void ApproveManualFiatWithdrawalRequest(User.User user,
        int approvedBy, PayoutDesc desc,
        BankTrackingCode bankTrackingCode)
    {
        Apply(new PayoutEvents.V1.FiatManualWithdrawalRequestApproved
        {
            ApprovedBy = approvedBy,
            Desc = desc,
            BankTrackingCode = bankTrackingCode
        });
    }

    public void ApproveManualCryptoWithdrawalRequest(User.User user,
        int approvedBy, PayoutDesc desc, TransactionUrl transactionUrl)
    {
        Apply(new PayoutEvents.V1.CryptoManualWithdrawalRequestApproved
        {

            ApprovedBy = approvedBy,
            Desc = desc,
            TransactionUrl = transactionUrl
        });
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case PayoutEvents.V1.FiatRequestCreated e:
                UserId = e.UserId;
                Value = Money.FromDecimal(e.Amount, e.CurrencyCode.Str2Enum<CurrencyType>());
                BankAccountId = new BankAccountId(e.BankAccountId);
                //ApprovedBy = UserId.NoUser;

                State = ApprovingState.Created;
                TransferType = TransferType.FiatWithdrawal;
                break;
            case PayoutEvents.V1.CryptoRequestCreated e:
                UserId = e.UserId;
                Value = Money.FromDecimal(e.Amount, e.CurrencyCode.Str2Enum<CurrencyType>());
                WalletId = new WalletId(e.WalletId);

                State = ApprovingState.Created;
                TransferType = TransferType.CryptoWithdrawal;
                break;
            case PayoutEvents.V1.PayoutRejected e:
                ApprovedBy = e.ApprovedBy;
                Desc = PayoutDesc.FromString(e.Desc);

                State = ApprovingState.Rejected;
                break;
            case PayoutEvents.V1.FiatManualWithdrawalRequestApproved e:
                ApprovedBy = e.ApprovedBy;
                Desc = PayoutDesc.FromString(e.Desc);
                BankTrackingCode = BankTrackingCode.FromString(e.BankTrackingCode);

                State = ApprovingState.Approved;
                break;
            case PayoutEvents.V1.CryptoManualWithdrawalRequestApproved e:
                ApprovedBy = e.ApprovedBy;
                Desc = PayoutDesc.FromString(e.Desc);
                TransactionUrl = TransactionUrl.FromString(e.TransactionUrl);

                State = ApprovingState.Approved;
                break;
        }
    }

    protected override void EnsureValidState() {}
    #endregion

    #region Properties
    public PayoutId PayoutId { get; private set; }
    [ForeignKey(nameof(UserId))]
    public User.User User { get; private set; }
    public int UserId { get; private set; }
    [ForeignKey(nameof(ApprovedBy))]
    public User.User Approver { get; private set; }
    public int? ApprovedBy { get; private set; }
    public Money Value { get; private set; }
    public BankAccountId BankAccountId { get; private set; }
    public BankAccount.BankAccount? BankAccount { get; private set; }
    public WalletId WalletId { get; private set; }
    public Wallet.Wallet? Wallet { get; private set; }
    public ApprovingState State { get; private set; }
    public TransferType TransferType { get; private set; }
    public BankTrackingCode BankTrackingCode { get; private set; }
    public TransactionUrl TransactionUrl { get; private set; }
    public PayoutDesc Desc { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }



    #endregion
}