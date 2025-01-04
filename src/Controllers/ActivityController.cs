using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Login.Database;

namespace OpenTelemetry.Login.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ApiContext _context;
        public ActivityController(ApiContext context) 
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var telemetryGetAll = _context.TelemetryItems.ToList();
            return Ok(telemetryGetAll);
        }
    }
}
