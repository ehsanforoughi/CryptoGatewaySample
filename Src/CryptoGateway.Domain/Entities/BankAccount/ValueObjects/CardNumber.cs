using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public class CardNumber : Value<CardNumber>
{
    // Satisfy the serialization requirements
    protected CardNumber() { }

    internal CardNumber(string cardNumber) => Value = cardNumber;

    public static CardNumber FromString(string cardNumber)
    {
        CheckValidity(cardNumber);
        return new CardNumber(cardNumber);
    }

    public static implicit operator string(CardNumber self) => self.Value;
    public string Value { get; internal set; }
    public static CardNumber NoCardNumber => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(CardNumber), "CardNumber cannot be empty");

        if (value.Length != 19)
            throw new ArgumentOutOfRangeException(nameof(CardNumber), "CardNumber should be 19 characters long");
    }
}