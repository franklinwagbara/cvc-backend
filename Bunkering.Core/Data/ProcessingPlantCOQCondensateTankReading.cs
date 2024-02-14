using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQCondensateTankReading
    {
        [Key]
        public int ProcessingPlantCOQCondensateTankReadingId { get; set; }
        public int ProcessingPlantCOQCondensateSBatchTankId { get; set; }
        public string MeasurementType { get; set; }
        public double Ullage {  get; set; } = 0;
        public double TankTemp { get; set;} = 0;
        public double Tov { get; set; } = 0;
        public double Bsw { get; set; } = 0;
        public double WaterGuage { get; set; } = 0;
        public double ObsvWater { get; set; } = 0;
        public double ApiAt60 { get; set; } = 0;
        public double Vcf { get; set; } = 0;
        public double LtBblFactor { get; set; } = 0;
       
        public double GrossUsBarrelsAtTankTemp { get => Tov - ObsvWater; set { } }
        public double GrossUsBarrelsAt60 { get => Vcf * GrossUsBarrelsAtTankTemp; set { } }
        public double GrossLongTons { get => LtBblFactor * GrossUsBarrelsAt60; set { } }

        public double BswBarrelsAt60 { get => GrossUsBarrelsAt60 * (Bsw / 100); set { } }
        public double BswLongTons { get => BswBarrelsAt60 * 0.15616; set { } }

        public double NettUsBarrelsAt60 { get => GrossUsBarrelsAt60 - BswBarrelsAt60; set { } }
        public double NettLongTons { get => GrossLongTons - BswLongTons; set { } }
        public double NettMetricTons { get => NettLongTons * 1.01605; set { } }

    }
}
