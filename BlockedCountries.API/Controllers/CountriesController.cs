using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IBlockedAttemptRepository _attemptRepository;

        public CountriesController(ICountryRepository countryRepository, IBlockedAttemptRepository attemptRepository)
        {
            _countryRepository = countryRepository;
            _attemptRepository = attemptRepository;
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string countryCode)
        {
            if (await _countryRepository.IsCountryBlockedAsync(countryCode))
                return Conflict("Country is already blocked.");

            await _countryRepository.AddBlockedCountryAsync(countryCode);
            return Ok("Country blocked Succsessfuly.");
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {
            if (!await _countryRepository.IsCountryBlockedAsync(countryCode))
                return NotFound("Country is not blocked.");

            await _countryRepository.RemoveBlockedCountryAsync(countryCode);
            return Ok("Country Deleted Succsessfuly.");
        }

        [HttpGet("blocked")]
        public async Task<IActionResult> GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = null)
        {
            var countries = await _countryRepository.GetBlockedCountriesAsync(page, pageSize, search);
            return Ok(countries);
        }

        [HttpPost("temporal-block")]
        public async Task<IActionResult> TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
        {
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            if (await _countryRepository.IsTemporarilyBlockedAsync(request.CountryCode))
                return Conflict("Country is already temporarily blocked.");

            await _countryRepository.AddTemporalBlockAsync(request.CountryCode, request.DurationMinutes);
            //return Ok("Country temporarily blocked Succsessfuly.");

            var log = new BlockedAttempt
            {
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow,
                CountryCode = request.CountryCode,
                BlockedStatus = true,
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
            };
            await _attemptRepository.LogAttemptAsync(log);

            return Ok("Country temporarily blocked successfully.");
        }
    }
}
