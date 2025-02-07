namespace CryptoGateway.Service.Models.Currency;

public static class CurrencyQueryModels
{
    public class GetCurrencies
    {
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public byte DecimalPlaces { get; set; }
        public bool InUse { get; set; }
    }
}