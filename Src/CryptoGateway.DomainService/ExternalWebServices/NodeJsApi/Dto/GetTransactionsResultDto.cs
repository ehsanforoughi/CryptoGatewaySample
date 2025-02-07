
namespace CryptoGateway.DomainService.ExternalWebServices.NodeJsApi.Dto;

public class GetTransactionsResultDto
{
    public Result result { get; set; }
}

public class Result
{
    public Datum[] data { get; set; }
    public bool success { get; set; }
    public Meta meta { get; set; }
}

public class Meta
{
    public long at { get; set; }
    public string fingerprint { get; set; }
    public Links links { get; set; }
    public int page_size { get; set; }
}

public class Links
{
    public string next { get; set; }
}

public class Datum
{
    public Ret[] ret { get; set; }
    public string[] signature { get; set; }
    public string txID { get; set; }
    public int net_usage { get; set; }
    public string raw_data_hex { get; set; }
    public int net_fee { get; set; }
    public int energy_usage { get; set; }
    public int blockNumber { get; set; }
    public long block_timestamp { get; set; }
    public int energy_fee { get; set; }
    public int energy_usage_total { get; set; }
    public Raw_Data raw_data { get; set; }
    public Internal_Transactions[] internal_transactions { get; set; }
    public string internal_tx_id { get; set; }
    public Data data { get; set; }
    public string to_address { get; set; }
    public string tx_id { get; set; }
    public string from_address { get; set; }
}

public class Raw_Data
{
    public Contract[] contract { get; set; }
    public string ref_block_bytes { get; set; }
    public string ref_block_hash { get; set; }
    public long expiration { get; set; }
    public long timestamp { get; set; }
    public int fee_limit { get; set; }
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
    public long balance { get; set; }
    public string resource { get; set; }
    public string receiver_address { get; set; }
    public string owner_address { get; set; }
    public int amount { get; set; }
    public string to_address { get; set; }
    public string data { get; set; }
    public string contract_address { get; set; }
    public int call_value { get; set; }
    public string asset_name { get; set; }
}

public class Data
{
    public string note { get; set; }
    public bool rejected { get; set; }
    public Call_Value call_value { get; set; }
}

public class Call_Value
{
    public long _ { get; set; }
}

public class Ret
{
    public string contractRet { get; set; }
    public int fee { get; set; }
}

public class Internal_Transactions
{
    public string internal_tx_id { get; set; }
    public Data1 data { get; set; }
    public string to_address { get; set; }
    public string from_address { get; set; }
}

public class Data1
{
    public string note { get; set; }
    public bool rejected { get; set; }
    public Call_Value1 call_value { get; set; }
}

public class Call_Value1
{
    public long _ { get; set; }
}
