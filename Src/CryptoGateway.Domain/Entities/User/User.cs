using Bat.Core;
using CryptoGateway.Framework;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.Domain.Entities.User;

// Inheritance in this class (Aggregate Root) is different from other classes due to the requirement to use the Bat.Core library.
public sealed class User : IdentityUser<int>, IAggregateRoot<int>
    , IEntity, ISoftDeleteProperty, IInsertDateProperty, IModifyDateProperty
{
    private readonly List<object> _changes;
    #region Constructors

    public User()
    {

    }

    public User(UserExternalId userExternalId, string email)
    {
        _changes = new List<object>();
        UserCredits = new List<UserCredit.UserCredit>();
        UserExternalId = userExternalId;
        Email = email;
        UserName = email;

        // Only here we set these properties because the setting is done in Identity.
        IsDeleted = false;
        InsertDateMi = DateTime.Now;
        ModifyDateMi = DateTime.Now;

        AllocateUserCredit(CurrencyType.IRR);
        AllocateUserCredit(CurrencyType.USDT);
    }

    private void Apply(object @event)
    {
        When(@event);
        _changes.Add(@event);
    }
    private void ApplyToEntity(IInternalEventHandler entity, object @event)
        => entity?.Handle(@event);
    #endregion

    #region Events
    public void EditProfile(FirstName firstName, LastName lastName,
        NationalCode nationalCode, BirthDate birthDate)
    {
        Apply(new UserEvents.V1.UserProfileUpdated
        {
            FirstName = firstName,
            LastName = lastName,
            NationalCode = nationalCode,
            Birthdate = birthDate
        });
    }

    //public void EditPassword(Password newPassword)
    //{
    //    Apply(new UserEvents.V1.PasswordUpdated
    //    {
    //        //UserId = Id,
    //        //UserExternalId = userExternalId,
    //        Password = newPassword
    //    });
    //}

    public void SetMobileNumber(MobileNumber mobileNumber)
    {
        Apply(new UserEvents.V1.MobileNumberChanged
        {
            MobileNumber = mobileNumber
        });
    }

    public void AllocateUserCredit(CurrencyType currencyType)
    {
        Apply(new UserEvents.V1.UserCreditAllocated
        {
            //UserId = UserId,
            CurrencyCode = currencyType.ToString(),
            AvailableAmount = 0
        });
    }

    private void When(object @event)
    {
        switch (@event)
        {
            //case UserEvents.V1.UserRegistered e:
            //    //UserId = new UserId(e.UserId);
            //    //Id = new UserId(e.UserId);
            //    UserExternalId = new UserExternalId(e.UserExternalId);
            //    Email = new Email(e.Email);

            //    MobileNumber = MobileNumber.NoMobileNumber;
            //    FirstName = FirstName.NoFirstName;
            //    LastName = LastName.NoLastName;
            //    NationalCode = NationalCode.NoNationalCode;
            //    BirthDate = BirthDate.NoBirthDate;
            //    break;

            case UserEvents.V1.UserProfileUpdated e:
                FirstName = new FirstName(e.FirstName);
                LastName = new LastName(e.LastName);
                NationalCode = new NationalCode(e.NationalCode);
                BirthDate = new BirthDate(e.Birthdate);
                break;

            //case UserEvents.V1.PasswordUpdated e:
            //    Password = e.Password;
            //    break;

            case UserEvents.V1.MobileNumberChanged e:
                MobileNumber = new MobileNumber(e.MobileNumber);
                break;
            case UserEvents.V1.UserCreditAllocated e:
                if (UserCredits.Any(x =>
                        x.RealCredit.CurrencyType.ToString().Equals(
                            e.CurrencyCode, StringComparison.CurrentCultureIgnoreCase)))
                    break;
                var userCredit = new UserCredit.UserCredit(Apply);
                ApplyToEntity(userCredit, @event);
                UserCredits?.Add(userCredit);
                break;
        }
    }

    //protected override void EnsureValidState() { }
    #endregion

    #region Public Methods
    public IReadOnlyList<UserCredit.UserCredit> GetAllUserCredits()
    {
        return UserCredits;
    }

    public UserCredit.UserCredit GetUserCredit(CurrencyType currencyType)
    {
        return UserCredits.FirstOrDefault(x => x.RealCredit.CurrencyType.Equals(currencyType))!;
    }

    public async Task ExchangeCredit(Money fromCurrency, Money toCurrency, Money spotPrice, IBlockedCredit blockedCredit)
    {
        var fromUserCredit = GetUserCredit(fromCurrency.CurrencyType);
        if (!await fromUserCredit.EnsureEnoughCredit(fromCurrency, blockedCredit))
            throw new SystemException("There is no enough credit");

        fromUserCredit.TakeOnCredit(TransActionType.ExchangeCredit, fromCurrency.Amount, 0, 0, blockedCredit);
        var toUserCredit = GetUserCredit(toCurrency.CurrencyType);
        toUserCredit.ChargeCredit(TransActionType.ExchangeCredit,
            fromCurrency.Amount * spotPrice.Amount, 0, 0, blockedCredit);
    }
    #endregion

    #region Properties
    public UserExternalId UserExternalId { get; private set; }
    public MobileNumber MobileNumber { get; private set; }
    //public Email Email { get; private set; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public NationalCode NationalCode { get; private set; }
    public BirthDate BirthDate { get; private set; }
    public Salt Salt { get; private set; }
    //public Password Password { get; private set; }
    public DateTime InsertDateMi { get;  set; }
    public DateTime ModifyDateMi { get; set; }
    public bool IsDeleted { get; set; }
    public List<UserCredit.UserCredit> UserCredits { get; }
    #endregion
}