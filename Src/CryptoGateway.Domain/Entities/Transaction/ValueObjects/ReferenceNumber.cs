using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Transaction.ValueObjects;

public class ReferenceNumber : Value<ReferenceNumber>
{
    // Satisfy the serialization requirements
    protected ReferenceNumber() { }

    internal ReferenceNumber(int referenceNumber) => Value = referenceNumber;

    public static ReferenceNumber FromInteger(int referenceNumber)
    {
        return new ReferenceNumber(referenceNumber);
    }

    public static implicit operator int(ReferenceNumber self) => self.Value;
    public int Value { get; internal set; }
    public static ReferenceNumber NoReferenceNumber => new();
}