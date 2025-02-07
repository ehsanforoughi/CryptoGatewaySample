using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.PublicApi.Infrastructure.ApiKey;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthFilter))
    {
    }
}