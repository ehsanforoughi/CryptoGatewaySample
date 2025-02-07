using ApiKeyGenerator;

namespace CryptoGateway.Domain.UnitTest.ApiKey;

public class ApiKeyTests
{
    [Fact]
    public async Task Generate_new_api_key()
    {
        var repository = new ApiKeyRepo
        {
            Algorithm = new ApiKeyAlgorithm()
            {
                Hash = HashAlgorithmType.BCrypt,
                SaltLength = 10,
                ClientSecretLength = 32,
                Prefix = "DiGiBlocks-",
                Suffix = "Test"
            }
        };

        var validator = new ApiKeyValidator(repository);
        var algorithm = repository.Algorithm ?? ApiKeyAlgorithm.DefaultAlgorithm;

        // Generate a key
        var persistedKey = new PersistedApiKey
        {
            KeyName = "TestKeyGeneration"
        };

        Assert.Equal(Guid.Empty, persistedKey.ApiKeyId);
        var apiKeyString = await validator.GenerateApiKey(persistedKey);
        Assert.NotEqual(string.Empty, apiKeyString);
        Assert.NotEqual(Guid.Empty, persistedKey.ApiKeyId);
    }
}