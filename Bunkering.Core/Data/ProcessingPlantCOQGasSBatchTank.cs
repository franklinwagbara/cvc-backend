using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQGasSBatchTank
    {
        public int Id { get; set; }
        public int ProcessingPlantCOQGasBatchId { get; set; }
        public int TankId { get; set; }
        //public double? SumDiffMCubeAt15Degree { get; set; }
        //public double? SumDiffUsBarrelsAt15Degree { get; set; }
        //public double? SumDiffMTVac { get; set; }
        //public double? SumDiffMTAir { get; set; }
        //public double? SumDiffLongTonsAir { get; set; }
        public ICollection<ProcessingPlantCOQGasTankReading> ProcessingPlantCOQGasTankReadings { get; set; }
    }
}
