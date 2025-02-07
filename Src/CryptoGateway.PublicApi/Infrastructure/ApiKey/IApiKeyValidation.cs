namespace CryptoGateway.PublicApi.Infrastructure.ApiKey;

public interface IApiKeyValidation
{
    Task<bool> IsValidApiKey(string userApiKey);
}