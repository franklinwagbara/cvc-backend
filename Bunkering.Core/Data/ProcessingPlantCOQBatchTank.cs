using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQBatchTank
    {
        [Key]
        public int ProcessingPlantCOQBatchTankId { get; set; }
        public int ProcessingPlantCOQBatchId { get; set; }
        public int TankId { get; set; }
        //public double? SumDiffMCubeAt15Degree { get; set; }
        //public double? SumDiffUsBarrelsAt15Degree { get; set; }
        //public double? SumDiffMTVac { get; set; }
        //public double? SumDiffMTAir { get; set; }
        //public double? SumDiffLongTonsAir { get; set; }
        public ICollection<ProcessingPlantCOQTankReading> ProcessingPlantCOQTankReadings { get; set; }
    }
}
