using Bat.Core;
using CryptoGateway.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;

namespace CryptoGateway.Domain.Entities.Wallet;

public class Wallet : AggregateRoot<WalletId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors

    protected Wallet()
    {
        WalletId = WalletId.NoWalletId;
    }
    #endregion

    #region Events
    public static Wallet Create(int userId, WalletTitle walletTitle, CurrencyId currencyId, Network network,
        WalletAddress address, MemoAddress memo, TagAddress tag)
    {
        var wallet = new Wallet();
        wallet.Apply(new WalletEvents.V1.WalletCreated
        {
            UserId = userId,
            Title = walletTitle,
            CurrencyId = currencyId,
            Network = network,
            Address = address,
            MemoAddress = memo,
            TagAddress = tag
        });

        return wallet;
    }

    public void Edit(WalletTitle walletTitle, CurrencyId currencyId, Network network,
        WalletAddress address, MemoAddress memo, TagAddress tag)
    {
        Apply(new WalletEvents.V1.WalletEdited()
        {
            Title = walletTitle,
            CurrencyId = currencyId,
            Network = network,
            Address = address,
            MemoAddress = memo,
            TagAddress = tag
        });
    }

    public void Remove()
    {
        Apply(new WalletEvents.V1.WalletRemoved()
        {
            WalletId = WalletId,
        });
    }
    public void Approve(int approvedBy, WalletDesc desc)
    {
        Apply(new WalletEvents.V1.WalletApproved
        {
            ApprovedBy = approvedBy,
            Desc = desc
        });
    }

    public void Reject(int approvedBy, WalletDesc desc)
    {
        Apply(new WalletEvents.V1.WalletRejected
        {
            ApprovedBy = approvedBy,
            Desc = desc
        });
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case WalletEvents.V1.WalletCreated e:
                UserId = e.UserId;
                WalletTitle = WalletTitle.FromString(e.Title);
                CurrencyId = new CurrencyId(e.CurrencyId);
                Network = Network.FromString(e.Network);
                Address = WalletAddress.FromString(e.Address);
                MemoAddress = MemoAddress.FromString(e.MemoAddress);
                TagAddress = TagAddress.FromString(e.TagAddress);

                State = ApprovingState.Created;
                break;
            case WalletEvents.V1.WalletEdited e:
                WalletTitle = WalletTitle.FromString(e.Title);
                CurrencyId = new CurrencyId(e.CurrencyId);
                Network = Network.FromString(e.Network);
                Address = WalletAddress.FromString(e.Address);
                MemoAddress = MemoAddress.FromString(e.MemoAddress);
                TagAddress = TagAddress.FromString(e.TagAddress);
                break;
            case WalletEvents.V1.WalletRemoved e:
                IsDeleted = true;
                break;
            case WalletEvents.V1.WalletApproved e:
                ApprovedBy = e.ApprovedBy;
                Desc = WalletDesc.FromString(e.Desc);

                State = ApprovingState.Approved;
                break;
            case WalletEvents.V1.WalletRejected e:
                ApprovedBy = e.ApprovedBy;
                Desc = WalletDesc.FromString(e.Desc);

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
                    WalletTitle != null
                    && CurrencyId != null
                    && Network != null,
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
    public WalletId WalletId { get; private set; }
    public WalletTitle WalletTitle { get; private set; }
    [ForeignKey(nameof(UserId))]
    public User.User User { get; private set; }
    public int UserId { get; private set; }
    [ForeignKey(nameof(ApprovedBy))]
    public User.User Approver { get; private set; }
    public int? ApprovedBy { get; private set; }
    public ApprovingState State { get; private set; }
    public CurrencyId CurrencyId { get; private set; }
    public Currency.Currency Currency { get; private set; }
    public Network Network { get; private set; }
    public WalletAddress Address { get; private set; }
    public MemoAddress MemoAddress { get; private set; }
    public TagAddress TagAddress { get; private set; }
    public WalletDesc Desc { get; private set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}