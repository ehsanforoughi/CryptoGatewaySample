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
[Route("v{version:apiVersion}/payouts")]
[Authorize]
public class PayoutCommandsApi : MyController<PayoutCommandsApi>
{
    private readonly PayoutApplicationService _applicationService;
    public PayoutCommandsApi(IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager, PayoutApplicationService applicationService)
        : base(httpContextAccessor, userManager) => _applicationService = applicationService;

    [Route("fiat"), HttpPost]
    public async Task<IActionResult> Post([FromBody] PayoutCommands.V1.CreateFiatRequest request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    [Route("crypto"), HttpPost]
    public async Task<IActionResult> Post([FromBody] PayoutCommands.V1.CreateCryptoRequest request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));
}