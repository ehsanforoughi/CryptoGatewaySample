namespace CryptoGateway.Service.Contracts.Command;

public static class PaymentCommands
{
    public static class V1
    {
        public class CreatePayment
        {
            public string PayCurrencyCode { get; set; }
            public decimal PriceAmount { get; set; }
            public string CustomerId { get; set; }
            public string CustomerContact { get; set; }
            public string OrderId { get; set; }
            public string OrderDescription { get; set; }
        }
    }
}