using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class OrderDescription : Value<OrderDescription>
{
    public string Value { get; internal set; }

    protected OrderDescription() { }

    internal OrderDescription(string value)
    {
        //if (string.IsNullOrWhiteSpace(value))
        //    throw new ArgumentNullException(nameof(OrderDescription), "Order description cannot be empty");

        Value = value;
    }

    public static OrderDescription FromString(string orderDescription) => new(orderDescription);

    public static implicit operator string(OrderDescription self) => self.Value;
    public static OrderDescription NoOrderDescription => new();
}