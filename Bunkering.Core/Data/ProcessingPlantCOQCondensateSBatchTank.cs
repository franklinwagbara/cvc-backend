using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQCondensateSBatchTank
    {
        [Key]
        public int ProcessingPlantCOQCondensateSBatchTankId { get; set; }
        //public int ProcessingPlantCOQCondensateSBatchId { get; set; }
        public int TankId { get; set; }
        public double? DiffGrossUsBarrelsAtTankTemp { get; set; }
        public double? DiffGrossBarrelsAt60 { get; set; }
        public double? DiffGrossLongTons { get; set; }

        public double? DiffBswBarrelsAt60 { get; set; }
        public double? DiffBswLongTons { get; set; }

        public double? DiffNettUsBarrelsAt60 { get; set; }
        public double? DiffNettLongTons { get; set; }
        public double? DiffNettMetricTons { get; set; }
        public ICollection<ProcessingPlantCOQCondensateTankReading> ProcessingPlantCOQCondensateTankReadings { get; set; }
    }
}
