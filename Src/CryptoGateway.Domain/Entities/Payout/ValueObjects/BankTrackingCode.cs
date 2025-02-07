using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payout.ValueObjects;

public class BankTrackingCode : Value<BankTrackingCode>
{
    // Satisfy the serialization requirements
    protected BankTrackingCode() { }

    internal BankTrackingCode(string bankTrackingCode) => Value = bankTrackingCode;

    public static BankTrackingCode FromString(string bankTrackingCode)
    {
        CheckValidity(bankTrackingCode);
        return new BankTrackingCode(bankTrackingCode);
    }

    public static implicit operator string(BankTrackingCode self) => self.Value;
    public string Value { get; internal set; }
    public static BankTrackingCode NoBankTrackingCode => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(BankTrackingCode), "BankTrackingCode cannot be empty");

        if (value.Length > 36)
            throw new ArgumentOutOfRangeException(nameof(BankTrackingCode), "BankTrackingCode cannot be longer that 36 characters");
    }
}