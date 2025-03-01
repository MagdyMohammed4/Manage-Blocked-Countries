using BlockedCountries.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Services
{
    public class TemporalBlockBackgroundService : BackgroundService
    {
        private readonly ICountryRepository _countryRepository;

        public TemporalBlockBackgroundService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _countryRepository.RemoveExpiredTemporalBlocksAsync();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
