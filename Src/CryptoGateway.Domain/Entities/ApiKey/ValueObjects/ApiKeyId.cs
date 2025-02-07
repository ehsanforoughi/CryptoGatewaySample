using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

public class ApiKeyId : Value<ApiKeyId>
{
    public int Value { get; internal set; }

    protected ApiKeyId() { }

    public ApiKeyId(int value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(ApiKeyId), "ApiKeyId cannot be empty");

        Value = value;
    }

    public static implicit operator int(ApiKeyId self) => self.Value;

    public static ApiKeyId NoApiKeyId => new();
}