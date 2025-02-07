using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.Currency;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CryptoGateway.DataAccess.Repositories;

public class CurrencyRepository : ICurrencyRepository, IDisposable
{
    private readonly AppDbContext _dbContext;

    public CurrencyRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public void Add(Currency entity)
    {
        _dbContext.Currency.Add(entity);
    }

    public async Task<bool> Exists(CurrencyId id) =>
        await _dbContext.Currency.FindAsync(id.Value) != null;

    public async Task<Currency> Load(CurrencyId id) =>
        await _dbContext.Currency.FirstOrDefaultAsync(x => x.CurrencyId.Equals(id));

    public async Task<Currency> Load(CurrencyType currencyType) =>
        await _dbContext.Currency.FirstOrDefaultAsync(x => x.Type.Equals(currencyType));


    public void Dispose() => _dbContext.Dispose();
}