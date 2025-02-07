using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

public class ContractAddress : Value<ContractAddress>
{
    public string Base58Value { get; internal set; }
    public string HexValue { get; internal set; }

    protected ContractAddress() { }
    internal ContractAddress(string base58Value, string hexValue)
    {
        Base58Value = base58Value;
        HexValue = hexValue;
    }

    public static ContractAddress FromString(string base58Value, string hexValue)
    {
        CheckValidity(base58Value, hexValue);
        return new ContractAddress(base58Value, hexValue);
    }

    private static void CheckValidity(string base58Value, string hexValue)
    {
        if (string.IsNullOrWhiteSpace(base58Value) || string.IsNullOrWhiteSpace(hexValue))
            throw new ArgumentNullException("Custody wallet addresses (base58 & hex) cannot be empty");
    }
}