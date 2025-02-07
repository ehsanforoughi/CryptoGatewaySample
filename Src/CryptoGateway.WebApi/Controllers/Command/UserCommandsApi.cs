using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Service.Implements;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;

namespace CryptoGateway.WebApi.Controllers.Command;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/users")]
public class UserCommandsApi : MyController<UserCommandsApi>
{
    private readonly UserApplicationService _applicationService;
    public UserCommandsApi(IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager, UserApplicationService applicationService)
        : base(httpContextAccessor, userManager) => _applicationService = applicationService;

    //[HttpPost]
    //[AllowAnonymous]
    //public async Task<IActionResult> Post([FromBody] UserCommands.V1.RegisterUser request)
    //    => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    //[Route("profile"), HttpPut]
    //[ApiKey]
    //public async Task<IActionResult> Put([FromBody] UserCommands.V1.EditUserProfile request)
    //    => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    //[Route("password"), HttpPut]
    //[ApiKey]
    //public async Task<IActionResult> Put([FromBody] UserCommands.V1.EditPassword request)
    //    => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

    //[Route("email"), HttpPut]
    //[ApiKey]
    //public async Task<IActionResult> Put([FromBody] UserCommands.V1.SetMobileNumber request)
    //    => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));
}