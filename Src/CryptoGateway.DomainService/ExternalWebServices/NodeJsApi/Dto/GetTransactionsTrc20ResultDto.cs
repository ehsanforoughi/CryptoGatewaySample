namespace CryptoGateway.DomainService.ExternalWebServices.NodeJsApi.Dto;

public class GetTransactionsTrc20ResultDto
{
    public Result result { get; set; }
    public class Result
    {
        public Datum[] data { get; set; }
        public bool success { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public long at { get; set; }
        public int page_size { get; set; }
    }

    public class Datum
    {
        public string transaction_id { get; set; }
        public Token_Info token_info { get; set; }
        public long block_timestamp { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public long value { get; set; }
    }

    public class Token_Info
    {
        public string symbol { get; set; }
        public string address { get; set; }
        public int decimals { get; set; }
        public string name { get; set; }
    }

}