using Bat.Core;
using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.Job.Controllers
{
    [Route("[controller]/[action]")]
    public class StartController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"Welcome to DGBlocks Job ... DateTime: {PersianDateTime.Now.ToString(PersianDateTimeFormat.DateTime)}");
        }
    }
}
