using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class LiquidDynamicMeterReading
    {
        [Key]
        public int LiquidDynamicMeterReadingId { get; set; }
        public int ProcessingPlantCOQLiquidDynamicMeterId { get; set; }
        public string MeasurementType { get; set; }
        public double MCube { get; set; } = 0;

    }
}
