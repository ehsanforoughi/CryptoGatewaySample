using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Service.Models.User;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Contracts.Query;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/users")]
[Authorize]
public class UserQueryApi : MyController<UserQueryApi>
{
    private readonly DbConnection _connection;
    public UserQueryApi(DbConnection connection, IHttpContextAccessor httpContextAccessor, 
        UserManager<User> userManager)
        : base(httpContextAccessor, userManager) => _connection = connection;

    [HttpGet, Route("user-credits")]
    public async Task<IActionResult> Get([FromQuery] UserQueryModels.GetUserCredits request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }
}