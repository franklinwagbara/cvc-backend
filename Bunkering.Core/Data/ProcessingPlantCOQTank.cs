using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQTank
    {
        [Key]
        public int ProcessingPlantCOQTankId { get; set; }
        public int ProcessingPlantCOQId { get; set; }
        public int TankId { get; set; }
        public ICollection<ProcessingPlantCOQTankReading> ProcessingPlantCOQTankReadings { get; set; }
    }
}
