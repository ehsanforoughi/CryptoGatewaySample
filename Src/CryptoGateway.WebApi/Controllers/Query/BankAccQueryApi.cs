using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Contracts.Query;
using CryptoGateway.Service.Models.BankAccount;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/bank-accounts")]
[Authorize(Roles = "user")]
public class BankAccQueryApi : MyController<BankAccQueryApi>
{
    private readonly DbConnection _connection;
    public BankAccQueryApi(DbConnection connection, IHttpContextAccessor httpContextAccessor, 
        UserManager<User> userManager) 
        : base(httpContextAccessor, userManager) => _connection = connection;

    [HttpGet, Route("list")]
    public async Task<IActionResult> Get([FromQuery] BankAccountQueryModels.GetBankAccounts request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }

    [HttpGet, Route("approved-list")]
    public async Task<IActionResult> Get(BankAccountQueryModels.GetApprovedList request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }

    [HttpGet, Route("bank-types")]
    public async Task<IActionResult> Get(BankAccountQueryModels.GetBankTypes request)
        => Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
}