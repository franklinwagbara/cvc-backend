﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQBatch
    {
        public int ProcessingPlantCOQBatchId { get; set; }
        public int ProcessingPlantCOQId { get; set; }
        public int BatchId { get; set; }
        public double? SumDiffMCubeAt15Degree { get; set; }
        public double? SumDiffUsBarrelsAt15Degree { get; set; }
        public double? SumDiffMTVac { get; set; }
        public double? SumDiffMTAir { get; set; }
        public double? SumDiffLongTonsAir { get; set; }
        public ICollection<ProcessingPlantCOQBatchTank> ProcessingPlantCOQBatchTanks { get; set; }
    }
}
