using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.PayIn.ValueObjects;

public class PayInId : Value<PayInId>
{
    public int Value { get; internal set; }

    protected PayInId() { }

    public PayInId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(PayInId), "PayIn id cannot be empty");

        Value = value;
    }

    public static implicit operator int(PayInId self) => self.Value;

    public static PayInId NoPayInId => new();
}