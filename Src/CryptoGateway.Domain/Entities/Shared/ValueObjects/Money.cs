using CryptoGateway.Framework;
using System.Collections.Generic;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Currency.ValueObjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class Money : Value<Money>
{
    protected Money() { }
    protected Money(decimal amount, CurrencyType currencyType //, ICurrencyLookup currencyLookup
    )
    {
        //if (currencyType == default)
        //    throw new ArgumentNullException(nameof(currencyType), "Currency code must be specified");

        if (amount < 0)
            throw new ArgumentException("Price cannot be negative", nameof(amount));

        //var currency = currencyLookup.FindCurrency(currencyType);
        var currency = FindCurrency(currencyType);
        if (!currency.IsActive)
            throw new ArgumentException($"Currency {currencyType} is not valid");

        if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            throw new ArgumentOutOfRangeException(
                nameof(amount), $"Amount in {currencyType} cannot have more than {currency.DecimalPlaces} decimals");

        Amount = amount;
        CurrencyType = currencyType;
    }

    public decimal Amount { get; internal set; }
    public CurrencyType CurrencyType { get; internal set; }

    private static Currency.Currency FindCurrency(CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.None:
                return new Currency.Currency(CurrencyType.None, DecimalPlaces.NoDecimalPlaces,
                    IsActive.FromBoolean(true), IsFiat.FromBoolean(false));
            case CurrencyType.IRR:
                return new Currency.Currency(currencyType, DecimalPlaces.FromByte(0),
                    IsActive.FromBoolean(true), IsFiat.FromBoolean(true));
            case CurrencyType.USDT:
                return new Currency.Currency(currencyType, DecimalPlaces.FromByte(6),
                    IsActive.FromBoolean(true), IsFiat.FromBoolean(false));
            case CurrencyType.USD:
                return new Currency.Currency(currencyType, DecimalPlaces.FromByte(2),
                    IsActive.FromBoolean(true), IsFiat.FromBoolean(true));
            case CurrencyType.TRX:
                return new Currency.Currency(currencyType, DecimalPlaces.FromByte(6),
                    IsActive.FromBoolean(true), IsFiat.FromBoolean(false));
            default:
                throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
        }
    }

    public static Money FromDecimal(decimal amount, CurrencyType currencyType) => new(amount, currencyType);
    public static Money FromNotClearAmount(CurrencyType currencyType) => new(0, currencyType);
    public static Money FromNotClearAmount(string currencyType) => new(0, currencyType.ParseCurrencyType());
    public static Money FromString(string amount, CurrencyType currencyType) => new(decimal.Parse(amount), currencyType);
    public static Money FromString(decimal amount, string currencyType) => new(amount, currencyType.ParseCurrencyType());
    public static Money FromString(string amount, string currencyType) => new(decimal.Parse(amount), currencyType.ParseCurrencyType());

    #region Operators
    public Money Add(Money summand)
    {
        if (CurrencyType != summand.CurrencyType)
            throw new CurrencyMismatchException("Cannot sum amounts with different currencies");

        var amount = (Amount + summand.Amount).TruncateDecimal(FindCurrency(summand.CurrencyType).DecimalPlaces);
        return new Money(amount, CurrencyType);
    }

    public Money Subtract(Money subtrahend)
    {
        if (CurrencyType != subtrahend.CurrencyType)
            throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");

        var amount = (Amount - subtrahend.Amount).TruncateDecimal(FindCurrency(subtrahend.CurrencyType).DecimalPlaces);
        return new Money(amount, CurrencyType);
    }

    public Money Multiply(Money second)
    {
        if (CurrencyType != second.CurrencyType)
            throw new CurrencyMismatchException("Cannot multiply amounts with different currencies");

        var amount = (Amount * second.Amount).TruncateDecimal(FindCurrency(second.CurrencyType).DecimalPlaces);
        return new Money(amount, CurrencyType);
    }

    public Money Division(Money second)
    {
        if (CurrencyType != second.CurrencyType)
            throw new CurrencyMismatchException("Cannot divide amounts with different currencies");

        var amount = (Amount / second.Amount).TruncateDecimal(FindCurrency(second.CurrencyType).DecimalPlaces);
        return new Money(amount, CurrencyType);
    }

    public static bool IsGreaterThan(Money first, Money second)
    {
        if (first.CurrencyType != second.CurrencyType)
            throw new CurrencyMismatchException("Cannot compare amounts with different currencies");

        return first.Amount > second.Amount;
    }

    public static bool IsLessThan(Money first, Money second)
    {
        if (first.CurrencyType != second.CurrencyType)
            throw new CurrencyMismatchException("Cannot compare amounts with different currencies");

        return first.Amount < second.Amount;
    }

    public static bool IsGreaterThanOrEqualTo(Money first, Money second)
    {
        if (first.CurrencyType != second.CurrencyType)
            throw new CurrencyMismatchException("Cannot compare amounts with different currencies");

        return first.Amount >= second.Amount;
    }

    public static bool IsLessThanOrEqualTo(Money first, Money second)
    {
        if (first.CurrencyType != second.CurrencyType)
            throw new CurrencyMismatchException("Cannot compare amounts with different currencies");

        return first.Amount <= second.Amount;
    }

    public static Money operator +(Money first, Money second) =>
        first.Add(second);
    public static Money operator -(Money first, Money second) =>
        first.Subtract(second);
    public static Money operator *(Money first, Money second) =>
        first.Multiply(second);
    public static Money operator /(Money first, Money second) =>
        first.Division(second);
    public static bool operator >(Money first, Money second) =>
        IsGreaterThan(first, second);
    public static bool operator <(Money first, Money second) =>
        IsLessThan(first, second);
    public static bool operator >=(Money first, Money second) =>
        IsGreaterThanOrEqualTo(first, second);
    public static bool operator <=(Money first, Money second) =>
        IsLessThanOrEqualTo(first, second);
    #endregion


    public override string ToString() => $"{CurrencyType} {Amount}";

    public static Money Empty =>
        new Money
        {
            Amount = 0,
            CurrencyType = CurrencyType.None
        };

    public static decimal ZeroValue => 0M;
}

public class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string message) : base(message)
    {
    }
}
