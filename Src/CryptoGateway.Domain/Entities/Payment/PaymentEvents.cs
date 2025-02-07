namespace CryptoGateway.Domain.Entities.Payment;

public static class PaymentEvents
{
    public static class V1
    {
        public class PaymentCreated
        {
            public int UserId { get; set; }
            public string PaymentExternalId { get; set; }
            public decimal PriceAmount { get; set; }
            public string PriceCurrencyCode { get; set; }
            public string PayCurrencyCode { get; set; }
            public string CustomerId { get; set; }
            public string CustomerContact { get; set; }
            public string OrderId { get; set; }
            public string OrderDescription { get; set; }
        }

        public class PayAmountEstimationUpdated
        {
            //public int Id { get; set; }
            public decimal PayAmount { get; set; }
            public string PayCurrencyCode { get; set; }
            public DateTime ExpirationDateMi { get; set; }
        }

        public class CustodyAccountAssigned
        {
            //public int Id { get; set; }
            public int CustodyAccountId { get; set; }
        }
    }
}