using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class CoQGasTankDTO
    {
        public int CoQId { get; set; }
        public string TankName { get; set; }
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
