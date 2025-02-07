using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.PublicApi.Infrastructure.ApiKey;

public class ApiKeyValidation : IApiKeyValidation
{
    private readonly IApiKeyRepository _repository;
    public ApiKeyValidation(IApiKeyRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsValidApiKey(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
            return false;

        if (!await _repository.Exists(KeyValue.FromString(userApiKey)))
            return false;

        return true;
    }
}