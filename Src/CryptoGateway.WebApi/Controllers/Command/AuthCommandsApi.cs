using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using CryptoGateway.Service.Implements;
using CryptoGateway.Service.Contracts.Command;

namespace CryptoGateway.WebApi.Controllers.Command;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/auth")]
public class AuthCommandsApi : Controller
{
    private readonly AuthApplicationService _applicationService;
    private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<AuthCommandsApi>();
    public AuthCommandsApi(AuthApplicationService authService) => this._applicationService = authService;

    [HttpPost("registration")]
    public async Task<IActionResult> Post([FromBody] AuthCommands.V1.Registration request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthCommands.V1.Login request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] AuthCommands.V1.ForgotPassword request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] AuthCommands.V1.ResetPassword request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));

    [HttpPost("email-confirmation")]
    public async Task<IActionResult> EmailConfirmation([FromBody] AuthCommands.V1.EmailConfirmation request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));

    [HttpPost("two-step-verification")]
    public async Task<IActionResult> TwoStepVerification([FromBody] AuthCommands.V1.VerifyTwoStep request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));

    [HttpPost("external-login")]
    public async Task<IActionResult> ExternalLogin([FromBody] AuthCommands.V1.ExternalLogin request)
        => Json(await RequestHandler.HandleCommand("", request, _applicationService.Handle, Log));
}