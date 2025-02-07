using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Models.Wallet;
using CryptoGateway.Service.Contracts.Query;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/wallets")]
[Authorize]
public class WalletQueryApi : MyController<WalletQueryApi>
{
    private readonly DbConnection _connection;
    public WalletQueryApi(DbConnection connection, IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager)
        : base(httpContextAccessor, userManager) => _connection = connection;

    [HttpGet, Route("list")]
    public async Task<IActionResult> Get([FromQuery] WalletQueryModels.GetWallets request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }

    [HttpGet, Route("approved-list")]
    public async Task<IActionResult> Get(WalletQueryModels.GetApprovedList request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }
}