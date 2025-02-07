using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Service.Implements;
using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.BoApi.Controllers.Command;

[Route("/payouts")]
public class PayoutCommandsApi : Controller
{
    private readonly PayoutApplicationService _applicationService;
    private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<PayoutCommandsApi>();
    public PayoutCommandsApi(PayoutApplicationService applicationService)
        => _applicationService = applicationService;

    [Route("rejection"), HttpPut]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] PayoutCommands.V1.RejectPayout request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);

    [Route("fiat/approval"), HttpPut]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] PayoutCommands.V1.ApproveFiatManualWithdrawalRequest request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);

    [Route("crypto/approval"), HttpPut]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] PayoutCommands.V1.ApproveCryptoManualWithdrawalRequest request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);
}