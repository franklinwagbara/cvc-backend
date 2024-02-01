using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQLiquidDynamic
    {
        [Key]
        public int ProcessingPlantCOQLiquidDynamicId { get; set; }
        public int ProcessingPlantCOQId { get; set; }
        public int MeterId { get; set; }
        public ICollection<ProcessingPlantCOQLiquidDynamicReading> ProcessingPlantCOQLiquidDynamicReadings { get; set; }
    }
}
