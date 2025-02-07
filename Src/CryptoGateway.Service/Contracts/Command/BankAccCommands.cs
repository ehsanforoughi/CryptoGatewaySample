namespace CryptoGateway.Service.Contracts.Command;

public static class BankAccCommands
{
    public static class V1
    {
        public class CreateBankAcc
        {
            public string Title { get; set; }
            public int BankType { get; set; }
            public string CardNumber { get; set; }
            public string Sheba { get; set; }
            public string AccountNumber { get; set; }
        }

        public class EditBankAcc
        {
            public int BankAccountId { get; set; }
            public string Title { get; set; }
            public int BankType { get; set; }
            public string CardNumber { get; set; }
            public string Sheba { get; set; }
            public string AccountNumber { get; set; }
        }

        public class RemoveBankAcc
        {
            public int BankAccountId { get; set; }
        }
        public class Approve
        {
            public int BankAccountId { get; set; }
            public string ApprovedBy { get; set; }
            public string Desc { get; set; }
        }

        public class Reject
        {
            public int BankAccountId { get; set; }
            public string ApprovedBy { get; set; }
            public string Desc { get; set; }
        }
    }
}