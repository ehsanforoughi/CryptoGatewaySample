namespace CryptoGateway.Framework;

public static class Helpers
{
    public static decimal TruncateDecimal(this decimal value, int decimalPlaces)
    {
        if (!value.ToString().Contains(".")) return value;
        return value.ToString().Split(".")[1].ToString().Length <= decimalPlaces ?
            value :
            Convert.ToDecimal(value.ToString().Split(".")[0] + "." + value.ToString().Split(".")[1].Substring(0, decimalPlaces));
    }
}