using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class CondensateDynamicMeterReading
    {
        [Key]
        public int CondensateDynamicMeterReadingId { get; set; }
        public int ProcessingPlantCOQCondensateDBatchMeterId { get; set; }
        public string MeasurementType { get; set; }
        public double MReadingBbl { get; set; } = 0;

    }
}
