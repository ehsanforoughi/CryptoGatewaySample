using System.Text.RegularExpressions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Extensions;

public static class PublicExtension
{
    public static int ExtractNumber(this string input)
    {
        var result = Regex.Replace(input, "[^0-9 _]", "");
        return string.IsNullOrWhiteSpace(result) ? 0 : int.Parse(result);
    }

    public static string[] SplitCamelCase(this string source)
        => Regex.Split(source, @"(?<!^)(?=[A-Z])");


    public static Money TruncateDecimal(this Money value, int decimalPlaces)
    {
        if (!value.ToString().Contains(".")) return value;
        return value.ToString().Split(".")[1].ToString().Length <= decimalPlaces ?
            value :
            Money.FromString(value.Amount.ToString().Split(".")[0] + "." + value.Amount.ToString().Split(".")[1].Substring(0, decimalPlaces), value.CurrencyType);
    }
}