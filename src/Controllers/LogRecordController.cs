using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Login.Database;

namespace OpenTelemetry.Login.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogRecordController : ControllerBase
    {
        private readonly ApiContext _context;
        public LogRecordController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var logRecordsGetAll = _context.LogRecords.ToList();
            return Ok(logRecordsGetAll);
        }
    }
}
