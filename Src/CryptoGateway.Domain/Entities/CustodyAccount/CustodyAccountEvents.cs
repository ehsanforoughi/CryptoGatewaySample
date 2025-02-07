namespace CryptoGateway.Domain.Entities.CustodyAccount;

public static class CustodyAccountEvents
{
    public static class V1
    {
        public class CustodyAccountCreated
        {
            public string CustodyAccExternalId { get; set; }
            public string CurrencyType { get; set; }
            public string Title { get; set; }
            public int UserId { get; set; }
            public string CustomerId { get; set; }
        }
    }
}