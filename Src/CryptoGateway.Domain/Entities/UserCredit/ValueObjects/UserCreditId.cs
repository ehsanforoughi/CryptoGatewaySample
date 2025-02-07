using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.UserCredit.ValueObjects;

public class UserCreditId : Value<UserCreditId>
{
    public int Value { get; internal set; }

    protected UserCreditId() { }

    public UserCreditId(int value)
    {
        //if (value == default)
        //    throw new ArgumentNullException(nameof(UserCreditId), "UserCreditId cannot be empty");

        Value = value;
    }

    public static implicit operator int(UserCreditId self) => self.Value;

    public static UserCreditId NoUserCreditId => new();
}