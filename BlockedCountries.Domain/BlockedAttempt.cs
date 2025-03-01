﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Domain
{
    public class BlockedAttempt
    {
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; }
        public bool BlockedStatus { get; set; }
        public string UserAgent { get; set; }
    }
}
