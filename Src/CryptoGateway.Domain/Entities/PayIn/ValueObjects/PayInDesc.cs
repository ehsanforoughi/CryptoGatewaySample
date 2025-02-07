using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.PayIn.ValueObjects;

public class PayInDesc : Value<PayInDesc>
{
    // Satisfy the serialization requirements
    protected PayInDesc() { }

    internal PayInDesc(string payInDesc) => Value = payInDesc;

    public static PayInDesc FromString(string payInDesc)
    {
        CheckValidity(payInDesc);
        return new PayInDesc(payInDesc);
    }

    public static implicit operator string(PayInDesc self) => self.Value;
    public string Value { get; internal set; }
    public static PayInDesc NoPayInDesc => new();
    private static void CheckValidity(string value)
    {
        if (value.Length > 70)
            throw new ArgumentOutOfRangeException(nameof(PayInDesc), "PayInDesc cannot be longer that 70 characters");
    }
}