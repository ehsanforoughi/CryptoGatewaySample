using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class ApiKeyRepository : IApiKeyRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public ApiKeyRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<ApiKey?> Load(ApiKeyId id)
        => await _dbContext.ApiKey.Include(x => x.User).FirstOrDefaultAsync(x => x.ApiKeyId.Equals(id));

    public async Task<ApiKey?> Load(KeyValue keyValue)
        => await _dbContext.ApiKey.Include(x => x.User).FirstOrDefaultAsync(x => x.KeyValue.Equals(keyValue) && !x.IsDeleted);

    public void Add(ApiKey entity) => _dbContext.ApiKey.Add(entity);

    public async Task<bool> Exists(ApiKeyId id)
        => await _dbContext.ApiKey.FindAsync(id.Value) != null;

    public async Task<bool> Exists(KeyValue keyValue)
        => await _dbContext.ApiKey.FirstOrDefaultAsync(x => x.KeyValue.Equals(keyValue) && !x.IsDeleted) != null;
    public async Task<bool> Exists(int userId)
        => await _dbContext.ApiKey.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && !x.IsDeleted) != null;
    public void Dispose() => _dbContext.Dispose();
}