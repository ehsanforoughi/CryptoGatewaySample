using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public class Sheba : Value<Sheba>
{
    // Satisfy the serialization requirements
    protected Sheba() { }

    internal Sheba(string sheba) => Value = sheba;

    public static Sheba FromString(string sheba)
    {
        CheckValidity(sheba);
        return new Sheba(sheba);
    }

    public static implicit operator string(Sheba self) => self.Value;
    public string Value { get; internal set; }
    public static Sheba NoSheba => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(Sheba), "Sheba cannot be empty");

        if (value.Length != 24)
            throw new ArgumentOutOfRangeException(nameof(Sheba), "Sheba should be 24 characters long");
    }
}