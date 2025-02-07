using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class OrderId : Value<OrderId>
{
    public string Value { get; internal set; }

    protected OrderId() { }

    public OrderId(string value)
    {
        //if (string.IsNullOrWhiteSpace(value))
        //    throw new ArgumentNullException(nameof(OrderId), "Order id cannot be empty");

        Value = value;
    }

    public static OrderId FromString(string orderId) => new(orderId);

    public static implicit operator string(OrderId self) => self.Value;
    public static OrderId NoOrderId => new();
}