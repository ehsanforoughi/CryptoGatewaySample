using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;

namespace CryptoGateway.Domain.Entities.CustodyAccount;

public class CustodyAccount : AggregateRoot<CustodyAccountId>,
    IEntity, ISoftDeleteProperty, IInsertDateProperty, IModifyDateProperty
{
    #region Constructors
    protected CustodyAccount() { }
    #endregion

    #region Events
    public static CustodyAccount Create(CustodyAccExternalId custodyAccExternalId,
        CurrencyType currencyType, CustodyAccountTitle title, int userId, CustomerId customerId)
    {
        var custodyAccount = new CustodyAccount();
        custodyAccount.Apply(new CustodyAccountEvents.V1.CustodyAccountCreated
        {
            CustodyAccExternalId = custodyAccExternalId,
            CurrencyType = currencyType.ToString(),
            Title = title,
            UserId = userId,
            CustomerId = customerId,
        });

        return custodyAccount;
    }

    public void AssignContractAccount(ContractAccount.ContractAccount contractAccount)
    {
        ContractAccount = contractAccount;
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case CustodyAccountEvents.V1.CustodyAccountCreated e:
                CustodyAccExternalId = new CustodyAccExternalId(e.CustodyAccExternalId);
                CurrencyType = e.CurrencyType.Str2Enum<CurrencyType>();
                UserId = e.UserId;
                CustomerId = new CustomerId(e.CustomerId);
                Title = new CustodyAccountTitle(e.Title);
                break;
        }
    }

    protected override void EnsureValidState() { }
    #endregion

    #region Properties
    public CustodyAccountId CustodyAccountId { get; private set; }
    public CustodyAccExternalId CustodyAccExternalId { get; private set; }
    public CurrencyType CurrencyType { get; private set; }
    public CustodyAccountTitle Title { get; private set; }
    public int UserId { get; private set; }
    public User.User User { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public ContractAccount.ContractAccount ContractAccount { get; private set; }
    public IsActive IsActive { get; private set; } = new IsActive(false);
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    #endregion
}