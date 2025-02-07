using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class Salt : Value<Salt>
{
    public string Value { get; internal set; }

    protected Salt() { }

    public Salt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(Salt), "Salt cannot be empty");

        Value = value;
    }

    public static implicit operator string(Salt self) => self.Value;

    public static Salt NoSalt => new();
}