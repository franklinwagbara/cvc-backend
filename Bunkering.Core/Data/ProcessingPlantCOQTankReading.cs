using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQTankReading
    {
        [Key]
        public int ProcessingPlantCOQTankId { get; set; }
        public string MeasurementType { get; set; }
        public double ReadingM {  get; set; } = 0;
        public double Temperature { get; set;} = 0;
        public double Density { get; set; } = 0;
        public double SpecificGravityObs { get; set; } = 0;
        public double BarrelsAtTankTables { get; set; } = 0;
        public double VolumeCorrectionFactor { get; set; } = 0;
        public double WTAir { get; set; } = 0;

        public double BarrelsToMCube 
        { 
            get => BarrelsAtTankTables * 0.158987; set { } 
        }

        public double MCubeAt15Degree
        {
            get => BarrelsToMCube * VolumeCorrectionFactor; set { }
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
