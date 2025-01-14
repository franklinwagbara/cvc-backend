﻿using Azure.Core.GeoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class NominatedSurveyor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public decimal NominatedVolume { get; set; } = 0;
    }
}
