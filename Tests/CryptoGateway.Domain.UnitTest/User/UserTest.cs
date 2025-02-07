using KsuidDotNet;
using CryptoGateway.Domain.UnitTest.Shared;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.User;

public class UserTest : IDisposable
{
    private readonly ISpotPriceProvider _spotUsdtPrice = new FakeSpotPrice();
    private readonly IBlockedCredit _blockedCredit = new FakeBlockedCredit();
    private readonly Password _password;

    public UserTest()
    {
        _password = Password.FromString("12345678");
        
    }

    private Entities.User.User CreateSampleUser()
    {
        return new Entities.User.User(
            userExternalId: new UserExternalId(Ksuid.NewKsuid()),
            email: Email.FromString("a@a.com"));
    }

    [Fact]
    public void Create_user_should_be_done_by_email_and_password()
    {
        // Arrange
        var userExternalId = new UserExternalId(Ksuid.NewKsuid());
        var email = Email.FromString("a@a.com");

        // Act
        var user = new Entities.User.User(userExternalId, email);

        // Assert
        Assert.Equal(userExternalId, user.UserExternalId);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void EditProfile_should_be_update_all_properties()
    {
        // Arrange
        var user = CreateSampleUser();
        var firstName = FirstName.FromString("John");
        var lastName = LastName.FromString("Smith");
        var nationalCode = NationalCode.FromLong("0793528739");
        var birthDate = BirthDate.FromDateTime(new DateTime(2000, 1, 1));

        // Act
        user.EditProfile(firstName, lastName, nationalCode, birthDate);

        // Assert
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(nationalCode, user.NationalCode);
        Assert.Equal(birthDate, user.BirthDate);
    }


    //[Fact]
    //public void UpdatePassword_should_be_change_the_old_password()
    //{
    //    // Arrange
    //    var user = new Domain.Entities.User.User(_userExternalId, _email, _password);
    //    var newPassword = "87654321";

    //    // Act
    //    user.EditPassword(Password.FromString(newPassword));

    //    // Assert
    //    Assert.NotEqual(_password, user.Password);
    //    Assert.Equal(newPassword, user.Password);
    //}

    [Fact]
    public void SetMobileNumber_should_be_set_mobileNumber_value()
    {
        // Arrange
        var user = CreateSampleUser();
        long? mobileNumber = 9363066996;

        // Act
        user.SetMobileNumber(MobileNumber.FromLong(mobileNumber));

        // Assert
        Assert.Equal(mobileNumber, user.MobileNumber);
    }

    [Fact]
    public void Create_user_should_allocate_two_default_user_credits()
    {
        // Arrange
        var userExternalId = new UserExternalId(Ksuid.NewKsuid());
        var email = Email.FromString("a@a.com");

        // Act
        var user = new Domain.Entities.User.User(userExternalId, email);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userExternalId, user.UserExternalId);
        Assert.Equal(email, user.Email);
        Assert.Equal(2, user.UserCredits.Count);
    }

    [Fact]
    public void Charge_credit_should_increase_user_credit_and_decrease_commission()
    {
        // Arrange
        var user = CreateSampleUser();
        const int creditAmount = 50;
        const int commissionAmount = 3;
        const int expectedBalance = creditAmount - commissionAmount;

        // Act
        user.GetUserCredit(CurrencyType.USDT).ChargeCredit(TransActionType.PayIn, creditAmount, 0, commissionAmount, _blockedCredit);

        // Assert
        Assert.Equal(expectedBalance, user.GetUserCredit(CurrencyType.USDT).RealCredit.Amount);
    }

    [Fact]
    public void Charge_credit_should_create_a_credit_transaction_and_a_commission_transaction()
    {
        // Arrange
        var user = CreateSampleUser();
        const int creditAmount = 50;
        const int commissionAmount = 3;

        // Act
        user.GetUserCredit(CurrencyType.USDT).ChargeCredit(TransActionType.PayIn, 50, 0, 3, _blockedCredit);

        // Assert
        Assert.Equal(2, user.GetUserCredit(CurrencyType.USDT).Transactions.Count);
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x.Type == TransType.Credit));
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x is { Type: TransType.Debit, ActionType: TransActionType.Commission }));
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x.Value.Amount == creditAmount));
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x.Value.Amount == commissionAmount));
    }

    [Fact]
    public void Take_on_credit_should_decrease_user_credit_and_commission()
    {
        // Arrange
        var user = CreateSampleUser();
        const int creditAmount = 50;
        const int debitAmount = 10;
        const int commissionAmount = 3;
        const int expectedBalance = (creditAmount - commissionAmount) + (-debitAmount - commissionAmount);

        // Act
        user.GetUserCredit(CurrencyType.USDT).ChargeCredit(TransActionType.PayIn, creditAmount, 0, commissionAmount, _blockedCredit);
        user.GetUserCredit(CurrencyType.USDT).TakeOnCredit(TransActionType.CryptoPayout, debitAmount, 0, commissionAmount, _blockedCredit);

        // Assert
        Assert.Equal(expectedBalance, user.GetUserCredit(CurrencyType.USDT).RealCredit.Amount);
    }

    [Fact]
    public void Take_on_credit_should_create_a_debit_transaction_and_a_commission_transaction()
    {
        // Arrange
        var user = CreateSampleUser();
        const int creditAmount = 50;
        const int debitAmount = 10;
        const int commissionAmount = 3;
        // Initial charge
        user.GetUserCredit(CurrencyType.USDT).ChargeCredit(TransActionType.PayIn, creditAmount, 0, commissionAmount, _blockedCredit);

        // Act
        user.GetUserCredit(CurrencyType.USDT).TakeOnCredit(TransActionType.CryptoPayout, debitAmount, 0, commissionAmount, _blockedCredit);

        // Assert
        Assert.Equal(4, user.GetUserCredit(CurrencyType.USDT).Transactions.Count);
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x is { Type: TransType.Credit, ActionType: TransActionType.PayIn }));
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x.Value.Amount == creditAmount));
        
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x is { Type: TransType.Debit, ActionType: TransActionType.CryptoPayout }));
        Assert.Equal(2, user.GetUserCredit(CurrencyType.USDT).Transactions.Count(x => x is { Type: TransType.Debit, ActionType: TransActionType.Commission }));
        Assert.Single(user.GetUserCredit(CurrencyType.USDT).Transactions.Where(x => x.Value.Amount == debitAmount));
        Assert.Equal(2, user.GetUserCredit(CurrencyType.USDT).Transactions.Count(x => x.Value.Amount == commissionAmount));
    }

    [Fact]
    public async Task Exchange_credit_should_change_a_currency_to_another_one()
    {
        // Arrange
        var user = CreateSampleUser();
        const int balance = 30; // and blockedCreditAmount = 10;
        const int amount = 10;
        var fromCurrency = Money.FromDecimal(amount, CurrencyType.USDT);
        var toCurrency = Money.FromNotClearAmount(CurrencyType.IRR);
        var spotPrice = await _spotUsdtPrice.FindSpotPrice(fromCurrency.CurrencyType, toCurrency.CurrencyType);
        // Initial charge
        user.GetUserCredit(CurrencyType.USDT).ChargeCredit(TransActionType.PayIn, balance, 0, 0, _blockedCredit);

        // Act
        await user.ExchangeCredit(fromCurrency, toCurrency, spotPrice, _blockedCredit);

        // Arrange
        Assert.Equal(balance - amount, user.GetUserCredit(CurrencyType.USDT).RealCredit.Amount);
        Assert.Equal(amount * spotPrice.Amount, user.GetUserCredit(CurrencyType.IRR).RealCredit.Amount);
        //Assert.Equal();
    }
        
   public void Dispose() => GC.SuppressFinalize(this);
}