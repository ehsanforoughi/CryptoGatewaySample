using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Wallet.ValueObjects;

public class TagAddress : Value<TagAddress>
{
    // Satisfy the serialization requirements
    protected TagAddress() { }

    internal TagAddress(string tagAddress) => Value = tagAddress;

    public static TagAddress FromString(string tagAddress)
    {
        CheckValidity(tagAddress);
        return new TagAddress(tagAddress);
    }

    public static implicit operator string(TagAddress self) => self.Value;
    public string Value { get; internal set; }
    public static TagAddress NoTagAddress => new();
    private static void CheckValidity(string value)
    {
        if (value is not null && string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(TagAddress), "TagAddress cannot be empty");

        if (value is { Length: > 30 })
            throw new ArgumentOutOfRangeException(nameof(TagAddress), "TagAddress cannot be longer that 30 characters");
    }
}