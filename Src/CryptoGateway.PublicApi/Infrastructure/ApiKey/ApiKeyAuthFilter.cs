using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using CryptoGateway.Service.Resources;
using Microsoft.AspNetCore.Mvc.Filters;
using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.PublicApi.Infrastructure.ApiKey;

public class ApiKeyAuthFilter : IAsyncAuthorizationFilter
{
    private readonly IApiKeyValidation _apiKeyValidation;
    private readonly IApiKeyRepository _apiKeyRepository;

    public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation, IApiKeyRepository apiKeyRepository)
    {
        _apiKeyValidation = apiKeyValidation;
        _apiKeyRepository = apiKeyRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string extractedApiKey = context.HttpContext.Request.Headers[Constants.ApiKeyHeaderName].ToString();

        if (string.IsNullOrWhiteSpace(extractedApiKey))
        {
            context.Result = new JsonResult(Bat.Core.Response<object>.Error(ServiceMessages.UnauthorizedResult));
            return;
        }

        if (!await _apiKeyValidation.IsValidApiKey(extractedApiKey))
        {
            context.Result = new JsonResult(Bat.Core.Response<object>.Error(ServiceMessages.UnauthorizedResult));
            return;
        }

        var apiKey = await _apiKeyRepository.Load(KeyValue.FromString(extractedApiKey));
        if (apiKey == null)
        {
            context.Result = new JsonResult(Bat.Core.Response<object>.Error(ServiceMessages.UnauthorizedResult));
            return;
        }

        var userExternalId = apiKey.User.UserExternalId.Value;

        // Add the user ID to headers
        if (!context.HttpContext.Request.Headers.Contains(new KeyValuePair<string, StringValues>("CurrentUserId", userExternalId)))
            context.HttpContext.Request.Headers.Append("CurrentUserId", userExternalId);
    }
}