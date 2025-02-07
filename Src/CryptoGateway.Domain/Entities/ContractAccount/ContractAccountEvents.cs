using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.ContractAccount;

public static class ContractAccountEvents
{
    public static class V1
    {
        public class ContractAccountCreated
        {
            public string AddressBase58 { get; set; }
            public string AddressHex { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public class ContractTransactionAdded
        {
            public string TxId { get; set; }
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
            public int? EnergyUsage { get; set; }
            public decimal? EnergyFee { get; set; }
            public int? GasLimit { get; set; }
            public decimal? GasPrice { get; set; }
            public int? BandwidthUsage { get; set; }
            public decimal? BandwidthFee { get; set; }
        }

        public class ContractTrc20TransactionAdded
        {
            public string TxId { get; set; }
            public long? Timestamp { get; set; }
            public string ContractType { get; set; }
            public string OwnerAddress { get; set; }
            public string ReceiverAddress { get; set; }
            public string ToAddress { get; set; }
            public decimal? Amount { get; set; }
            public string Symbol { get; set; }
        }
    }
}