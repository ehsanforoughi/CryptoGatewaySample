using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using CryptoGateway.Service.Implements;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.PublicApi.Infrastructure.ApiKey;

namespace CryptoGateway.PublicApi.Controllers.V1.Command
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/payments")]
    [ApiController]
    public class PaymentCommandsApi : Controller
    {
        private readonly PaymentApplicationService _applicationService;
        private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<PaymentCommandsApi>();
        public PaymentCommandsApi(PaymentApplicationService applicationService) 
            => _applicationService = applicationService;

        //[MapToApiVersion("1.0")]
        [HttpPost]
        [ApiKey]
        public async Task<IActionResult> Post([FromHeader] string currentUserId, [FromBody] PaymentCommands.V1.CreatePayment request)
            => Json(await RequestHandler.HandleCommand(currentUserId, request, _applicationService.Handle, Log));
    }
}
