using System.Text.Json.Serialization;
using CryptoGateway.Domain.Extensions;

namespace CryptoGateway.Service.Models.ContractAccount;

public static class ContractAccReadModels
{
    public class ContractAccItems
    {
        public int ContractAccountId { get; set; }
        public string AddressBase58 { get; set; }
        public string AddressHex { get; set; }
    }

    public class ContractTransactionItems
    {
        public int ContractAccountId { get; set; }
        public int ContractTransactionId { get; set; }
        public string TxId { get; set; }
        public string ContractType { get; set; }
        public string ContractAddress { get; set; }
        public string ContractData { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public long TimeStamp { get; set; }
        public string Symbol { get; set; }
        public string ToAddress { get; set; }
    }
}