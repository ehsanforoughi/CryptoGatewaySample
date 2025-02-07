namespace CryptoGateway.Service.Contracts.Command;

public static class PayoutCommands
{
    public static class V1
    {
        public class CreateFiatRequest
        {
            public string UserId { get; set; }
            public decimal Amount { get; set; }
            public string CurrencyCode { get; set; }
            public int BankAccountId { get; set; }
        }

        public class CreateCryptoRequest
        {
            public string UserId { get; set; }
            public decimal Amount { get; set; }
            public string CurrencyCode { get; set; }
            public int WalletId { get; set; }
        }

        public class RejectPayout
        {
            public int PayoutId { get; set; }
            public int ApprovedBy { get; set; }
            public string? Desc { get; set; }
        }

        public class ApproveFiatManualWithdrawalRequest
        {
            public int PayoutId { get; set; }
            public int ApprovedBy { get; set; }
            public string? Desc { get; set; }
            public string? BankTrackingCode { get; set; }
        }

        public class ApproveCryptoManualWithdrawalRequest
        {
            public int PayoutId { get; set; }
            public int ApprovedBy { get; set; }
            public string? Desc { get; set; }
            public string? TransactionUrl { get; set; }
        }
    }
}