using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class MeterReading
    {
        [Key]
        public int MeterReadingId { get; set; }
        public int ProcessingPlantCOQLiquidDynamicReadingId { get; set; }
        public double MCube { get; set; } = 0;

    }
}
