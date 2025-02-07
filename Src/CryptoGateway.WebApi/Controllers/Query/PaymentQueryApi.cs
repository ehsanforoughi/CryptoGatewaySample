using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Models.Payment;
using CryptoGateway.Service.Contracts.Query;
using CryptoGateway.Service.Models.CustodyAccount;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/payments")]
public class PaymentQueryApi : MyController<PaymentQueryApi>
{
    private readonly DbConnection _connection;
    private readonly IConfiguration _configuration;
    public PaymentQueryApi(DbConnection connection, IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager, IConfiguration configuration)
    : base(httpContextAccessor, userManager)
    {
        _connection = connection;
        _configuration = configuration;
    }

    [HttpGet, Route("list")]
    [Authorize]
    public async Task<IActionResult> Get(PaymentQueryModels.GetOwnersPayment request)
    {
        request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(_configuration, request), Log));
    }

    //Anonymous API
    [HttpGet, Route("{PaymentId}")]
    public async Task<IActionResult> Get(PaymentQueryModels.GetPaymentLinkInfo request)
    {
        //request.UserId = CurrentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }
}