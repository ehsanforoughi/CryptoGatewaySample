namespace CryptoGateway.Domain.Entities.PayIn;

public static class PayInEvents
{
    public static class V1
    {
        public class PayInCreated
        {
            public string PayInExternalId { get; set; }
            public int UserId { get; set; }
            public string CustomerId { get; set; }
            public decimal Value { get; set; }
            public string CurrencyType { get; set; }
            public string TxId { get; set; }
        }
    }
}