using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.BankAccount;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class BankAccountRepository : IBankAccountRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public BankAccountRepository(AppDbContext dbContext) 
        => _dbContext = dbContext;

    public async Task<BankAccount?> Load(BankAccountId id)
        => await _dbContext.BankAccount.FirstOrDefaultAsync(x => x.BankAccountId.Equals(id));

    public async Task<bool> Exists(int userId, CardNumber cardNumber)
        => await _dbContext.BankAccount.AnyAsync(x => x.UserId.Equals(userId) && x.CardNumber.Equals(cardNumber));

    public void Add(BankAccount entity) 
        => _dbContext.BankAccount.Add(entity);

    public async Task<bool> Exists(BankAccountId id)
        => await _dbContext.BankAccount.FindAsync(id.Value) != null;

    public void Dispose() => _dbContext.Dispose();
}