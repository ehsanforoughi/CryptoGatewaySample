using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Wallet.ValueObjects;

public class WalletId : Value<WalletId>
{
    public int Value { get; internal set; }

    protected WalletId() { }

    public WalletId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(WalletId), "Wallet id cannot be empty");

        Value = value;
    }

    public static implicit operator int(WalletId self) => self.Value;

    public static WalletId NoWalletId => new();
}