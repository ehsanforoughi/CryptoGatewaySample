using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public class AccountNumber : Value<AccountNumber>
{
    // Satisfy the serialization requirements
    protected AccountNumber() { }

    internal AccountNumber(string accountNumber) => Value = accountNumber;

    public static AccountNumber FromString(string accountNumber)
    {
        CheckValidity(accountNumber);
        return new AccountNumber(accountNumber);
    }

    public static implicit operator string(AccountNumber self) => self.Value;
    public string Value { get; internal set; }
    public static AccountNumber NoAccountNumber => new();
    private static void CheckValidity(string value)
    {
        //if (string.IsNullOrWhiteSpace(value))
        //    throw new ArgumentNullException(nameof(AccountNumber), "AccountNumber cannot be empty");

        if (value is { Length: 18 })
            throw new ArgumentOutOfRangeException(nameof(AccountNumber), "AccountNumber should be 18 characters long");
    }
}