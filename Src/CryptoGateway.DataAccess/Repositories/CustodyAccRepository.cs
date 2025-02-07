using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class CustodyAccRepository : ICustodyAccRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public CustodyAccRepository(AppDbContext dbContext) 
        => _dbContext = dbContext;

    public void Add(CustodyAccount entity)
        => _dbContext.CustodyAccount.Add(entity);

    public async Task<bool> Exists(CustodyAccountId id)
        => await _dbContext.CustodyAccount.FirstOrDefaultAsync(x => x.CustodyAccountId.Equals(id)) != null;
    public async Task<bool> Exists(int userId, CustomerId customerId)
        => await _dbContext.CustodyAccount
            .FirstOrDefaultAsync(x => x.UserId.Equals(userId) &&
                                      x.CustomerId.Equals(customerId)) != null;
    public async Task<CustodyAccount?> Load(CustodyAccountId id)
        => await _dbContext
            .CustodyAccount
            .Include(x => x.ContractAccount)
            .FirstOrDefaultAsync(x => x.CustodyAccountId.Equals(id));
    public async Task<CustodyAccount?> Load(CustodyAccExternalId custodyAccExternalId)
        => await _dbContext.CustodyAccount.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.CustodyAccExternalId.Value.Equals(custodyAccExternalId.Value));
    public async Task<CustodyAccount?> Load(int userId, CustomerId customerId)
        => await _dbContext.CustodyAccount.Include(x => x.ContractAccount)
            .OrderByDescending(x => x.InsertDateMi)
            .FirstOrDefaultAsync(x => x.UserId.Equals(userId) &&
                                      x.CustomerId.Equals(customerId));
    public void Dispose() => _dbContext.Dispose();
}