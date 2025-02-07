namespace CryptoGateway.Domain.Entities.BankAccount;

public static class BankAccountEvents
{
    public static class V1
    {
        public class BankAccCreated
        {
            //public int BankAccountId { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public byte BankType { get; set; }
            public string CardNumber { get; set; }
            public string Sheba { get; set; }
            public string AccountNumber { get; set; }
        }

        public class BankAccEdited
        {
            public int BankAccountId { get; set; }
            public string Title { get; set; }
            public int BankType { get; set; }
            public string CardNumber { get; set; }
            public string Sheba { get; set; }
            public string AccountNumber { get; set; }
        }

        public class BankAccRemoved
        {
            public int BankAccountId { get; set; }
        }

        public class BankAccountApproved
        {
            public int BankAccountId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }

        public class BankAccountRejected
        {
            public int BankAccountId { get; set; }
            public int ApprovedBy { get; set; }
            public string Desc { get; set; }
        }
    }
}