using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class PaymentId : Value<PaymentId>
{
    public int Value { get; internal set; }

    protected PaymentId() { }

    public PaymentId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(PaymentId), "Payment id cannot be empty");

        Value = value;
    }

    public static implicit operator int(PaymentId self) =>
        self.Value;

    public static implicit operator PaymentId(string value)
        => new PaymentId(int.Parse(value));

    public override string ToString() => Value.ToString();
}