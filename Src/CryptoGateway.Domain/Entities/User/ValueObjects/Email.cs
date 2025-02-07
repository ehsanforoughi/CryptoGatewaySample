using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class Email : Value<Email>
{
    protected Email() { }

    public Email(string value) => Value = value;

    public static Email FromString(string email)
    {
        CheckValidity(email);
        return new Email(email);
    }

    public static implicit operator string(Email self) => self.Value;
    public string Value { get; private set; }
    //public static Email NoEmail => new();

    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(Email), "Email cannot be empty");

        if (!IsValidEmail(value))
            throw new InvalidDataException("Email is not valid");
    }

    private static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
            return false; // suggested by @TK-421

        var address = new System.Net.Mail.MailAddress(email);
        return address.Address == trimmedEmail;
    }
}