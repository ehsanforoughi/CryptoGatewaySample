using Moq;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;
using CryptoGateway.Domain.Entities.Payout.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.Payout;

public class PayoutTest
{
    private Entities.Payout.Payout CreateSampleFiatPayOut()
    {
        return Entities.Payout.Payout.CreateFiatPayout(
            userId: 1, Money.FromDecimal(1000, CurrencyType.IRR), bankAccountId: new BankAccountId(1)
        );
    }

    private Entities.Payout.Payout CreateSampleCryptoPayOut()
    {
        return Entities.Payout.Payout.CreateCryptoPayout(
            userId: 1, Money.FromDecimal(10, CurrencyType.USDT), walletId: new WalletId(1)
            );
    }
    
    [Fact]
    public void Create_fiat_payout_should_apply_created_event()
    {
        // Arrange
        var userId = 1;
        var fiatPrice = Money.FromDecimal(10000, CurrencyType.IRR);
        var bankAccountId = new BankAccountId(1);

        // Act
        var payout = Entities.Payout.Payout.CreateFiatPayout(userId, fiatPrice, bankAccountId);

        // Assert
        Assert.NotNull(payout);
    }

    [Fact]
    public void Create_crypto_payout_should_apply_created_event()
    {
        // Arrange
        var userId = 1;
        var cryptoPrice = Money.FromDecimal(5, CurrencyType.USDT);
        var walletId = new WalletId(1);

        // Act
        var payout = Entities.Payout.Payout.CreateCryptoPayout(userId, cryptoPrice, walletId);

        // Assert
        Assert.NotNull(payout);
    }

    [Fact]
    public void Approve_manual_fiat_should_change_state_to_approved()
    {
        // Arrange
        var payout = CreateSampleFiatPayOut();
        var payOutId = new PayoutId(1);
        var approvedBy = 1;
        var payoutDesc = PayoutDesc.FromString("Approved for special privileges");
        var bankTrackingCode = BankTrackingCode.FromString("12345");

        // Act
        payout.ApproveManualFiatWithdrawalRequest(payout.User, approvedBy, payoutDesc, bankTrackingCode);

        // Assert
        Assert.Equal(ApprovingState.Approved, payout.State);
        Assert.Equal(approvedBy, payout.ApprovedBy);
        Assert.Equal(payoutDesc, payout.Desc);
    }

    [Fact]
    public void Approve_manual_crypto_should_change_state_to_approved()
    {
        // Arrange
        var payout = CreateSampleCryptoPayOut();
        var payOutId = new PayoutId(1);
        var approvedBy = 1;
        var payoutDesc = PayoutDesc.FromString("Approved for special privileges");
        var transactionUrl = TransactionUrl.FromString("12345");

        // Act
        payout.ApproveManualCryptoWithdrawalRequest(payout.User, approvedBy, payoutDesc, transactionUrl);

        // Assert
        Assert.Equal(ApprovingState.Approved, payout.State);
        Assert.Equal(approvedBy, payout.ApprovedBy);
        Assert.Equal(payoutDesc, payout.Desc);
    }

    [Fact]
    public void Reject_should_change_state_to_rejected()
    {
        // Arrange
        var payout = CreateSampleFiatPayOut();
        var payOutId = new PayoutId(1);
        var approvedBy = 1;
        var payoutDesc = PayoutDesc.FromString("Rejected due to invalid information");

        // Act
        payout.Reject(payOutId, approvedBy, payoutDesc);

        // Assert
        Assert.Equal(ApprovingState.Rejected, payout.State);
        Assert.Equal(approvedBy, payout.ApprovedBy);
        Assert.Equal(payoutDesc, payout.Desc);
    }
}