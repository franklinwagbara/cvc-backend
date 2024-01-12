using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class COQLiquidCertificateDTO
    {
        public string CompanyName { get; set; }
        public string MotherVessel { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public string Product { get; set; }
        public string Jetty { get; set; }
        public DateTime DateOfVessselUllage { get; set; }
        public string VesselName { get; set; }
        public string ReceivingTerminal { get; set; }
        
        public List<CoQLiquidTankBeforeReading> BeforeTankMeasurements { get; set; }
        public List<CoQLiquidTankAfterReading> AfterTankMeasurement { get; set; }
        public List<CoQLiquidTankAfterReading> Difference { get; set; }
    }

    public class CoQLiquidTank
    {
        public string MeasurementType { get; set; }
        public double DIP { get; set; } = 0;
        public double WaterDIP { get; set; } = 0;
        public double TOV { get; set; } = 0;
        public double WaterVolume { get; set; } = 0;
        public double FloatRoofCorr { get; set; } = 0;
        public double GOV { get; set; } = 0;
        public decimal Tempearture { get; set; } = 0;
        public double Density { get; set; } = 0;
        public double VCF { get; set; } = 0;
        public double GSV { get; set; } = 0;
        public double MTVAC { get; set; } = 0;
    }

    public class CoQLiquidTankBeforeReading
    {
        public int TankId { get; set; }
        public CoQLiquidTank coQLiquidTank { get; set; }
    }
    public class CoQLiquidTankAfterReading
    {
        public int TankId { get; set; }
        public CoQLiquidTank coQLiquidTank { get; set; }
    }
}
