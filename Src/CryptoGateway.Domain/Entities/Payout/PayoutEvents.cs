namespace CryptoGateway.Domain.Entities.Payout;

public static class PayoutEvents
{
    public static class V1
    {
        public class FiatRequestCreated
        {
            public int UserId { get; set; }
            public decimal Amount { get; set; }
            public string CurrencyCode { get; set; }
            public int BankAccountId { get; set; }
        }

        public class CryptoRequestCreated
        {
            public int UserId { get; set; }
            public decimal Amount { get; set; }
            public string CurrencyCode { get; set; }
            public int WalletId { get; set; }
        }

        public class PayoutRejected
        {
            public int PayoutId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }

        public class FiatManualWithdrawalRequestApproved
        {
            public int PayoutId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
            public int BanAccountId { get; set; }
            public string BankTrackingCode { get; set; }
        }

        public class CryptoManualWithdrawalRequestApproved
        {
            public int PayoutId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
            public int WalletId { get; set; }
            public string TransactionUrl { get; set; }
        }
    }
}