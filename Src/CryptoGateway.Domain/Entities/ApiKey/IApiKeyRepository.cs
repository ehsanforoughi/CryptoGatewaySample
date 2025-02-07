using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.Domain.Entities.ApiKey;

public interface IApiKeyRepository : IRepository<ApiKey, ApiKeyId>
{
    Task<ApiKey?> Load(KeyValue keyValue);
    Task<bool> Exists(KeyValue keyValue);
    Task<bool> Exists(int userId);
}