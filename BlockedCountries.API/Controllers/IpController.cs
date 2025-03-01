using BlockedCountries.Application.Interfaces;
using BlockedCountries.Application.Services;
using BlockedCountries.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeolocationService _geolocationService;
        private readonly ICountryRepository _countryRepository;
        private readonly IBlockedAttemptRepository _attemptRepository;

        public IpController(IGeolocationService geolocationService, ICountryRepository countryRepository, IBlockedAttemptRepository attemptRepository)
        {
            _geolocationService = geolocationService;
            _countryRepository = countryRepository;
            _attemptRepository = attemptRepository;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string ipAddress = null)
        {
            try
            {
                ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();

                if (!IsValidIpAddress(ipAddress))
                {
                    return BadRequest("Invalid IP address format.");
                }

                var country = await _geolocationService.GetCountryByIpAsync(ipAddress);
                return Ok(country);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, "An error occurred while fetching country details.");
            }
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckIpBlock()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (ipAddress == "::1" || ipAddress == "127.0.0.1")
            {
                ipAddress = "1.1.1.1"; 
            }

            if (!IsValidIpAddress(ipAddress))
            {
                return BadRequest("Invalid IP address format.");
            }
            var country = await _geolocationService.GetCountryByIpAsync(ipAddress);
            var isBlocked = await _countryRepository.IsCountryBlockedAsync(country.CountryCode);

            var log = new BlockedAttempt
            {
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow,
                CountryCode = country.CountryCode,
                BlockedStatus = isBlocked,
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
            };
            await _attemptRepository.LogAttemptAsync(log);

            return Ok(new { IsBlocked = isBlocked });
        }




        private bool IsValidIpAddress(string ipAddress)
        {
            
            return System.Net.IPAddress.TryParse(ipAddress, out _);
        }


    }
}
