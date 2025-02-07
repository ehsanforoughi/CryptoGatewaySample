using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class PayInRepository : IPayInRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public PayInRepository(AppDbContext dbContext) 
        => _dbContext = dbContext;

    public async Task<PayIn?> Load(PayInId id)
        => await _dbContext.PayIn
            .Include(x => x.CustodyAccount.ContractAccount)
            .FirstOrDefaultAsync(x => x.PayInId.Equals(id));

    public void Add(PayIn entity)
        => _dbContext.PayIn.Add(entity);

    public async Task<bool> Exists(PayInId id)
        => await _dbContext.PayIn.FindAsync(id.Value) != null;

    public async Task<bool> Exists(TxId txId)
        => await _dbContext.PayIn.FirstOrDefaultAsync(x => x.TxId.Equals(txId)) != null;

    public void Dispose() => _dbContext.Dispose();
}