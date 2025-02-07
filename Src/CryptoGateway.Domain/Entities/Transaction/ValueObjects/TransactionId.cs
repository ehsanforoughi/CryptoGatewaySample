using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Transaction.ValueObjects;

public class TransactionId : Value<TransactionId>
{
    public int Value { get; internal set; }

    protected TransactionId() { }

    public TransactionId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(TransactionId), "TransactionId cannot be empty");

        Value = value;
    }

    public static implicit operator int(TransactionId self) =>
        self.Value;

    public static implicit operator TransactionId(string value)
        => new TransactionId(int.Parse(value));

    public override string ToString() => Value.ToString();
}