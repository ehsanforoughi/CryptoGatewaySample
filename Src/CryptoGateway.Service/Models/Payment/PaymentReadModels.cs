namespace CryptoGateway.Service.Models.Payment;

public static class PaymentReadModels
{
    public class PaymentListItem
    {
        public string PaymentId { get; set; }
        public string UserId { get; set; }
        public decimal PriceAmount { get; set; }
        public string PriceCurrencyType { get; set; }
        public decimal PayAmount { get; set; }
        public string PayCurrencyType { get; set; }
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public string OrderDesc { get; set; }
        public string PaymentLink { get; set; }
        public string CreatedAt { get; set; }
    }

    public class WaitingPaymentsListItem
    {
        public int PaymentId { get; set; }
        public string PaymentExternalId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserMobileNumber { get; set; }
        public string CustomerId { get; set; }
        public int ContractAccountId { get; set; }
        public decimal PriceAmount { get; set; }
        public byte PriceCurrencyType { get; set; }
        public decimal PayAmount { get; set; }
        public byte PayCurrencyType { get; set; }
        public byte PaymentState { get; set; }
        public string AddressBase58 { get; set; }
        public string AddressHex { get; set; }
        public string OrderId { get; set; }
        public string OrderDesc { get; set; }
        public string PaymentLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    public class PaymentLinkInfo
    {
        public string PaymentId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContact { get; set; }
        public string OrderId { get; set; }
        public string OrderDesc { get; set; }
        public string PriceAmount { get; set; }
        public string PriceCurrencyType { get; set; }
        public string PayAmount { get; set; }
        public string PayCurrencyType { get; set; }
        public string Network { get; set; }
        public string Address { get; set; }
        public string CreatedAt { get; set; }
    }
}