using System.Text.Json.Serialization;
using CryptoGateway.Domain.Extensions;


namespace CryptoGateway.Service.Models.CustodyAccount;

public static class CustodyAccReadModels
{
    public class CustodyAccAllItems
    {
        public int CustodyAccountId { get; set; }
        public int ContractAccountId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserMobileNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContact { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CustodyAccListItem
    {
        public string CustodyAccId { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public byte CType { get; set; }
        public string CurrencyType => this.CType.ParseCurrencyTypeStr();
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserFullName { get; set; }
        public string CustomerId { get; set; }
        public string AddressBase58 { get; set; }
        public bool IsActive { get; set; }
        public string CustodyAccountLink { get; set; }
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
    }

    public class CustodyAccountLinkInfo
    {
        public string CustodyAccountId { get; set; }
        public string CustomerId { get; set; }
        public string Title { get; set; }
        public string CurrencyType { get; set; }
        public string Network { get; set; }
        public string Address { get; set; }
        public string CreatedAt { get; set; }
    }
}