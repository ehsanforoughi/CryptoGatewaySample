namespace CryptoGateway.Service.Models.Wallet;

public static class WalletQueryModels
{
    public class GetWallets
    {
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetApprovedList
    {
        public string UserId { get; set; }
    }
}