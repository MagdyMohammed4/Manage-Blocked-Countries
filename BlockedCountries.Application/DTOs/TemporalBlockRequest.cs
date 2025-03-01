using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.DTOs
{
    public class TemporalBlockRequest
    {
        [Required]
        //[StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be 2 characters.")]
        public string CountryCode { get; set; }

        [Required]
        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
        public int DurationMinutes { get; set; }
    }
}
