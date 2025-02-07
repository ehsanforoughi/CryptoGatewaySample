namespace CryptoGateway.DomainService.ExternalWebServices.NodeJsApi.Dto;

public class GetTransactionByTxIdResultDto
{
    public Account account { get; set; }

    public class Account
    {
        public Ret[] ret { get; set; }
        public string[] signature { get; set; }
        public string txID { get; set; }
        public Raw_Data raw_data { get; set; }
        public string raw_data_hex { get; set; }
    }

    public class Raw_Data
    {
        public Contract[] contract { get; set; }
        public string ref_block_bytes { get; set; }
        public string ref_block_hash { get; set; }
        public long expiration { get; set; }
        public int fee_limit { get; set; }
        public long timestamp { get; set; }
    }

    public class Contract
    {
        public Parameter parameter { get; set; }
        public string type { get; set; }
    }

    public class Parameter
    {
        public Value value { get; set; }
        public string type_url { get; set; }
    }

    public class Value
    {
        public string data { get; set; }
        public string owner_address { get; set; }
        public string contract_address { get; set; }
    }

    public class Ret
    {
        public string contractRet { get; set; }
    }

}