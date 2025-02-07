using CryptoGateway.Framework;
using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.DataAccess.DbContexts.Extensions;

namespace CryptoGateway.DataAccess.UnitOfWorks;

public class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public EfCoreUnitOfWork(AppDbContext dbContext)
        => _dbContext = dbContext;

    public Task Commit()
    {
        _dbContext.BasePropertiesInitializer();
        return _dbContext.SaveChangesAsync();
    }

    public Task<bool> HasChanges()
    {
        return Task.FromResult(_dbContext.ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));
    }
}

