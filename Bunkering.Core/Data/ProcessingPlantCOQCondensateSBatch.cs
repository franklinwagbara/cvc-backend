using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQCondensateSBatch
    {
        public int ProcessingPlantCOQCondensateSBatchId { get; set; }
        public int ProcessingPlantCOQId { get; set; }
        public int BatchId { get; set; }
        public double? SumDiffGrossUsBarrelsAtTankTemp { get; set; }
        public double? SumDiffGrossBarrelsAt60 { get; set; }
        public double? SumDiffGrossLongTons { get; set; }

        public double? SumDiffBswBarrelsAt60 { get; set; }
        public double? SumDiffBswLongTons { get; set; }

        public double? SumDiffNettUsBarrelsAt60 { get; set; }
        public double? SumDiffNettLongTons { get; set; }
        public double? SumDiffNettMetricTons { get; set; }
        public ICollection<ProcessingPlantCOQCondensateSBatchTank> ProcessingPlantCOQCondensateSBatchTanks { get; set; }
    }
}
