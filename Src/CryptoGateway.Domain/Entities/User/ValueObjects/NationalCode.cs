using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.User.ValueObjects;

public class NationalCode : Value<NationalCode>
{
    // Satisfy the serialization requirements
    protected NationalCode() { }

    internal NationalCode(string nationalCode) => Value = nationalCode;

    public static NationalCode FromLong(string nationalCode)
    {
        CheckValidity(nationalCode);
        return new NationalCode(nationalCode);
    }

    public static implicit operator string(NationalCode self) => self.Value;

    public static NationalCode NoNationalCode => new();

    public string Value { get; private set; }

    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(NationalCode), "National code cannot be empty");

        if (value.Length != 10)
            throw new ArgumentOutOfRangeException(nameof(NationalCode), "National code length should be 10 characters");

        CheckNationalCodeStructure(value);
    }

    private static void CheckNationalCodeStructure(string value)
    {
        char[] chArray = value.ToCharArray();
        int[] numArray = new int[chArray.Length];
        for (int i = 0; i < chArray.Length; i++)
        {
            numArray[i] = (int)char.GetNumericValue(chArray[i]);
        }

        int num2 = numArray[9];
        switch (value)
        {
            case "0000000000":
            case "1111111111":
            case "22222222222":
            case "33333333333":
            case "4444444444":
            case "5555555555":
            case "6666666666":
            case "7777777777":
            case "8888888888":
            case "9999999999":
                throw new InvalidDataException("National code is not valid");
                break;
        }

        int num3 =
            numArray[0] * 10 + numArray[1] * 9 + numArray[2] * 8 + numArray[3] * 7 +
                numArray[4] * 6 + numArray[5] * 5 + numArray[6] * 4 + numArray[7] * 3 + numArray[8] * 2;
        int num4 = num3 - num3 / 11 * 11;
        var isValid = num4 == 0 && num2 == num4 || num4 == 1 && num2 == 1 ||
                       num4 > 1 && num2 == Math.Abs(num4 - 11);

        if (!isValid) throw new InvalidDataException("National code is not valid");
    }
}