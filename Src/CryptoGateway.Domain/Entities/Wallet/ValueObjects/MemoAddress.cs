using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Wallet.ValueObjects;

public class MemoAddress : Value<MemoAddress>
{
    // Satisfy the serialization requirements
    protected MemoAddress() { }

    internal MemoAddress(string memoAddress) => Value = memoAddress;

    public static MemoAddress FromString(string memoAddress)
    {
        CheckValidity(memoAddress);
        return new MemoAddress(memoAddress);
    }

    public static implicit operator string(MemoAddress self) => self.Value;
    public string Value { get; internal set; }
    public static MemoAddress NoMemoAddress => new();
    private static void CheckValidity(string value)
    {
        if (value is not null && string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(MemoAddress), "MemoAddress cannot be empty");

        if (value is { Length: > 30 })
            throw new ArgumentOutOfRangeException(nameof(MemoAddress), "MemoAddress cannot be longer that 30 characters");
    }
}