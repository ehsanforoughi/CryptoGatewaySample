namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class CustomerId
{
    public string Value { get; internal set; }

    protected CustomerId() { }

    public CustomerId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(CustomerId), "CustomerId cannot be empty");

        Value = value;
    }

    public static CustomerId FromString(string customerId) => new(customerId);

    public static implicit operator string(CustomerId self) => self.Value;
    public static CustomerId NoCustomerId => new();
}