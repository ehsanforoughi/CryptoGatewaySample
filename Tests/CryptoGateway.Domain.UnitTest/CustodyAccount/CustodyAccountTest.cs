using KsuidDotNet;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.CustodyAccount;

public class CustodyAccountTest
{
    private Entities.CustodyAccount.CustodyAccount CreateSampleCustodyAccount()
    {
        return Entities.CustodyAccount.CustodyAccount.Create(
            custodyAccExternalId: new CustodyAccExternalId(Ksuid.NewKsuid()), currencyType: CurrencyType.USDT,
            title: CustodyAccountTitle.FromString("Test Title"), userId: 1, customerId: CustomerId.FromString("123"));
    }

    private Entities.ContractAccount.ContractAccount CreateSampleContractAccount()
    {
        return Entities.ContractAccount.ContractAccount.Create(
            contractAddress: ContractAddress.FromString("Base 58 Value", "Hex Value"),
            publicKey: PublicKey.FromString("Public Key"),
            privateKey: PrivateKey.FromString("Private Key")
        );
    }

    [Fact]
    public void Create_should_apply_custody_account_created_event()
    {
        // Arrange
        var custodyAccExternalId = new CustodyAccExternalId(Ksuid.NewKsuid());
        var currencyType = CurrencyType.USDT;
        var custodyAccountTitle = CustodyAccountTitle.FromString("Test Title");
        var userId = 1;
        var customerId = CustomerId.FromString("123");

        // Act
        var custodyAccount = Entities.CustodyAccount.CustodyAccount.Create(
            custodyAccExternalId: custodyAccExternalId, currencyType: currencyType, 
            title: custodyAccountTitle, userId: userId, customerId: customerId
            );

        // Assert
        Assert.NotNull(custodyAccount);
        Assert.Equal(custodyAccExternalId, custodyAccount.CustodyAccExternalId);
        Assert.Equal(currencyType, custodyAccount.CurrencyType);
        Assert.Equal(userId, custodyAccount.UserId);
        Assert.Equal(customerId.Value, custodyAccount.CustomerId.Value);
        Assert.Equal(custodyAccountTitle, custodyAccount.Title);
    }

    [Fact]
    public void Assign_contract_account_should_attach_it_to_custody_account()
    {
        // Arrange
        var custodyAccount = CreateSampleCustodyAccount();
        var contractAccount = CreateSampleContractAccount();

        // Act
        custodyAccount.AssignContractAccount(contractAccount);

        // Assert
        Assert.NotNull(custodyAccount);
        Assert.NotNull(custodyAccount.ContractAccount);
    }
}