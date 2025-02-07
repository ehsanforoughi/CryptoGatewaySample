using Bat.Core;
using CryptoGateway.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Domain.Entities.BankAccount;

public class BankAccount : AggregateRoot<BankAccountId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors

    protected BankAccount()
    {
        BankAccountId = BankAccountId.NoBankAccountId;
    }
    #endregion

    #region Events
    public static BankAccount Create(int userId, BankAccountTitle bankAccountTitle,
        BankType bankType, CardNumber cardNumber, Sheba sheba, AccountNumber accountNumber)
    {
        var bankAccount = new BankAccount();
        bankAccount.Apply(new BankAccountEvents.V1.BankAccCreated
        {
            UserId = userId,
            Title = bankAccountTitle,
            BankType = (byte)bankType,
            CardNumber = cardNumber,
            Sheba = sheba,
            AccountNumber = accountNumber,
        });

        return bankAccount;
    }

    public void Edit(BankAccountTitle bankAccountTitle,
        BankType bankType, CardNumber cardNumber, Sheba sheba, AccountNumber accountNumber)
    {
        Apply(new BankAccountEvents.V1.BankAccEdited
        {
            Title = bankAccountTitle,
            BankType = (byte)bankType,
            CardNumber = cardNumber,
            Sheba = sheba,
            AccountNumber = accountNumber,
        });
    }

    public void Remove()
    {
        Apply(new BankAccountEvents.V1.BankAccRemoved
        {
            BankAccountId = BankAccountId,
        });
    }

    public void Approve(int approvedBy, BankAccountDesc desc)
    {
        Apply(new BankAccountEvents.V1.BankAccountApproved
        {
            BankAccountId = BankAccountId,
            ApprovedBy = approvedBy,
            Desc = desc
        });
    }

    public void Reject(int approvedBy, BankAccountDesc desc)
    {
        Apply(new BankAccountEvents.V1.BankAccountRejected
        {
            BankAccountId = BankAccountId,
            ApprovedBy = approvedBy,
            Desc = desc
        });
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case BankAccountEvents.V1.BankAccCreated e:
                //BankAccountId = new BankAccountId(e.BankAccountId);
                //Id = new BankAccountId(e.BankAccountId);
                UserId = e.UserId;
                BankAccountTitle = BankAccountTitle.FromString(e.Title);
                BankType = (BankType)e.BankType;
                CardNumber = CardNumber.FromString(e.CardNumber);
                Sheba = Sheba.FromString(e.Sheba);
                AccountNumber = AccountNumber.FromString(e.AccountNumber);
                //ApprovedBy = UserId.NoUser;

                State = ApprovingState.Created;
                break;
            case BankAccountEvents.V1.BankAccEdited e:
                BankAccountTitle = BankAccountTitle.FromString(e.Title);
                BankType = (BankType)e.BankType;
                CardNumber = CardNumber.FromString(e.CardNumber);
                Sheba = Sheba.FromString(e.Sheba);
                AccountNumber = AccountNumber.FromString(e.AccountNumber);
                break;
            case BankAccountEvents.V1.BankAccRemoved e:
                IsDeleted = true;
                break;
            case BankAccountEvents.V1.BankAccountApproved e:
                ApprovedBy = e.ApprovedBy;
                Desc = BankAccountDesc.FromString(e.Desc);

                State = ApprovingState.Approved;
                break;
            case BankAccountEvents.V1.BankAccountRejected e:
                ApprovedBy = e.ApprovedBy;
                Desc = BankAccountDesc.FromString(e.Desc);

                State = ApprovingState.Rejected;
                break;
        }
    }

    protected override void EnsureValidState()
    {
        var valid =
            UserId != null &&
            State switch
            {
                ApprovingState.Created =>
                    BankAccountTitle != null
                    && CardNumber != null
                    && Sheba != null,
                ApprovingState.Approved =>
                    ApprovedBy != null
                    && Desc != null
                    && State == ApprovingState.Approved,
                ApprovingState.Rejected =>
                    ApprovedBy != null
                    && Desc != null
                    && State == ApprovingState.Rejected,
                _ => true
            };

        if (!valid)
            throw new DomainExceptions.InvalidEntityState(this, $"Post-checks failed in state {State}");
    }
    #endregion

    #region Properties
    public BankAccountId BankAccountId { get; private set; }

    [ForeignKey(nameof(UserId))]
    public User.User User { get; private set; }
    public int UserId { get; private set; }

    public BankAccountTitle BankAccountTitle { get; private set; }
    [ForeignKey(nameof(ApprovedBy))]
    public User.User Approver { get; private set; }
    public int? ApprovedBy { get; private set; }
    public ApprovingState State { get; private set; }
    public BankType BankType { get; private set; }
    public CardNumber CardNumber { get; private set; }
    public Sheba Sheba { get; private set; }
    public AccountNumber AccountNumber { get; private set; }
    public BankAccountDesc Desc { get; private set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}