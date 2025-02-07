using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class MobileNumber : Value<MobileNumber>
{
    // Satisfy the serialization requirements
    protected MobileNumber() { }

    internal MobileNumber(long? mobileNumber) => Value = mobileNumber;

    public static MobileNumber FromLong(long? mobileNumber)
    {
        CheckValidity(mobileNumber);
        return new MobileNumber(mobileNumber);
    }

    public static implicit operator long?(MobileNumber self) => self.Value;
    public long? Value { get; private set; }
    public static MobileNumber NoMobileNumber => new();

    private static void CheckValidity(long? value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(MobileNumber), "Mobile number cannot be empty");


        if (value.ToString().Length != 10)
            throw new ArgumentOutOfRangeException(nameof(MobileNumber),
                "MobileNumber without starting zero should be 10 numbers");
    }
}