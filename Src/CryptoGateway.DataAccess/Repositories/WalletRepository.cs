using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class WalletRepository : IWalletRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public WalletRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Wallet?> Load(WalletId id)
        => await _dbContext.Wallet.FirstOrDefaultAsync(x => x.WalletId.Equals(id));

    public void Add(Wallet entity)
        => _dbContext.Wallet.Add(entity);

    public async Task<bool> Exists(WalletId id)
        => await _dbContext.Wallet.FindAsync(id.Value) != null;

    public async Task<bool> Exists(int userId, WalletAddress walletAddress)
        => await _dbContext.Wallet.AnyAsync(x => x.UserId.Equals(userId) && x.Address.Equals(walletAddress));

    public void Dispose() => _dbContext.Dispose();
}