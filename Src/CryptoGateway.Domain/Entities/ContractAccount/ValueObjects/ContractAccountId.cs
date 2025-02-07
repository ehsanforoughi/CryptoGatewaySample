using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

public class ContractAccountId : Value<ContractAccountId>
{
    public int Value { get; internal set; }

    protected ContractAccountId() { }

    public ContractAccountId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(ContractAccountId), "ContractAccountId cannot be empty");

        Value = value;
    }

    public static implicit operator int(ContractAccountId self) => self.Value;

    public static ContractAccountId NoContractAccountId => new();
}