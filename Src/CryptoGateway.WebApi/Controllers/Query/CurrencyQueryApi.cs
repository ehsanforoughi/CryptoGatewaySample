using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.Service.Contracts.Query;

namespace CryptoGateway.WebApi.Controllers.Query;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/Currencies")]
[Authorize]
public class CurrencyQueryApi : Controller
{
    private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<CurrencyQueryApi>();
    private readonly DbConnection _connection;
    public CurrencyQueryApi(DbConnection connection) => _connection = connection;

    [HttpGet, Route("list")]
    //public Task<IActionResult> Get(CurrencyQueryModels.GetCurrencies request)
    public async Task<IActionResult> Get()
        => Json(await RequestHandler.HandleQuery(() => _connection.Query(), Log));
}