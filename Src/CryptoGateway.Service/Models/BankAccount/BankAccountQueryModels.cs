namespace CryptoGateway.Service.Models.BankAccount;

public static class BankAccountQueryModels
{
    public class GetBankAccounts
    {
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetBankTypes
    {
    }

    public class GetApprovedList
    {
        public string UserId { get; set; }
    }
}