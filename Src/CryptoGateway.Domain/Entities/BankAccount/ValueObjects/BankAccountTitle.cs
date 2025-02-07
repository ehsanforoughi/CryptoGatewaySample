using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public class BankAccountTitle : Value<BankAccountTitle>
{
    // Satisfy the serialization requirements
    protected BankAccountTitle() { }

    internal BankAccountTitle(string bankAccountTitle) => Value = bankAccountTitle;

    public static BankAccountTitle FromString(string bankAccountTitle)
    {
        CheckValidity(bankAccountTitle);
        return new BankAccountTitle(bankAccountTitle);
    }

    public static implicit operator string(BankAccountTitle self) => self.Value;
    public string Value { get; internal set; }
    public static BankAccountTitle NoBankAccountTitle => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(BankAccountTitle), "BankAccountTitle cannot be empty");

        if (value.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(BankAccountTitle), "BankAccountTitle cannot be longer that 30 characters");
    }
}