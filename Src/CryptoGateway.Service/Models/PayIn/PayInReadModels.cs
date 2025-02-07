using System.Text.Json.Serialization;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Service.Models.PayIn;

public static class PayInReadModels
{
    public class WaitingPayInsListItem
    {
        public int PayInId { get; set; }
        public int UserId { get; set; }
        public int ContractAccountId { get; set; }
        public string UserEmail { get; set; }
        public string UserMobileNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal Value { get; set; }
        public byte CurrencyType { get; set; }
        public string TxId { get; set; }
        public string CustomerContact { get; set; }
        public string AddressBase58 { get; set; }
        public string AddressHex { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PayInListItem
    {
        public string PayInId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string CustomerId { get; set; }
        public decimal Value { get; set; }
        [JsonIgnore]
        public byte VCType { get; set; }
        public string ValueCurrencyType => VCType.Byte2EnumStr<CurrencyType>()!;
        public decimal CommissionValue { get; set; }
        [JsonIgnore]
        public byte CCType { get; set; }
        public string CommissionCurrencyType => CCType.Byte2EnumStr<CurrencyType>()!;
        public string TxId { get; set; }
        public string CustomerContact { get; set; }
        public string CreatedAt { get; set; }
        //public string ModifiedAt { get; set; }
    }
}