using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;

public class CustodyAccountTitle : Value<CustodyAccountTitle>
{
    // Satisfy the serialization requirements
    protected CustodyAccountTitle() { }

    internal CustodyAccountTitle(string custodyAccountTitle) => Value = custodyAccountTitle;

    public static CustodyAccountTitle FromString(string custodyAccountTitle)
    {
        CheckValidity(custodyAccountTitle);
        return new CustodyAccountTitle(custodyAccountTitle);
    }

    public static implicit operator string(CustodyAccountTitle self) => self.Value;
    public string Value { get; internal set; }
    public static CustodyAccountTitle NoCustodyAccountTitle => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(CustodyAccountTitle), "CustodyAccountTitle cannot be empty");

        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(CustodyAccountTitle), "CustodyAccountTitle cannot be longer that 100 characters");
    }
}