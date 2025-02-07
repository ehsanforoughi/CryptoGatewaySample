using Bat.Core;
using CryptoGateway.Domain.Entities.Currency.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency;

public class Currency : AggregateRoot<CurrencyId>,
    IEntity, ISoftDeleteProperty, IInsertDateProperty, IModifyDateProperty
{
    protected Currency() { }
    public Currency(CurrencyType currencyType, DecimalPlaces decimalPlaces,
        IsActive isActive, IsFiat isFiat)
    {
        Code = CurrencyCode.FromString(currencyType.ToString().ToUpper());
        Type = currencyType;
        DecimalPlaces = decimalPlaces;
        IsActive = isActive;
        IsFiat = isFiat;

        FullName = CurrencyFullName.NoFullName;
        NameFa = CurrencyNameFa.NoNameFa;
        Priority = Priority.NoPriority;
        Network = Network.NoNetwork;
        IsTradable = IsTradable.NotTradable;
        IsDepositable = IsDepositable.NotDepositable;
        IsWithdrawable = IsWithdrawable.NotWithdrawable;
        NetworkTransferFee = NetworkTransferFee.NoNetworkTransferFee;
        MinimumAmount = MinimumAmount.NoMinimumAmount;
        MinimumDeposit = MinimumDeposit.NoMinimumDeposit;
        MinimumWithdraw = MinimumWithdraw.NoMinimumWithdraw;
    }

    public static readonly Currency None = new Currency();

    protected override void When(object @event)
    {
        throw new NotImplementedException();
    }

    protected override void EnsureValidState()
    {
        throw new NotImplementedException();
    }

    public CurrencyId CurrencyId { get; private set; }
    public CurrencyCode Code { get; private set; }
    public CurrencyType Type { get; private set; }
    public CurrencyFullName FullName { get; private set; }
    public CurrencyNameFa NameFa { get; private set; }
    public IsFiat IsFiat { get; private set; }
    public Priority Priority { get; private set; }
    public IsActive IsActive { get; private set; }
    public LogoUrl LogoUrl { get; private set; }
    public Network Network { get; private set; }
    public DecimalPlaces DecimalPlaces { get; private set; }
    public IsTradable IsTradable { get; private set; }
    public IsDepositable IsDepositable { get; private set; }
    public IsWithdrawable IsWithdrawable { get; private set; }
    public NetworkTransferFee NetworkTransferFee { get; private set; }
    public MinimumAmount MinimumAmount { get; private set; }
    public MinimumDeposit MinimumDeposit { get; private set; }
    public MinimumWithdraw MinimumWithdraw { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
}