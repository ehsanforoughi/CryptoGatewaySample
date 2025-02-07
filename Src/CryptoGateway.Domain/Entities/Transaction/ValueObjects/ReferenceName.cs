using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Transaction.ValueObjects;

public class ReferenceName : Value<ReferenceName>
{
    // Satisfy the serialization requirements
    protected ReferenceName() { }

    internal ReferenceName(string referenceName) => Value = referenceName;

    public static ReferenceName FromString(string referenceName)
    {
        CheckValidity(referenceName);
        return new ReferenceName(referenceName);
    }

    public static implicit operator string(ReferenceName self) => self.Value;
    public string Value { get; internal set; }
    public static ReferenceName NoReferenceName => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(ReferenceName), "ReferenceName cannot be empty");

        if (value.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(ReferenceName), "ReferenceName cannot be longer that 30 characters");
    }
}