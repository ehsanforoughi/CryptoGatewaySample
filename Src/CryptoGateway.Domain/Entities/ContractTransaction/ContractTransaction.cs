using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.ContractTransaction.ValueObjects;


namespace CryptoGateway.Domain.Entities.ContractTransaction;

public class ContractTransaction : Entity<ContractTransactionId>,
    IEntity, IInsertDateProperty, IModifyDateProperty
{
    #region Constructors
    protected ContractTransaction() { }

    public ContractTransaction(Action<object> applier) : base(applier) { }
    #endregion

    #region Events
    protected override void When(object @event)
    {
        switch (@event)
        {
            case ContractAccountEvents.V1.ContractTransactionAdded e:
                TxId = TxId.FromString(e.TxId);
                Timestamp = e.Timestamp;
                ContractType = e.ContractType;
                ContractResource = e.ContractResource;
                ContractData = e.ContractData;
                ContractAddress = e.ContractAddress;
                OwnerAddress = e.OwnerAddress;
                ReceiverAddress = e.ReceiverAddress;
                FromAddress = e.FromAddress;
                ToAddress = e.ToAddress;
                Amount = e.Amount;
                Expiration = e.Expiration;
                RefBlockBytes = e.RefBlockBytes;
                RefBlockHash = e.RefBlockHash;
                FeeLimit = e.FeeLimit;
                Signature = e.Signature;
                EnergyUsage = e.EnergyUsage;
                EnergyFee = e.EnergyFee;
                GasLimit = e.GasLimit;
                GasPrice = e.GasPrice;
                BandwidthUsage = e.BandwidthUsage;
                BandwidthFee = e.BandwidthFee;
                break;
            case ContractAccountEvents.V1.ContractTrc20TransactionAdded e:
                TxId = TxId.FromString(e.TxId);
                Timestamp = e.Timestamp;
                ContractType = e.ContractType;
                OwnerAddress = e.OwnerAddress;
                ReceiverAddress = e.ReceiverAddress;
                ToAddress = e.ToAddress;
                Amount = e.Amount;
                Symbol = e.Symbol;
                break;
        }
    }
    #endregion

    #region Properties
    public ContractTransactionId ContractTransactionId { get; set; }
    public ContractAccount.ContractAccount ContractAccount { get; private set; }
    public TxId TxId { get; private set; }
    public long? Timestamp { get; set; }
    public string ContractType { get; set; }
    public string ContractResource { get; set; }
    public string ContractData { get; set; }
    public string ContractAddress { get; set; }
    public string OwnerAddress { get; set; }
    public string ReceiverAddress { get; set; }
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public decimal? Amount { get; set; }
    public long? Expiration { get; set; }
    public string RefBlockBytes { get; set; }
    public string RefBlockHash { get; set; }
    public decimal? FeeLimit { get; set; }
    public string Signature { get; set; }
    public string Symbol { get; set; }
    public int? EnergyUsage { get; set; }
    public decimal? EnergyFee { get; set; }
    public int? GasLimit { get; set; }
    public decimal? GasPrice { get; set; }
    public int? BandwidthUsage { get; set; }
    public decimal? BandwidthFee { get; set; }

    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    #endregion
}