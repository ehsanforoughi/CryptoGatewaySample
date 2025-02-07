using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class ExpirationDateMi : Value<ExpirationDateMi>
{
    // Satisfy the serialization requirements
    protected ExpirationDateMi() { }

    internal ExpirationDateMi(DateTime expirationDateMi) => Value = expirationDateMi;

    public static ExpirationDateMi FromDateTime(DateTime expirationDateMi) => new(expirationDateMi);

    public static ExpirationDateMi GenerateExpirationDate()
    {
        return new ExpirationDateMi(DateTime.Now.AddMinutes(30));
    }

    public bool IsExpired() => Value < DateTime.Now;

    public static implicit operator DateTime(ExpirationDateMi self) => self.Value;
    public DateTime Value { get; internal set; }
    public static ExpirationDateMi NoExpirationDateMi => new();
}