namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public enum PaymentType : byte
{
    OrderOriented = 1,
    CustomerOriented = 2
}