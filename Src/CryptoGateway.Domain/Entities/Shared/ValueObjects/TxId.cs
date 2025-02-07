using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class TxId : Value<TxId>
{
    // Satisfy the serialization requirements
    protected TxId() { }

    internal TxId(string txId) => Value = txId;

    public static TxId FromString(string txId)
    {
        CheckValidity(txId);
        return new TxId(txId);
    }

    public static implicit operator string(TxId self) => self.Value;
    public string Value { get; internal set; }
    public static TxId NoTxId => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(TxId), "TxId cannot be empty");

        if (value.Length > 255)
            throw new ArgumentOutOfRangeException(nameof(TxId), "TxId cannot be longer that 255 characters");
    }
}