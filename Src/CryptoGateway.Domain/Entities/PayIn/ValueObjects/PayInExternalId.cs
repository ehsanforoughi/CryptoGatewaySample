using CryptoGateway.Domain.Entities.Payment.ValueObjects;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.PayIn.ValueObjects;

public class PayInExternalId : Value<PayInExternalId>
{
    public string Value { get; internal set; }

    protected PayInExternalId() {}

    public PayInExternalId(string value)
    {
        CheckValidity(value);
        Value = value;
    }

    public static implicit operator string(PayInExternalId self) => self.Value;

    public static PayInExternalId NoPayInExternalId => new();

    private static void CheckValidity(string value)
    {
        if (value == default)
            throw new ArgumentNullException($"PayInId cannot be empty");

        if (value.Length != 27)
            throw new ArgumentOutOfRangeException($"PayInId should be 27 characters long");
    }
}