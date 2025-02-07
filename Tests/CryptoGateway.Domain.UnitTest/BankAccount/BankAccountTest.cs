using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.BankAccount;

public class BankAccountTest
{
    private Entities.BankAccount.BankAccount CreateSampleBankAccount()
    {
        return Entities.BankAccount.BankAccount.Create(
            userId: 1,
            bankAccountTitle: BankAccountTitle.FromString("Sample Account"),
            bankType: (BankType)BankType.Melli,
            cardNumber: CardNumber.FromString("1111-2222-3333-4444"),
            sheba: Sheba.FromString("111111111111111111111111"),
            accountNumber: AccountNumber.FromString("1111111111111111111")
        );
    }

    [Fact]
    public void Create_should_apply_BankAccount_created_event()
    {
        // Arrange
        var userId = 1;
        var bankAccountTitle = BankAccountTitle.FromString("Test");
        var bankType = (BankType)BankType.Melli;
        var cardNumber = CardNumber.FromString("1111-2222-3333-4444");
        var sheba = Sheba.FromString("111111111111111111111111");
        var accountNumber = AccountNumber.FromString("1111111111111111111");

        // Act
        var bankAccount = Entities.BankAccount.BankAccount.Create(userId, bankAccountTitle,
            bankType, cardNumber, sheba, accountNumber);

        // Assert
        Assert.NotNull(bankAccount);
        Assert.Equal(userId, bankAccount.UserId);
        Assert.Equal(bankAccountTitle, bankAccount.BankAccountTitle);
        Assert.Equal(bankType, bankAccount.BankType);
        Assert.Equal(cardNumber, bankAccount.CardNumber);
        Assert.Equal(sheba, bankAccount.Sheba);
        Assert.Equal(accountNumber, bankAccount.AccountNumber);
        Assert.Equal(ApprovingState.Created, bankAccount.State);
    }

    [Fact]
    public void Edit_should_update_BankAccount_properties()
    {
        // Arrange
        var bankAccount = CreateSampleBankAccount();
        var updatedTitle = BankAccountTitle.FromString("Updated Title");
        var updatedBankType = BankType.Pasargad;
        var updatedCardNumber = CardNumber.FromString("4444-3333-2222-1111");
        var updatedSheba = Sheba.FromString("222222222222222222222222");
        var updatedAccountNumber = AccountNumber.FromString("2222222222222222222");

        // Act
        bankAccount.Edit(updatedTitle, updatedBankType, updatedCardNumber, updatedSheba, updatedAccountNumber);

        // Assert
        Assert.Equal(updatedTitle, bankAccount.BankAccountTitle);
        Assert.Equal(updatedBankType, bankAccount.BankType);
        Assert.Equal(updatedCardNumber, bankAccount.CardNumber);
        Assert.Equal(updatedSheba, bankAccount.Sheba);
        Assert.Equal(updatedAccountNumber, bankAccount.AccountNumber);

    }

    [Fact]
    public void Remove_should_mark_BankAccount_as_deleted()
    {
        // Arrange
        var bankAccount = CreateSampleBankAccount();

        // Act
        bankAccount.Remove();

        // Assert
        Assert.True(bankAccount.IsDeleted);
    }

    [Fact]
    public void Approve_should_change_state_to_approved()
    {
        // Arrange
        var bankAccount = CreateSampleBankAccount();
        var approvedBy = 42;
        var desc = BankAccountDesc.FromString("Approved for special privileges");

        // Act
        bankAccount.Approve(approvedBy, desc);

        // Assert
        Assert.Equal(ApprovingState.Approved, bankAccount.State);
        Assert.Equal(approvedBy, bankAccount.ApprovedBy);
        Assert.Equal(desc, bankAccount.Desc);
    }

    [Fact]
    public void Reject_should_change_state_to_rejected()
    {
        // Arrange
        var bankAccount = CreateSampleBankAccount();
        var approvedBy = 42;
        var desc = BankAccountDesc.FromString("Rejected due to invalid information");

        // Act
        bankAccount.Reject(approvedBy, desc);

        // Assert
        Assert.Equal(ApprovingState.Rejected, bankAccount.State);
        Assert.Equal(approvedBy, bankAccount.ApprovedBy);
        Assert.Equal(desc, bankAccount.Desc);
    }
}