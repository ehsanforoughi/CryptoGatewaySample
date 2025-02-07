using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public class BankAccountId : Value<BankAccountId>
{
    public int Value { get; internal set; }

    protected BankAccountId() { }

    public BankAccountId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(BankAccountId), "Bank account id cannot be empty");

        Value = value;
    }

    public static implicit operator int(BankAccountId self) => self.Value;

    public static BankAccountId NoBankAccountId => new();
}