namespace CryptoGateway.Service.Models.Payment;

public static class PaymentQueryModels
{
    public class GetOwnersPayment
    {
        public string? UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetWaitingPayments
    {
    }

    public class GetPaymentLinkInfo
    {
        public string? UserId { get; set; }
        public string PaymentId { get; set; }
    }
}