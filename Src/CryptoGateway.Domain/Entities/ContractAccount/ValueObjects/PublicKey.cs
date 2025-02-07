using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

public class PublicKey : Value<PublicKey>
{
    public string Value { get; internal set; }

    protected PublicKey() { }

    public PublicKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(PublicKey), "PublicKey cannot be empty");

        Value = value;
    }
    public static PublicKey FromString(string value) => new(value);

    public static implicit operator string(PublicKey self) => self.Value;

    public static implicit operator PublicKey(string value) => new(value);

    public override string ToString() => Value.ToString();
}