namespace CryptoGateway.Service.Models.Payout;

public static class PayoutQueryModels
{
    public class GetWaitingPayouts
    {
    }   
    public class GetFiatPayouts
    {
        public string? UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetCryptoPayouts
    {
        public string? UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}