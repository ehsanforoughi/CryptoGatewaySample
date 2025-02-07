using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Wallet.ValueObjects;

public class WalletTitle : Value<WalletTitle>
{
    // Satisfy the serialization requirements
    protected WalletTitle() { }

    internal WalletTitle(string walletTitle) => Value = walletTitle;

    public static WalletTitle FromString(string walletTitle)
    {
        CheckValidity(walletTitle);
        return new WalletTitle(walletTitle);
    }

    public static implicit operator string(WalletTitle self) => self.Value;
    public string Value { get; internal set; }
    public static WalletTitle NoWalletTitle => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(WalletTitle), "WalletTitle cannot be empty");

        if (value.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(WalletTitle), "WalletTitle cannot be longer that 30 characters");
    }
}