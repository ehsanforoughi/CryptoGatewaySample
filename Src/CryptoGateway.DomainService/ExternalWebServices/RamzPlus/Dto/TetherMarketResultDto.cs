namespace CryptoGateway.DomainService.ExternalWebServices.RamzPlus.Dto;

public class TetherMarketResultDto
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
    public ResultM Result { get; set; }
    public int ResultCode { get; set; }

    public class ResultM
    {
        public string CoinName { get; set; }
        public string Market { get; set; }
        public int SellPrice { get; set; }
        public int BuyPrice { get; set; }
    }
}