using Microsoft.EntityFrameworkCore.Storage;

namespace CryptoGateway.DataAccess.DbContexts.Extensions;

public class MyDbContextTransaction : IDbContextTransaction
{
    private readonly IDbContextTransaction _transaction;
    private readonly AppDbContext _dbContext;
    public MyDbContextTransaction(IDbContextTransaction transaction, AppDbContext dbContext)
    {
        _transaction = transaction;
        _dbContext = dbContext;
    }

    public static async Task<MyDbContextTransaction> BeginTransactionAsync(AppDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        return new MyDbContextTransaction(transaction, dbContext);
    }

    public void Commit() => _transaction.Commit();

    public async Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        _dbContext.BasePropertiesInitializer();
        await _transaction.CommitAsync(cancellationToken);
    }

    public void Rollback() => _transaction.Rollback();

    public async Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken()) => await _transaction.RollbackAsync(cancellationToken);

    public Guid TransactionId => _transaction.TransactionId;

    public void Dispose() => _transaction.Dispose();

    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
}