using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Service.Models.PayIn;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Contracts.Query;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/deposits")]
public class PayInQueryApi : MyController<PayInQueryApi>
{
    private readonly DbConnection _connection;
    public PayInQueryApi(DbConnection connection, IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager)
        : base(httpContextAccessor, userManager) => _connection = connection;

    [HttpGet, Route("list")]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] PayInQueryModels.GetPayIns request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }
}