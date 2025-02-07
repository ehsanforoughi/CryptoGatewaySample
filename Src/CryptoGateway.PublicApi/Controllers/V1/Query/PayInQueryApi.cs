using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using CryptoGateway.Service.Models.PayIn;
using CryptoGateway.Service.Contracts.Query;
using CryptoGateway.PublicApi.Infrastructure.ApiKey;
using Asp.Versioning;

namespace CryptoGateway.PublicApi.Controllers.V1.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/pay-in-transactions")]
[ApiController]
public class PayInQueryApi : Controller
{
    private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<PayInQueryApi>();
    private readonly DbConnection _connection;
    public PayInQueryApi(DbConnection connection) => _connection = connection;

    //[HttpGet, Route("list")]
    [HttpGet]
    [ApiKey]
    public async Task<IActionResult> Get([FromHeader] string currentUserId, [FromQuery] PayInQueryModels.GetPayIns request)
    {
        request.UserId = currentUserId;
        return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
    }
}