using BlockedCountries.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeolocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeolocationApi:ApiKey"];
        }

        public async Task<Country> GetCountryByIpAsync(string ipAddress)
        {
            try
            {
               
                if (string.IsNullOrEmpty(_apiKey))
                {
                    throw new ApplicationException("Geolocation API key is missing or invalid.");
                }

              
                var response = await _httpClient.GetAsync($"/ipgeo?apiKey={_apiKey}&ip={ipAddress}");
                response.EnsureSuccessStatusCode(); 

                
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(content);

             
                if (result == null || result.country_code2 == null || result.country_name == null)
                {
                    throw new ApplicationException("Invalid response from the geolocation API.");
                }

                return new Country { CountryCode = result.country_code2, CountryName = result.country_name };
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch country details from the geolocation API.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Failed to parse the response from the geolocation API.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while fetching country details.", ex);
            }
        }
    }
}
