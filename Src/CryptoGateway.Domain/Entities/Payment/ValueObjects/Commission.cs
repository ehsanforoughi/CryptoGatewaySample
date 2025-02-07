using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class Commission
{
    // Satisfy the serialization requirements
    protected Commission() { }

    internal Commission(Money percentage, Money fixedValue)
    {
        Percentage = percentage;
        FixedValue = fixedValue;
    }

    public static Commission FromValues(Money percentage, Money fixedValue)
        => new(percentage, fixedValue);

    public static Commission FromDecimal(decimal percentage, decimal fixedValue, CurrencyType currencyType)
        => new(Money.FromDecimal(percentage, currencyType),
               Money.FromDecimal(fixedValue, currencyType));

    public static Commission FromDecimal(decimal percentage, decimal fixedValue, string currencyType)
        => new(Money.FromDecimal(percentage, currencyType.Str2Enum<CurrencyType>()),
            Money.FromDecimal(fixedValue, currencyType.Str2Enum<CurrencyType>()));

    public Money Percentage { get; internal set; }
    public Money FixedValue { get; internal set; }
    public static Commission NoCommission()
        => new(Money.FromDecimal(0, CurrencyType.None),
               Money.FromDecimal(0, CurrencyType.None));
}