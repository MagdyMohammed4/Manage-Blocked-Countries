using BlockedCountries.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Interfaces
{
    public interface IBlockedAttemptRepository
    {
        Task LogAttemptAsync(BlockedAttempt blockedAttempt);
        Task<List<BlockedAttempt>> GetBlockedAttemptsAsync(int page, int pageSize);
    }
}
