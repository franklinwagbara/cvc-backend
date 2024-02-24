using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public decimal LiquidTemperature { get; set; }

        public double LiquidDensityAir { get => LiquidDensityVac * 0.0011; set { } }
        public double CorrectedLiquidLevel { get => ObservedSounding + TapeCorrection; set { } }
        public double CorrectedLiquidVolumeM3 { get => ObservedLiquidVolume * ShrinkageFactorLiquid; set { } }
        public double GrossStandardVolumeGas { get => CorrectedLiquidVolumeM3 * VCF; set { } }
        public double LiquidWeightVAC { get => LiquidDensityVac * GrossStandardVolumeGas; set { } }
        public double LiquidWeightAir { get => LiquidDensityAir * GrossStandardVolumeGas; set { } }
        public double VapourVolume { get => TankVolume - ObservedLiquidVolume; set { } }
        public double CorrectedVapourVolume { get => VapourVolume * ShrinkageFactorVapour; set { } }
        public double VapourWeightVAC { get => CorrectedVapourVolume * VapourFactor; set { } }
        public double? VapourWeightAir { get => LiquidDensityVac == 0 ? 0 : (LiquidDensityAir / LiquidDensityVac) * VapourWeightVAC; set { } }
        public double TotalGasWeightVAC { get => LiquidWeightVAC + VapourWeightVAC; set { } }
        public double? TotalGasWeightAir { get => LiquidWeightAir + VapourWeightAir; set { } }
    }
}
