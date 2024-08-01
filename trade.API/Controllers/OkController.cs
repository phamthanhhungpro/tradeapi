using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace trade.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OkController : ControllerBase
    {
        public OkController()
        {
        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "Healthy"
            });
        }
    }
}
