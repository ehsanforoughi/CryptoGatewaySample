using Asp.Versioning;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using CryptoGateway.Service.Models.Payment;
using CryptoGateway.Service.Contracts.Query;
using CryptoGateway.PublicApi.Infrastructure.ApiKey;

namespace CryptoGateway.PublicApi.Controllers.V1.Query
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/payments")]
    [ApiController]
    public class PaymentQueryApi : Controller
    {
        private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<PaymentQueryApi>();
        private readonly DbConnection _connection;
        private readonly IConfiguration _configuration;
        public PaymentQueryApi(DbConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        [HttpGet, Route("list")]
        [ApiKey]
        public async Task<IActionResult> Get([FromHeader] string currentUserId, [FromQuery] PaymentQueryModels.GetOwnersPayment request)
        {
            request.UserId = currentUserId;
            return Json(await RequestHandler.HandleQuery(() => _connection.Query(_configuration, request), Log));
        }

        [HttpGet, Route("{PaymentId}")]
        [ApiKey]
        //public async Task<IActionResult> Get([FromHeader] string currentUserId, [FromRoute] PaymentQueryModels.GetPaymentLinkInfo request)
        public async Task<IActionResult> Get([FromRoute] PaymentQueryModels.GetPaymentLinkInfo request)
        {
            //request.UserId = currentUserId;
            return Json(await RequestHandler.HandleQuery(() => _connection.Query(request), Log));
        }
    }
}
