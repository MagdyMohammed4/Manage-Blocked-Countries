using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace BlockedCountries.Infrastructure.Repositories
{
    public class BlockedAttemptRepository : IBlockedAttemptRepository
    {
        private readonly List<BlockedAttempt> _blockeds = new();
        public Task<List<BlockedAttempt>> GetBlockedAttemptsAsync(int page, int pageSize)
        {
            return Task.FromResult(_blockeds.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public Task LogAttemptAsync(BlockedAttempt blockedAttempt)
        {
            _blockeds.Add(blockedAttempt);
            return Task.CompletedTask;
        }
    }
}
