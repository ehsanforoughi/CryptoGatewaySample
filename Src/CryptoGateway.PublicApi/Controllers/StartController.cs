using Bat.Core;
using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.PublicApi.Controllers
{
    [Route("[controller]/[action]")]
    public class StartController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"Welcome to DGBlocks Public API ... DateTime: {PersianDateTime.Now.ToString(PersianDateTimeFormat.DateTime)}");
        }
    }
}
