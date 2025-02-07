using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class PaymentExternalId : Value<PaymentExternalId>
{
    public string Value { get; internal set; }

    protected PaymentExternalId() { }

    public PaymentExternalId(string value)
    {
        CheckValidity(value);
        Value = value;
    }

    public static implicit operator string(PaymentExternalId self) => self.Value;

    public static PaymentExternalId NoPaymentExternalId => new();

    private static void CheckValidity(string value)
    {
        if (value == default)
            throw new ArgumentNullException($"PaymentId cannot be empty");

        if (value.Length != 27)
            throw new ArgumentOutOfRangeException($"PaymentId should be 27 characters long");
    }
}