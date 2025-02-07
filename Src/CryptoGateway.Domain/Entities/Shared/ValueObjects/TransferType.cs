namespace CryptoGateway.Domain.Entities.Shared.ValueObjects;

public enum TransferType
{
    FiatDeposit = 1,
    FiatWithdrawal = 2,
    CryptoDeposit = 3,
    CryptoWithdrawal = 4,
}