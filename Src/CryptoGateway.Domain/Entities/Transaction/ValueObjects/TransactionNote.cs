using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Transaction.ValueObjects;

public class TransactionNote : Value<TransactionNote>
{
    // Satisfy the serialization requirements
    protected TransactionNote() { }

    internal TransactionNote(string transactionNote) => Value = transactionNote;

    public static TransactionNote FromString(string transactionNote)
    {
        CheckValidity(transactionNote);
        return new TransactionNote(transactionNote);
    }

    public static implicit operator string(TransactionNote self) => self.Value;
    public string Value { get; internal set; }
    public static TransactionNote NoTransactionNote => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(TransactionNote), "TransactionNote cannot be empty");

        if (value.Length > 50)
            throw new ArgumentOutOfRangeException(nameof(TransactionNote), "TransactionNote cannot be longer that 50 characters");
    }
}