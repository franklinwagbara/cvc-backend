using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQCondensateDBatchMeter
    {
        [Key]
        public int ProcessingPlantCOQCondensateDBatchMeterId { get; set; }
        public int ProcessingPlantCOQCondensateDBatchId { get; set; }
        public int MeterId { get; set; }
        public double Temperature { get; set; } = 0;
        public double Pressure { get; set; } = 0;
        public double MeterFactor { get; set; } = 0;
        public double Ctl { get; set; } = 0;
        public double Cpl { get; set; } = 0;

        public double ApiAt60 { get; set; } = 0;
        public double Vcf { get; set; } = 0;
        public double Bsw { get; set; } = 0;
        public double GrossLtBblFactor { get; set; } = 0;
        public double GrossUsBarrelsAt60 { get => Vcf * GrossLtBblFactor; set { } }
        public double GrossLongTons { get => GrossLtBblFactor * GrossUsBarrelsAt60; set { } }

        public double BswBarrelsAt60 { get => GrossUsBarrelsAt60 * (MeterFactor / 100); set { } }
        public double BswLongTons { get => BswBarrelsAt60 * 0.15616; set { } }

        public double NettUsBarrelsAt60 { get => GrossUsBarrelsAt60 - BswBarrelsAt60; set { } }
        public double NettLongTons { get => GrossLongTons - BswLongTons; set { } }
        public double NettMetricTons { get => NettLongTons * 1.01605; set { } }
        public ICollection<CondensateDynamicMeterReading> CondensateDynamicMeterReadings { get; set; }
    }
}
