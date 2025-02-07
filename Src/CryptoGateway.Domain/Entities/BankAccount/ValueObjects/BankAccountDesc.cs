using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public class BankAccountDesc : Value<BankAccountDesc>
{
    // Satisfy the serialization requirements
    protected BankAccountDesc() { }

    internal BankAccountDesc(string bankAccountDesc) => Value = bankAccountDesc;

    public static BankAccountDesc FromString(string bankAccountDesc)
    {
        CheckValidity(bankAccountDesc);
        return new BankAccountDesc(bankAccountDesc);
    }

    public static implicit operator string(BankAccountDesc self) => self.Value;
    public string Value { get; internal set; }
    public static BankAccountDesc NoBankAccountDesc => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(BankAccountDesc), "BankAccountDesc cannot be empty");

        if (value.Length > 70)
            throw new ArgumentOutOfRangeException(nameof(BankAccountDesc), "BankAccountDesc cannot be longer that 70 characters");
    }
}