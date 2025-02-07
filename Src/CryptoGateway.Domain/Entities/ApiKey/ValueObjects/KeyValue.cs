using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

public class KeyValue : Value<KeyValue>
{
    // Satisfy the serialization requirements
    protected KeyValue() { }

    internal KeyValue(string keyValue) => Value = keyValue;

    public static KeyValue FromString(string keyValue)
    {
        CheckValidity(keyValue);
        return new KeyValue(keyValue);
    }

    public static implicit operator string(KeyValue self) => self.Value;
    public string Value { get; internal set; }
    public static KeyValue NoKeyValue => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(KeyValue), "KeyValue cannot be empty");
    }
}