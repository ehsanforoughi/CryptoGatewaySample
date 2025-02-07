using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Service.Implements;
using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.BoApi.Controllers.Command;

[Route("/bank-accounts")]
public class BankAccountCommandsApi : Controller
{
    private readonly BankAccountApplicationService _applicationService;
    private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<BankAccountCommandsApi>();
    public BankAccountCommandsApi(BankAccountApplicationService applicationService)
        => _applicationService = applicationService;

    [HttpPut, Route("approval")]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] BankAccCommands.V1.Approve request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);

    [HttpPut, Route("rejection")]
    public Task<IActionResult> Put([FromHeader] string currentUserId, [FromBody] BankAccCommands.V1.Reject request)
        => RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log);
}