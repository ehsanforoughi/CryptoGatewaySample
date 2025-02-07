using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class UserExternalId : Value<UserExternalId>
{
    public string Value { get; set; }

    protected UserExternalId() { }

    public UserExternalId(string value)
    {
        CheckValidity(value);
        Value = value;
    }

    public static implicit operator string(UserExternalId self) => self.Value;

    public static UserExternalId NoUserExternalId => new();

    private static void CheckValidity(string value)
    {
        if (value == default)
            throw new ArgumentNullException($"UserId cannot be empty");

        if (value.Length != 27)
            throw new ArgumentOutOfRangeException($"UserId should be 27 characters long");
    }
}