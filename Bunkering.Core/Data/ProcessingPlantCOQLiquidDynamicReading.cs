using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQLiquidDynamicReading
    {
        [Key]
        public int ProcessingPlantCOQLiquidDynamicReadingId { get; set; }
        public int ProcessingPlantCOQLiquidDynamicId { get; set; }
        public int Batch { get; set; }
        public double Temperature { get; set;} = 0;
        public double Density { get; set; } = 0;
        public double MeterFactor { get; set; } = 0;
        public double Ctl { get; set; } = 0;
        public double Cpl { get; set; } = 0;

        public double WTAir { get; set; } = 0;
                
        public double MCubeAt15Degree
        {
            get => MeterFactor * Ctl * Cpl; set { }
        }

        public double UsBarrelsAt15Degree
        {
            get => MCubeAt15Degree * 6.294; set { }
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
