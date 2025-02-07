using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Models.Payout;
using CryptoGateway.Service.Contracts.Query;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/payouts")]
public class PayoutQueryApi : MyController<PayoutQueryApi>
{
    private readonly DbConnection _connection;
    public PayoutQueryApi(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, 
        DbConnection connection) 
        : base(httpContextAccessor, userManager) => _connection = connection;

    [HttpGet, Route("fiat/list")]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] PayoutQueryModels.GetFiatPayouts request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }

    [HttpGet, Route("crypto/list")]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] PayoutQueryModels.GetCryptoPayouts request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }
}