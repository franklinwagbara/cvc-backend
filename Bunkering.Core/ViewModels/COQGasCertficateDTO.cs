using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class COQGasCertficateDTO
    {
        public string CompanyName { get; set; }
        //public string MotherVessel { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public string Product { get; set; }
        public string Jetty { get; set; }
        // public DateTime DateOfVessselUllage { get; set; }
        public DateTime ShoreDate { get; set; }
        public string VesselName { get; set; }
        public string ReceivingTerminal { get; set; }
        public string Consignee { get; set; }
        public double TotalAfterWeightAir { get; set; }
        public double TotalBeforeWeightAir { get; set; } 
        public double TotalAfterWeightVac { get; set; }
        public double TotalBeforeWeightVac { get; set; }
        public double QuantityReflectedOnBill { get; set; }
        public double ArrivalShipFigure { get; set; }
        public double DischargeShipFigure { get; set; }
        public List<COQGasTankDTO> tanks { get; set; }
    }

    public class COQGasTankDTO
    {
        public string TankName { get; set; }
        public List<COQGastTankBefore> BeforeMeasuremnts { get; set; }
        public List<COQGastTankAfter> AfterMeasurements { get; set; }
    }
    public class COQGastTankBefore
    {
        public int TankId { get; set; }
        public string Product { get; set; }
        public CoQGasTank coQGasTank { get; set; }
    }
    public class COQGastTankAfter
    {
        public int TankId { get; set; }
        public string Product { get; set; }

        public CoQGasTank coQGasTank { get; set; }
    }
    public class CoQGasTank
    {
        public double LiquidDensityVac { get; set; }
        public double ObservedSounding { get; set; }
        public double TapeCorrection { get; set; }
        public double LiquidTemperature { get; set; }
        public double ObservedLiquidVolume { get; set; }
        public double ShrinkageFactorLiquid { get; set; }
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
        //public double VCF { get; set; } = 0;

        public string? MeasurementType { get; set; }

        //public string? TankName { get; set; }
        //public double VapourWeightVAC { get; set; }
        //public double VapourWeightAir { get; set; }
        //public double TotalGasWeightVAC { get; set; }
        //public double TotalGasWeightAir { get; set; }
        public double LiquidDensityAir => LiquidDensityVac * 0.0011;
        public double CorrectedLiquidLevel => ObservedSounding + TapeCorrection;
        public double CorrectedLiquidVolumeM3 => ObservedLiquidVolume * ShrinkageFactorLiquid;
        public double GrossStandardVolumeGas => CorrectedLiquidVolumeM3 * Vcf;
        public double LiquidWeightVAC => LiquidDensityVac * GrossStandardVolumeGas;
        public double LiquidWeightAir => LiquidDensityAir * GrossStandardVolumeGas;
        public double VapourVolume => TankVolume - ObservedLiquidVolume;
        public double CorrectedVapourVolume => VapourVolume * ShrinkageFactorVapour;
        public double VapourWeightVAC => CorrectedVapourVolume * VapourFactor;
        public double VapourWeightAir => (LiquidDensityAir / LiquidDensityVac) * VapourWeightVAC;
        public double TotalGasWeightVAC => LiquidWeightVAC + VapourWeightVAC;
        public double TotalGasWeightAir => LiquidWeightAir + VapourWeightAir;



    }
}
