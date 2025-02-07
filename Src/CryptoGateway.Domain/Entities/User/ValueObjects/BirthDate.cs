using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class BirthDate : Value<BirthDate>
{
    protected BirthDate() { }

    internal BirthDate(DateTime? value) => Value = value;

    public static BirthDate FromDateTime(DateTime birthdate)
    {
        CheckValidity(birthdate);
        return new BirthDate(birthdate);
    }

    public static implicit operator DateTime?(BirthDate self) => self.Value;
    public DateTime? Value { get; private set; }
    public static BirthDate NoBirthDate => new();

    private static void CheckValidity(DateTime birthdate)
    {
        if (birthdate == default)
            throw new ArgumentNullException(nameof(LastName), "Birth date cannot be empty");

        if (birthdate == default)
            throw new ArgumentNullException(nameof(birthdate), "Birth date cannot be empty");
    }
}