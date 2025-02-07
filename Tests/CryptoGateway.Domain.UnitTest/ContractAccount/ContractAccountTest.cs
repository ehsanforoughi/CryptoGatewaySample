using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.ContractAccount;

public class ContractAccountTest
{
    private Entities.ContractAccount.ContractAccount CreateSampleContractAccount()
    {
        return Entities.ContractAccount.ContractAccount.Create(
            contractAddress: ContractAddress.FromString("Base 58 Value", "Hex Value"),
            publicKey: PublicKey.FromString("Public Key"),
            privateKey: PrivateKey.FromString("Private Key")
        );
    }

    [Fact]
    public void Create_should_apply_contract_account_created_event()
    {
        // Arrange
        var contractAddress = ContractAddress.FromString("Base 58 Value", "Hex Value");
        var publicKey = PublicKey.FromString("Public Key");
        var privateKey = PrivateKey.FromString("Private Key");

        // Act
        var contractAccount =  Entities.ContractAccount.ContractAccount.Create(
            contractAddress: contractAddress, publicKey: publicKey, privateKey: privateKey
            );

        // Assert
        Assert.NotNull(contractAccount);
        Assert.Equal(contractAddress, contractAccount.Address);
        Assert.Equal(publicKey, contractAccount.PublicKey);
        Assert.Equal(privateKey, contractAccount.PrivateKey);
    }

    [Fact]
    public void Activate_contract_account_should_be_done()
    {
        // Arrange
        var contractAccount = CreateSampleContractAccount();

        // Act
        contractAccount.ActivateContractAcc();

        // Assert
        Assert.True(contractAccount.IsActive);
    }

    [Fact]
    public void Add_contract_transaction_should_create_a_transaction_for_contract_account()
    {
        // Arrange
        var contractAccount = CreateSampleContractAccount();

        // Act
        contractAccount.AddContractTransaction(txId: "Test TxId", timestamp: 1234567890, contractType: "Test Type", contractResource: "test Resource",
            contractData: "Test Data", contractAddress: "Test Contract Address", ownerAddress: "Test Owner Address", receiverAddress: "Test Receive Address",
            fromAddress: "Test From Address", toAddress: "Test To Address", amount: null, expiration: null, refBlockBytes: "Test Ref Block Bytes",
            refBlockHash: "Test Ref Block Hash", feeLimit: null, signature: "Test Signature", energyUsage: null, energyFee: null, gasLimit: null,
            gasPrice: null, bandwidthUsage: null, bandwidthFee: null);

        // Assert
        Assert.NotNull(contractAccount.ContractTransactions);
        Assert.Single(contractAccount.ContractTransactions);
    }

    [Fact]
    public void Add_contract_trc20_transaction_should_create_a_transaction_for_contract_account()
    {
        // Arrange
        var contractAccount = CreateSampleContractAccount();

        // Act
        contractAccount.AddContractTrc20Transaction(txId: "Test TxId", timestamp: 1234567890, contractType: "Test Type", 
            ownerAddress: "Test Owner Address", receiverAddress: "Test Receive Address", toAddress: "Test To Address", amount: null, 
            symbol: "Test Symbol");

        // Assert
        Assert.NotNull(contractAccount.ContractTransactions);
        Assert.Single(contractAccount.ContractTransactions);
    }
}