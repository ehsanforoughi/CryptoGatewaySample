using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

public class PrivateKey : Value<PrivateKey>
{
    public string Value { get; internal set; }

    protected PrivateKey() { }

    public PrivateKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(PrivateKey), "PrivateKey cannot be empty");

        Value = value;
    }
    public static PrivateKey FromString(string value) => new(value);

    public static implicit operator string(PrivateKey self) => self.Value;

    public static implicit operator PrivateKey(string value) => new(value);

    public override string ToString() => Value.ToString();
}