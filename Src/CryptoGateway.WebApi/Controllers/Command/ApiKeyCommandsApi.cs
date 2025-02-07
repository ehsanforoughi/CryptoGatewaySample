using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Contracts.Command;

namespace CryptoGateway.WebApi.Controllers.Command;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/bank-accounts")]
[Authorize]
public class ApiKeyCommandsApi : MyController<ApiKeyCommandsApi>
{
    private readonly ApiKeyApplicationService _applicationService;
    public ApiKeyCommandsApi(IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager, ApiKeyApplicationService applicationService)
        : base(httpContextAccessor, userManager) => _applicationService = applicationService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ApiKeyCommands.V1.GenerateApiKey request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));
}