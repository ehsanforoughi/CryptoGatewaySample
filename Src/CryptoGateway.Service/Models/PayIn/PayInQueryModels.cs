namespace CryptoGateway.Service.Models.PayIn;

public static class PayInQueryModels
{
    public class GetWaitingPayIns
    {
    }   
    public class GetPayIns
    {
        public string? UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}