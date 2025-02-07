using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class CustodyAccountId : Value<CustodyAccountId>
{
    public int Value { get; internal set; }

    protected CustodyAccountId() { }

    public CustodyAccountId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(CustodyAccountId), "CustodyAccountId cannot be empty");

        Value = value;
    }

    public static implicit operator int(CustodyAccountId self) =>
        self.Value;

    public static implicit operator CustodyAccountId(string value)
        => new CustodyAccountId(int.Parse(value));

    public override string ToString() => Value.ToString();
}