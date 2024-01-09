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
        public double LiquidDensityVac { get; set; } = 0;
        public double ObservedSounding { get; set; } = 0;
        public double TapeCorrection { get; set; } = 0;
        public double ObservedLiquidVolume { get; set; } = 0;
        public double ShrinkageFactorLiquid { get; set; } = 0;
        public double TankVolume { get; set; } = 0;
        public double ShrinkageFactorVapour { get; set; } = 0;
        public double VapourTemperature { get; set; } = 0;
        public double VapourPressure { get; set; } = 0;
        public double MolecularWeight { get; set; } = 0;
        public double VapourFactor { get; set; } = 0;
        public double LiquidDensityAir => LiquidDensityVac * 0.0011;
        public double CorrectedLiquidLevel => ObservedSounding + TapeCorrection;
        public double CorrectedLiquidVolumeM3 => ObservedLiquidVolume * ShrinkageFactorLiquid;
        public double GrossStandardVolumeGas => CorrectedLiquidVolumeM3 * VCF;
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
