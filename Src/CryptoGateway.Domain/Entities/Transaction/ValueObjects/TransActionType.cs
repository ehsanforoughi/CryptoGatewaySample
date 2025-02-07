namespace CryptoGateway.Domain.Entities.Transaction.ValueObjects;

public enum TransActionType
{
    PayIn = 1,
    FiatPayout = 2,
    CryptoPayout = 3,
    Payment = 4,
    Commission = 5,
    ExchangeCredit = 6,
}