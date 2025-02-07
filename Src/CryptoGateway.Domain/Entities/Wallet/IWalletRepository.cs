using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;

namespace CryptoGateway.Domain.Entities.Wallet;

public interface IWalletRepository : IRepository<Wallet, WalletId>
{
    Task<bool> Exists(int id, WalletAddress fromString);
}