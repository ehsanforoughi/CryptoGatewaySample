using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.Domain.Entities.Payout.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CryptoGateway.DataAccess.Repositories;

public class PayoutRepository : IPayoutRepository, IDisposable
{
    private readonly AppDbContext _dbContext;

    public PayoutRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public void Add(Payout entity)
        => _dbContext.Payout.Add(entity);

    public async Task<bool> Exists(PayoutId id)
        => await _dbContext.Payout.FindAsync(id.Value) != null;

    public async Task<Payout?> Load(PayoutId id)
        => await _dbContext.Payout.FirstOrDefaultAsync(x => x.PayoutId.Equals(id));

    public async Task<List<Payout>> Load(int userId, CurrencyType currencyType)
        => await _dbContext.Payout.Where(x => x.UserId.Equals(userId) && x.Value.CurrencyType.Equals(currencyType)).ToListAsync();

    public void Dispose() => _dbContext.Dispose();
}