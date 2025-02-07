using Bat.Core;
using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public class CustomerContact : Value<CustomerContact>
{
    protected CustomerContact() { }

    public CustomerContact(string value) => Value = value;

    public static CustomerContact FromString(string customerContact)
    {
        CheckValidity(customerContact);
        return new CustomerContact(customerContact);
    }

    public static implicit operator string(CustomerContact self) => self.Value;
    public string Value { get; private set; }

    private static void CheckValidity(string value)
    {
        //if (string.IsNullOrWhiteSpace(value))
        //    throw new ArgumentNullException(nameof(CustomerContact), "CustomerContact cannot be empty");

        if (value.IsEmail() && !IsValidEmailContact(value))
            throw new InvalidDataException("Email is not valid");

        if (long.TryParse(value, out _) &&
            (value.Substring(0, 1) == "0" && value.Length != 11 ||
             value.Substring(0, 1) != "0" && value.Length != 10))
            throw new ArgumentOutOfRangeException(nameof(CustomerContact),
                "MobileNumber length is invalid");
    }

    private static bool IsValidEmailContact(string customerContact)
    {
        var trimmedCustomerContact = customerContact.Trim();

        if (trimmedCustomerContact.EndsWith("."))
            return false; // suggested by @TK-421

        var address = new System.Net.Mail.MailAddress(customerContact);
        return address.Address == trimmedCustomerContact;
    }
}