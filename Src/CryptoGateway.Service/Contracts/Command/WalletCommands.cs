namespace CryptoGateway.Service.Contracts.Command;

public static class WalletCommands
{
    public static class V1
    {
        public class CreateWallet
        {
            public string UserId { get; set; }
            public string Title { get; set; }
            public string CurrencyType { get; set; }
            public string Network { get; set; }
            public string Address { get; set; }
            public string? MemoAddress { get; set; }
            public string? TagAddress { get; set; }
        }

        public class EditWallet
        {
            public int WalletId { get; set; }
            public string Title { get; set; }
            public string CurrencyType { get; set; }
            public string Network { get; set; }
            public string Address { get; set; }
            public string? MemoAddress { get; set; }
            public string? TagAddress { get; set; }
        }

        public class RemoveWallet
        {
            public int WalletId { get; set; }
        }

        public class Approve
        {
            public int WalletId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }

        public class Reject
        {
            public int WalletId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }
    }
}