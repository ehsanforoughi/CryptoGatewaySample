using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;

public class CustodyAccExternalId : Value<CustodyAccExternalId>
{
    public string Value { get; internal set; }

    protected CustodyAccExternalId() { }

    public CustodyAccExternalId(string value)
    {
        CheckValidity(value);
        Value = value;
    }

    public static implicit operator string(CustodyAccExternalId self) => self.Value;

    public static CustodyAccExternalId NoCustodyAccExternalId => new();

    private static void CheckValidity(string value)
    {
        if (value == default)
            throw new ArgumentNullException($"CustodyAccId cannot be empty");

        if (value.Length != 27)
            throw new ArgumentOutOfRangeException($"CustodyAccId should be 27 characters long");
    }
}