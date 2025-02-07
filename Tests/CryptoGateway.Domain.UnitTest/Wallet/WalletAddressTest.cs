using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.Wallet;

public class WalletAddressTest
{
    private Entities.Wallet.Wallet CreateSampleWallet()
    {
        return Entities.Wallet.Wallet.Create(
            userId: 1,
            walletTitle: WalletTitle.FromString("Sample Wallet"),
            currencyId: new CurrencyId(1),
            network: Network.FromString("TRC20"),
            address: WalletAddress.FromString("TestAddress"),
            memo: MemoAddress.FromString("TestMemo"),
            tag: TagAddress.FromString("TestTag")
            );
    }

    [Fact]
    public void Create_should_apply_wallet_created_event()
    {
        // Arrange
        var userId = 1;
        var walletTitle = WalletTitle.FromString("Test");
        var currencyId = new CurrencyId(1);
        var network = Network.FromString("TRC20");
        var walletAddress = WalletAddress.FromString("TestAddress");
        var memoAddress = MemoAddress.FromString("TestMemo");
        var tagAddress = TagAddress.FromString("TestTag");

        // Act
        var wallet =
            Entities.Wallet.Wallet.Create(userId, walletTitle, currencyId,
                network, walletAddress, memoAddress, tagAddress);

        // Assert
        Assert.NotNull(wallet);
        Assert.Equal(userId, wallet.UserId);
        Assert.Equal(walletTitle, wallet.WalletTitle);
        Assert.Equal(currencyId, wallet.CurrencyId);
        Assert.Equal(network, wallet.Network);
        Assert.Equal(walletAddress, wallet.Address);
        Assert.Equal(memoAddress, wallet.MemoAddress);
        Assert.Equal(tagAddress, wallet.TagAddress);
    }

    [Fact]
    public void Edit_should_update_wallet_properties()
    {
        // Arrange
        var wallet = CreateSampleWallet();
        var walletTitle = WalletTitle.FromString("Updated Title");
        var currencyId = new CurrencyId(2);
        var network = Network.FromString("TRC10");
        var walletAddress = WalletAddress.FromString("Updated Address");
        var memoAddress = MemoAddress.FromString("Updated Memo");
        var tagAddress = TagAddress.FromString("Updated Tag");

        // Act
        wallet.Edit(walletTitle, currencyId, network, walletAddress, memoAddress, tagAddress);

        // Assert
        Assert.Equal(walletTitle, wallet.WalletTitle);
        Assert.Equal(currencyId, wallet.CurrencyId);
        Assert.Equal(network, wallet.Network);
        Assert.Equal(walletAddress, wallet.Address);
        Assert.Equal(memoAddress, wallet.MemoAddress);
        Assert.Equal(tagAddress, wallet.TagAddress);
    }

    [Fact]
    public void Remove_should_mark_wallet_as_deleted()
    {
        // Arrange
        var wallet = CreateSampleWallet();

        // Act
        wallet.Remove();

        // Assert
        Assert.True(wallet.IsDeleted);
    }

    [Fact]
    public void Approve_should_change_state_to_approved()
    {
        // Arrange
        var wallet = CreateSampleWallet();
        var approvedBy = 1;
        var walletDesc = WalletDesc.FromString("Approved for special privileges");

        // Act
        wallet.Approve(approvedBy, walletDesc);

        // Assert
        Assert.Equal(ApprovingState.Approved, wallet.State);
        Assert.Equal(approvedBy, wallet.ApprovedBy);
        Assert.Equal(walletDesc, wallet.Desc);
    }

    [Fact]
    public void Reject_should_change_state_to_rejected()
    {
        // Arrange
        var wallet = CreateSampleWallet();
        var approvedBy = 1;
        var walletDesc = WalletDesc.FromString("Rejected due to invalid information");

        // Act
        wallet.Reject(approvedBy, walletDesc);

        // Assert
        Assert.Equal(ApprovingState.Rejected, wallet.State);
        Assert.Equal(approvedBy, wallet.ApprovedBy);
        Assert.Equal(walletDesc, wallet.Desc);
    }
}