using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Login.Database;
using OpenTelemetry.Logs;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace OpenTelemetry.Login.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly ILogger _logger;
        private readonly ActivitySource _activitySource = new("Tracing.NET");

        public UserController(ApiContext context, 
                              ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserRequest request)
        {
            var rawData = JsonSerializer.Serialize(new
            {
                id = request.Id,
                name = request.Name,
                email = request.Email,
            });

            using var userLogging = _logger.BeginScope(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Rawdata", rawData)
            });
            
            _logger.LogInformation("Mapeando entidade");

            var entity = new User()
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                CreatedAt = DateTime.UtcNow,
            };

            if(request.Id == null || request.Id == 0)
            {
                _logger.LogWarning("Invalid {User}", entity);
                return BadRequest("Invalid {User}");
            }
                        
            var user = _context.Users.Add(entity);
            _context.SaveChanges();

            _logger.LogInformation("Successfully create {@User}", user.Entity.Name);

            return Ok($"Usuário adicionado: {user.Entity.Name}");
        }
    }
}
