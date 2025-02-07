using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.Extensions;

public static class EnumParsers
{
    public static CurrencyType ParseCurrencyType(this string currencyType)
    {
        Enum.TryParse(currencyType, true, out CurrencyType cType);
        if (!Enum.IsDefined(typeof(CurrencyType), cType) || cType == CurrencyType.None)
            throw new InvalidOperationException($"The currency type: {currencyType} is invalid");

        return cType;
    }

    public static CurrencyType ParseCurrencyType(this byte currencyTypeValue)
    {
        if (!Enum.IsDefined(typeof(CurrencyType), currencyTypeValue) || currencyTypeValue == 0)
            throw new InvalidOperationException($"The currency type value: {currencyTypeValue} is invalid");

        return (CurrencyType)currencyTypeValue;
    }

    public static string ParseCurrencyTypeStr(this byte currencyTypeValue)
    {
        if (!Enum.IsDefined(typeof(CurrencyType), currencyTypeValue) || currencyTypeValue == 0)
            throw new InvalidOperationException($"The currency type value: {currencyTypeValue} is invalid");

        return ((CurrencyType)currencyTypeValue).ToString();
    }

    public static T Str2Enum<T>(this string str) where T : struct
    {
        T res = (T)Enum.Parse(typeof(T), str, true);
        if (!Enum.IsDefined(typeof(T), res) || res.Equals(default(T)))
            throw new InvalidOperationException($"The {typeof(T)}: {str} is invalid");

        return res;
    }

    public static T Byte2Enum<T>(this byte value) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new InvalidOperationException($"{typeof(T)} is not an enum type");

        if (!Enum.IsDefined(typeof(T), value))
            throw new InvalidOperationException($"The value {value} is not defined in the {typeof(T)} enum");

        return (T)(object)value;
    }

    public static string? Byte2EnumStr<T>(this byte value) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new InvalidOperationException($"{typeof(T)} is not an enum type");

        if (!Enum.IsDefined(typeof(T), value))
            throw new InvalidOperationException($"The value {value} is not defined in the {typeof(T)} enum");

        return ((T)(object)value).ToString();
    }
}