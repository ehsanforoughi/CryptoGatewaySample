using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Service.Implements;
using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.BoApi.Controllers.Command;

[Route("/wallets")]
public class WalletCommandsApi : Controller
{
    private readonly WalletApplicationService _applicationService;
    private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<WalletCommandsApi>();
    public WalletCommandsApi(WalletApplicationService applicationService)
        => _applicationService = applicationService;

    [HttpPut, Route("approval")]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] WalletCommands.V1.Approve request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);

    [HttpPut, Route("rejection")]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] WalletCommands.V1.Reject request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);
}