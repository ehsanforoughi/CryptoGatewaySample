using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Wallet.ValueObjects;

public class WalletAddress : Value<WalletAddress>
{
    public string Value { get; internal set; }

    protected WalletAddress() { }
    internal WalletAddress(string walletAddress) => Value = walletAddress;

    public static WalletAddress FromString(string walletAddress)
    {
        CheckValidity(walletAddress);
        return new WalletAddress(walletAddress);
    }

    public static implicit operator string(WalletAddress self) => self.Value;

    public static implicit operator WalletAddress(string value) => new(value);

    public override string ToString() => Value.ToString();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(WalletAddress), "Wallet address cannot be empty");
    }
}