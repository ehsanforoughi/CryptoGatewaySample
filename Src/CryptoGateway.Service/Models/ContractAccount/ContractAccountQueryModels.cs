namespace CryptoGateway.Service.Models.ContractAccount;

public static class ContractAccountQueryModels
{
    public class GetContractAccountList
    {
    }

    public class GetContractTransactionList
    {
        public int ContractAccountId { get; set; }
        public string ContractType { get; set; }
    }

    public class GetNewContractTransactionList
    {
        public int ContractAccountId { get; set; }
        public string ContractType { get; set; }
    }
}