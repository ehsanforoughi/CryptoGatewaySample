using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class NetworkTransferFee : Value<NetworkTransferFee>
{
    // Satisfy the serialization requirements
    protected NetworkTransferFee() { }

    internal NetworkTransferFee(decimal networkTransferFee) => Value = networkTransferFee;

    public static NetworkTransferFee FromDecimal(decimal networkTransferFee)
    {
        CheckValidity(networkTransferFee);
        return new NetworkTransferFee(networkTransferFee);
    }

    public static implicit operator decimal(NetworkTransferFee self) => self.Value;
    public decimal Value { get; internal set; }
    public static NetworkTransferFee NoNetworkTransferFee => new();
    private static void CheckValidity(decimal value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(NetworkTransferFee), "NetworkTransferFee cannot be empty");
    }
}