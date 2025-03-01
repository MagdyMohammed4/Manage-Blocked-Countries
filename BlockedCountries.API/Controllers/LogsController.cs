using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptRepository _attemptRepository;

        public LogsController(IBlockedAttemptRepository attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }

        [HttpGet("blocked-attempts")]
        public async Task<IActionResult> GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = await _attemptRepository.GetBlockedAttemptsAsync(page, pageSize);
            return Ok(logs);
        }
    }
}
