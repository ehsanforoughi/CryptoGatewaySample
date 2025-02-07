namespace CryptoGateway.Domain.Entities.UserCredit;

public static class UserCreditEvents
{
    public static class V1
    {
        public class CreditTransactionAdded
        {
            public string TransactionType { get; set; }
            public string TransactionActionType { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }
            public string CurrencyType { get; set; }
            public decimal ComPercentageAmount { get; set; }
            public decimal ComFixedValueAmount { get; set; }
        }

        public class DebitTransactionAdded
        {
            public string TransactionType { get; set; }
            public string TransactionActionType { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }
            public string CurrencyType { get; set; }
        }

        public class CommissionTransactionAdded
        {
            public string TransactionType { get; set; }
            public string TransactionActionType { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }
            public string CurrencyType { get; set; }
            public decimal ComPercentageAmount { get; set; }
            public decimal ComFixedValueAmount { get; set; }
            public decimal FinalCommissionValue { get; set; }
        }
    }
}