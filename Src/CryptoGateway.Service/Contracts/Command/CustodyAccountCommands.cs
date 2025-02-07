namespace CryptoGateway.Service.Contracts.Command;

public class CustodyAccountCommands
{
    public static class V1
    {
        public class CreateCustodyAccount
        {
            public string CustomerId { get; set; }
            public string CurrencyType { get; set; }
            public string Title { get; set; }
        }
    }
}