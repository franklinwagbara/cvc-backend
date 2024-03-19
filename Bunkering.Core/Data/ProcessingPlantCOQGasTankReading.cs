using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQGasTankReading
    {
        public int Id { get; set; }
        public int ProcessingPlantCOQGasBatchTankId { get; set; }
        public string MeasurementType { get; set; }
        public double ReadingM { get; set; } = 0;
        public double Density { get; set; } = 0;
        public double VapourFactor { get; set; }
        public double WTAir { get; set; } = 0;

        public double MCubeAt15Degree
        {
            get => (ReadingM * 1000) / Density; set { }
        }

        public double UsBarrelsAt60F
        {
            get => 6.294 * MTVac; set { }
        }

        public double MTVac
        {
            get => (Density * MCubeAt15Degree) / 1000; set { }
        }

        public double MTAir
        {
            get => MTVac * WTAir; set { }
        }

        public double LongTonsAir
        {
            get => MTAir * 0.984206; set { }
        }
    }
}
