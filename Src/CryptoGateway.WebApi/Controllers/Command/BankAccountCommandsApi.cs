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
public class BankAccountCommandsApi : MyController<BankAccountCommandsApi>
{
    private readonly BankAccountApplicationService _applicationService;
    public BankAccountCommandsApi(IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager, BankAccountApplicationService applicationService) 
        : base(httpContextAccessor, userManager) => _applicationService = applicationService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BankAccCommands.V1.CreateBankAcc request) 
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] BankAccCommands.V1.EditBankAcc request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    [HttpDelete, Route("{BankAccountId}")]
    public async Task<IActionResult> Delete([FromQuery] BankAccCommands.V1.RemoveBankAcc request)
        => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));
}