namespace CryptoGateway.Domain.Entities.Wallet;

public static class WalletEvents
{
    public static class V1
    {
        public class WalletCreated
        {
            public int UserId { get; set; }
            public string Title { get; set; }
            public int CurrencyId { get; set; }
            public string Network { get; set; }
            public string Address { get; set; }
            public string? MemoAddress { get; set; } = null;
            public string? TagAddress { get; set; } = null;
        }

        public class WalletEdited
        {
            public int WalletId { get; set; }
            public string Title { get; set; }
            public int CurrencyId { get; set; }
            public string Network { get; set; }
            public string Address { get; set; }
            public string? MemoAddress { get; set; } = null;
            public string? TagAddress { get; set; } = null;
        }

        public class WalletRemoved
        {
            public int WalletId { get; set; }
        }

        public class WalletApproved
        {
            public int WalletId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }

        public class WalletRejected
        {
            public int WalletId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }
    }
}