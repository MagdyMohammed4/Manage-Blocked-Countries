using BlockedCountries.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Interfaces
{
    public interface ICountryRepository
    {
        Task AddBlockedCountryAsync(string countryCode );
        Task RemoveBlockedCountryAsync(string countryCode);
        Task<List<Country>> GetBlockedCountriesAsync(int page, int pageSize, string search = null);
        Task<bool> IsCountryBlockedAsync(string countryCode);
        Task AddTemporalBlockAsync(string countryCode, int durationMinutes);
        Task RemoveExpiredTemporalBlocksAsync();
        Task<bool> IsTemporarilyBlockedAsync(string countryCode);
    }
}
