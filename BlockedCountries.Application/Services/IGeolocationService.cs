using BlockedCountries.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Services
{
    public interface IGeolocationService
    {
        Task<Country> GetCountryByIpAsync(string ipAddress);
    }
}
