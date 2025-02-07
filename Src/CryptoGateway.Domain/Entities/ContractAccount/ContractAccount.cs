using Bat.Core;
using CryptoGateway.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.Domain.Entities.ContractAccount;

public class ContractAccount : AggregateRoot<ContractAccountId>
    , IEntity, IInsertDateProperty, IModifyDateProperty, ISoftDeleteProperty
{
    #region Constructors
    protected ContractAccount() { }
    #endregion

    #region Events
    public static ContractAccount Create(ContractAddress contractAddress,
        PublicKey publicKey, PrivateKey privateKey)
    {
        var contractAccount = new ContractAccount();
        contractAccount.Apply(new ContractAccountEvents.V1.ContractAccountCreated
        {
            AddressBase58 = contractAddress.Base58Value,
            AddressHex = contractAddress.HexValue,
            PublicKey = publicKey,
            PrivateKey = privateKey
        });

        return contractAccount;
    }

    public void AddContractTransaction(string txId, long? timestamp, string contractType, string contractResource, string contractData,
        string contractAddress, string ownerAddress, string receiverAddress, string fromAddress, string toAddress, decimal? amount, long? expiration, 
        string refBlockBytes, string refBlockHash, decimal? feeLimit, string signature, int? energyUsage, decimal? energyFee, 
        int? gasLimit, decimal? gasPrice, int? bandwidthUsage, decimal? bandwidthFee)
    {
        Apply(new ContractAccountEvents.V1.ContractTransactionAdded
        {
            TxId = txId,
            Timestamp = timestamp,
            ContractType = contractType,
            ContractResource = contractResource,
            ContractData = contractData,
            ContractAddress = contractAddress,
            OwnerAddress = ownerAddress,
            ReceiverAddress = receiverAddress,
            FromAddress = fromAddress,
            ToAddress = toAddress,
            Amount = amount,
            Expiration = expiration,
            RefBlockBytes = refBlockBytes,
            RefBlockHash = refBlockHash,
            FeeLimit = feeLimit,
            Signature = signature,
            EnergyUsage = energyUsage,
            EnergyFee = energyFee,
            GasLimit = gasLimit,
            GasPrice = gasPrice,
            BandwidthUsage = bandwidthUsage,
            BandwidthFee = bandwidthFee
        });
    }

    public void AddContractTrc20Transaction(string txId, long? timestamp, string contractType, string ownerAddress, string receiverAddress, 
        string toAddress, decimal? amount, string symbol)
    {
        Apply(new ContractAccountEvents.V1.ContractTrc20TransactionAdded
        {
            TxId = txId,
            Timestamp = timestamp,
            ContractType = contractType,
            OwnerAddress = ownerAddress,
            ReceiverAddress = receiverAddress,
            ToAddress = toAddress,
            Amount = amount,
            Symbol = symbol,
        });
    }

    public void ActivateContractAcc() => IsActive = IsActive.FromBoolean(true);

    protected override void When(object @event)
    {
        switch (@event)
        {
            case ContractAccountEvents.V1.ContractAccountCreated e:
                Address = ContractAddress.FromString(e.AddressBase58, e.AddressHex);
                PublicKey = PublicKey.FromString(e.PublicKey);
                PrivateKey = PrivateKey.FromString(e.PrivateKey);
                break;
            case ContractAccountEvents.V1.ContractTransactionAdded e:
                var contractTransaction = new ContractTransaction.ContractTransaction(Apply);
                ApplyToEntity(contractTransaction, e);
                ContractTransactions ??= new List<ContractTransaction.ContractTransaction>();
                ContractTransactions.Add(contractTransaction);
                break;
            case ContractAccountEvents.V1.ContractTrc20TransactionAdded e:
                var contractTrc20Transaction = new ContractTransaction.ContractTransaction(Apply);
                ApplyToEntity(contractTrc20Transaction, e);
                ContractTransactions ??= new List<ContractTransaction.ContractTransaction>();
                ContractTransactions.Add(contractTrc20Transaction);
                break;
        }
    }

    protected override void EnsureValidState() { }
    #endregion

    #region Properties
    public ContractAccountId ContractAccountId { get; private set; }

    [ForeignKey(nameof(CustodyAccountId))]
    public CustodyAccount.CustodyAccount CustodyAccount { get; private set; }
    public CustodyAccountId CustodyAccountId { get; private set; }
    public ContractAddress Address { get; private set; }
    public PublicKey PublicKey { get; private set; }
    public PrivateKey PrivateKey { get; private set; }
    public IsActive IsActive { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    public List<ContractTransaction.ContractTransaction> ContractTransactions { get; set; }
    #endregion
}