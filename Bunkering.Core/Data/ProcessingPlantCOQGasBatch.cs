using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQGasBatch
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public double? SumDiffMCubeAt15Degree { get; set; }
        public double? SumDiffUsBarrelsAt15Degree { get; set; }
        public double? SumDiffMTVac { get; set; }
        public double? SumDiffMTAir { get; set; }
        public double? SumDiffLongTonsAir { get; set; }
        public ICollection<ProcessingPlantCOQGasSBatchTank> ProcessingPlantCOQGasSBatchTanks { get; set; }
    }
}
