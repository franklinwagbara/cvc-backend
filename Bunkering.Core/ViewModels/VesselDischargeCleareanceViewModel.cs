﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class VesselDischargeCleareanceViewModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int DepotId { get; set; }
        public string? DischargeId { get; set; }
        public string VesselName { get; set; }
        public string VesselPort { get; set; }
        public string Product { get; set; }
        public double Density { get; set; }
        public string RON { get; set; }
        public double FlashPoint { get; set; }
        public double FinalBoilingPoint { get; set; }
        public string Color { get; set; }
        public string Odour { get; set; }
        public string Oxygenate { get; set; }
        public string Others { get; set; }
        public string Comment { get; set; }
        public bool IsAllowed { get; set; }
    }
}
