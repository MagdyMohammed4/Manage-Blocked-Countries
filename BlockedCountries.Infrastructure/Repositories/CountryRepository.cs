using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ConcurrentDictionary<string, Country> _blockedCountries = new();
        private readonly ConcurrentDictionary<string, TemporalBlock> _temporalBlocks = new();

        public Task AddBlockedCountryAsync(string countryCode)
        {
            _blockedCountries.TryAdd(countryCode, new Country { CountryCode = countryCode });
            return Task.CompletedTask;
        }

        public Task AddTemporalBlockAsync(string countryCode, int durationMinutes)
        {
            var expiryTime = DateTime.UtcNow.AddMinutes(durationMinutes);
            _temporalBlocks.TryAdd(countryCode, new TemporalBlock { CountryCode = countryCode, ExpiryTime = expiryTime });
            return Task.CompletedTask;
        }

        public Task<List<Country>> GetBlockedCountriesAsync(int page, int pageSize, string search = null)
        {
            var query = _blockedCountries.Values.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.CountryCode.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return Task.FromResult(query.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public Task<bool> IsCountryBlockedAsync(string countryCode)
        {
            return Task.FromResult(_blockedCountries.ContainsKey(countryCode));
        }

        public Task<bool> IsTemporarilyBlockedAsync(string countryCode)
        {
            return Task.FromResult(_temporalBlocks.ContainsKey(countryCode));
        }

        public Task RemoveBlockedCountryAsync(string countryCode)
        {
            _blockedCountries.TryRemove(countryCode, out _);
            return Task.CompletedTask;
        }

        public Task RemoveExpiredTemporalBlocksAsync()
        {
            var expiredBlocks = _temporalBlocks.Where(b => b.Value.ExpiryTime <= DateTime.UtcNow).ToList();
            foreach (var block in expiredBlocks)
            {
                _temporalBlocks.TryRemove(block.Key, out _);
            }
            return Task.CompletedTask;
        }
    }
}
