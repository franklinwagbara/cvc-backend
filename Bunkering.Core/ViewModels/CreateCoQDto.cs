using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class CreateCoQGasTankDTO
    {
        public int CoQId { get; set; }
        public string TankName { get; set; }
        public double LiquidDensityVac { get; set; }
        public double ObservedSounding { get; set; }
        public double TapeCorrection { get; set; }
        public double LiquidTemperature { get; set; }
        public double ObservedLiquidVolume { get; set; }
        public double ShrinkageFactorAir { get; set; }
        public double Vcf { get; set; }
        //public double LiquidWeightVAC { get; set; }
        //public double LiquidWeightAir { get; set; }
        public double TankVolume { get; set; }
        //public double VapourVolume { get; set; }
        public double ShrinkageFactorVapour { get; set; }
        //public double CorrectedVapourVolume { get; set; }
        public double VapourTemperature { get; set; }
        public double VapourPressure { get; set; }
        public double MolecularWeight { get; set; }
        public double VapourFactor { get; set; }
        //public double VapourWeightVAC { get; set; }
        //public double VapourWeightAir { get; set; }
        //public double TotalGasWeightVAC { get; set; }
        //public double TotalGasWeightAir { get; set; }
        
    }

    public class TankBeforeReading
    {
        public CreateCoQGasTankDTO coQGasTankDTO { get; set; }
    }

    public class TankAfterReading
    {
        public CreateCoQGasTankDTO coQGasTankDTO { get; set; }
    }

    public class CreateGasProductCoQDto
    {
        public int PlantId { get; set; }
        public int NoaAppId { get; set; }
        public double QuauntityReflectedOnBill { get; set; }
        public double ArrivalShipFigure { get; set; }
        public double DischargeShipFigure { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public DateTime DateOfVesselUllage { get; set; }
        public DateTime DateOfSTAfterDischarge { get; set; }
        public Decimal DepotPrice { get; set; }
        public List<TankBeforeReading> TankBeforeReadings { get; set; }
        public List<TankAfterReading> TankAfterReadings { get; set; }
    }
}
