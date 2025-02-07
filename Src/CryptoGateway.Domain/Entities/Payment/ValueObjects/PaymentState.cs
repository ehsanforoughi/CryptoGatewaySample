namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public enum PaymentState : byte
{
    Waiting = 1,
    Confirming = 2,
    Confirmed = 3,
    Sending = 4,
    PartiallyPaid = 5,
    Finished = 6,
    Failed = 7,
    Refunded = 8,
    Expired = 9
}