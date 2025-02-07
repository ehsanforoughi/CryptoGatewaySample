using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payout.ValueObjects;

public class TransactionUrl : Value<TransactionUrl>
{
    // Satisfy the serialization requirements
    protected TransactionUrl() { }

    internal TransactionUrl(string transactionUrl) => Value = transactionUrl;

    public static TransactionUrl FromString(string transactionUrl)
    {
        CheckValidity(transactionUrl);
        return new TransactionUrl(transactionUrl);
    }

    public static implicit operator string(TransactionUrl self) => self.Value;
    public string Value { get; internal set; }
    public static TransactionUrl NoTransactionUrl => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(TransactionUrl), "TransactionUrl cannot be empty");

        if (value.Length > 255)
            throw new ArgumentOutOfRangeException(nameof(TransactionUrl), "TransactionUrl cannot be longer that 255 characters");
    }
}