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
[Route("v{version:apiVersion}/wallets")]
[Authorize]
public class WalletCommandsApi : MyController<WalletCommandsApi>
{
    private readonly WalletApplicationService _applicationService;
    public WalletCommandsApi(IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager, WalletApplicationService applicationService)
        : base(httpContextAccessor, userManager) => _applicationService = applicationService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WalletCommands.V1.CreateWallet request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] WalletCommands.V1.EditWallet request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    [HttpDelete, Route("{WalletId}")]
    public async Task<IActionResult> Delete([FromQuery] WalletCommands.V1.RemoveWallet request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));
}