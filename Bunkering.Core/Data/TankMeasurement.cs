using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class TankMeasurement
    {
        public int Id { get; set; }
        public int COQTankId { get; set; }
        public int MeasurementTypeId { get; set; }
        public double DIP { get; set; }
        public double WaterDIP { get; set; }
        public double TOV { get; set; }
        public double WaterVolume { get; set; }
        public double FloatRoofCorr { get; set; }
        public double GOV { get; set; }
        public decimal Tempearture { get; set; }
        public double Density { get; set; }
        public double VCF { get; set; }
        public double GSV { get; set; }
        public double MTVAC { get; set; }
        public double LiquidDensityAir { get; set; }
        public double ObservedSounding { get; set; }
        public double TapeCorrection { get; set; }
        public double CorrectedLiquidVolume { get; set; }
        public double ObservedLiquidVolume { get; set; }
        public double ShrinkageFactorLiquid { get; set; }
        public double CorrectedLiquidVolumeM3 { get; set; }
        public double LiquidWeightVAC { get; set; }
        public double LiquidWeightAir { get; set; }
        public double TankVolume { get; set; }
        public double VapourVolume { get; set; }
        public double ShrinkageFactorVapour { get; set; }
        public double CorrectedVapourVolume { get; set; }
        public double VapourTemperature { get; set; }
        public double VapourPressure { get; set; }
        public double MolecularWeight { get; set; }
        public double VapourFactor { get; set; }
        public double VapourWeightVAC { get; set; }
        public double VapourWeightAir { get; set; }
        public double TotalGasWeightVAC { get; set; }
        public double TotalGasWeightAir { get; set; }
    }
}
