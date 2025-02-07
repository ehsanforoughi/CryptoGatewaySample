using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class ContractAccountRepository : IContractAccountRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public ContractAccountRepository(AppDbContext dbContext) 
        => _dbContext = dbContext;

    public async Task<ContractAccount?> Load(ContractAccountId id)
        => await _dbContext.ContractAccount
            .Include(x => x.ContractTransactions)
            .FirstOrDefaultAsync(x => x.ContractAccountId.Equals(id));

    public void Add(ContractAccount entity)
        => _dbContext.ContractAccount.Add(entity);

    public async Task<bool> Exists(ContractAccountId id)
        => await _dbContext.ContractAccount.AnyAsync(x => x.ContractAccountId.Equals(id));

    public async Task<bool> Exists(TxId txId)
        => await _dbContext.ContractAccount
            .Include(x => x.ContractTransactions)
            .AnyAsync(x => x.ContractTransactions.Any(y => y.TxId.Equals(txId)));

    public void Dispose() => _dbContext.Dispose();
}