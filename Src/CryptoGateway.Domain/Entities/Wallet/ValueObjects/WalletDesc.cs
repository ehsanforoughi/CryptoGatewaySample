using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Wallet.ValueObjects;

public class WalletDesc : Value<WalletDesc>
{
    // Satisfy the serialization requirements
    protected WalletDesc() { }

    internal WalletDesc(string walletDesc) => Value = walletDesc;

    public static WalletDesc FromString(string walletDesc)
    {
        CheckValidity(walletDesc);
        return new WalletDesc(walletDesc);
    }

    public static implicit operator string(WalletDesc self) => self.Value;
    public string Value { get; internal set; }
    public static WalletDesc NoWalletDesc => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(WalletDesc), "WalletDesc cannot be empty");

        if (value.Length > 70)
            throw new ArgumentOutOfRangeException(nameof(WalletDesc), "WalletDesc cannot be longer that 70 characters");
    }
}