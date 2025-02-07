using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class Priority : Value<Priority>
{
    // Satisfy the serialization requirements
    protected Priority() { }

    internal Priority(byte priority) => Value = priority;

    public static Priority FromByte(byte priority)
    {
        CheckValidity(priority);
        return new Priority(priority);
    }

    public static implicit operator byte(Priority self) => self.Value;
    public byte Value { get; internal set; }
    public static Priority NoPriority => new();
    private static void CheckValidity(byte value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(Priority), "Priority cannot be empty");
    }
}