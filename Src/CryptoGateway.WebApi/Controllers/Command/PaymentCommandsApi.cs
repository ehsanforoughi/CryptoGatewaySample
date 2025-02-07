using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.Service.Contracts.Command;

namespace CryptoGateway.WebApi.Controllers.Command
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/payments")]
    [Authorize]
    public class PaymentCommandsApi : MyController<PaymentCommandsApi>
    {
        private readonly PaymentApplicationService _applicationService;
        public PaymentCommandsApi(IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager, PaymentApplicationService applicationService) 
            :base(httpContextAccessor, userManager) => _applicationService = applicationService;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentCommands.V1.CreatePayment request)
            => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));

        //[HttpPut, Route(":id/estimation")]
        //public async Task<IActionResult> Put(PaymentCommands.V1.UpdateEstimatedPayAmount request)
        //    => Json(await RequestHandler.HandleCommand(CurrentUserId, request, _applicationService.Handle, Log));
    }
}
