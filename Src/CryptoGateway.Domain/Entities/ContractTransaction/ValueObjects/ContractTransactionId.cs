using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ContractTransaction.ValueObjects;

public class ContractTransactionId : Value<ContractTransactionId>
{
    public int Value { get; internal set; }

    protected ContractTransactionId() { }

    public ContractTransactionId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(ContractTransactionId), "Contract transaction id cannot be empty");

        Value = value;
    }

    public static implicit operator int(ContractTransactionId self) =>
        self.Value;

    public static implicit operator ContractTransactionId(string value)
        => new ContractTransactionId(int.Parse(value));

    public override string ToString() => Value.ToString();
}