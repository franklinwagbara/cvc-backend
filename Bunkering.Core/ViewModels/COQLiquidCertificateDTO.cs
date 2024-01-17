using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Bunkering.Core.ViewModels
{
    public class COQGASCertificateDTO
    {
        public string CompanyName { get; set; }
        public string MotherVessel { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public string Product { get; set; }
        public string ProductType { get; set; }
        public string Jetty { get; set; }
        public DateTime DateOfVessselUllage { get; set; }
        public string VesselName { get; set; }
        public string ReceivingTerminal { get; set; }
        public string Cosignee { get; set; }
        public double GSV {  get; set; }
        public double GOV {  get; set; }
        public double MTVAC {  get; set; }
        public double DepotPrice {  get; set; }
        public DateTime DateAfterDischarge { get; set; }

        public double TotalAfterWeightAir { get; set; }
        public double TotalBeforeWeightAir { get; set; }
        public double TotalAfterWeightVac { get; set; }
        public double TotalBeforeWeightVac { get; set; }
        public double QuantityReflectedOnBill { get; set; }
        public double ArrivalShipFigure { get; set; }
        public double DischargeShipFigure { get; set; }
        public double TotalWeightAir => TotalAfterWeightAir - TotalBeforeWeightAir;
        public double TotalWeightVAC => TotalAfterWeightVac - TotalBeforeWeightVac;
        public double TotalWeightKg => TotalWeightVAC * 1000;
        public double DebitNoteAmount => TotalWeightVAC * DepotPrice * 0.01;

        public List<CoQTanksDTO> tanks { get; set; }
       
    }

    public class COQNonGasCertificateDTO
    {
        public string CompanyName { get; set; }
        public string MotherVessel { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public string Product { get; set; }
        public string ProductType { get; set; }
        public string Jetty { get; set; }
        public DateTime DateOfVessselUllage { get; set; }
        public string VesselName { get; set; }
        public string ReceivingTerminal { get; set; }
        public string Cosignee { get; set; }
        public double GSV { get; set; }
        public double GOV { get; set; }
        public double MTVAC { get; set; }
        public double DepotPrice { get; set; }
        public DateTime DateAfterDischarge { get; set; }
        public double QuantityReflectedOnBill { get; set; }
        public double ArrivalShipFigure { get; set; }
        public double DischargeShipFigure { get; set; }
        public double DebitNoteAmount => GSV * DepotPrice * 0.01;

        public List<CoQNonGasTanksDTO> tanks { get; set; }

    }

    public class CoQTanksDTO
    {
        public string  TankName { get; set; }
        public List<CoQTankBeforeReading> BeforeTankMeasurements { get; set; }
        public List<CoQTankAfterReading> AfterTankMeasurement { get; set; }
       // public List<CoQLiquidTankAfterReading> Difference { get; set; }
    }

    public class CoqCertTank
    {
        public string MeasurementType { get; set; }
        public double DIP { get; set; }  
        public double WaterDIP { get; set; }  
        public double TOV { get; set; }  
        public double WaterVolume { get; set; }  
        public double FloatRoofCorr { get; set; }  
        public double GOV { get; set; }  
        public decimal Tempearture { get; set; }
        public double Density { get; set; } 
        //public double VCF { get; set; }  
        public double GSV { get; set; } 
        public double MTVAC { get; set; } 

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
        //public double VCF { get; set; }  

        
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

    public class CoQTankBeforeReading
    {
        public int TankId { get; set; }
        public CoqCertTank coQCertTank { get; set; }
    }
    public class CoQTankAfterReading
    {
        public int TankId { get; set; }
        public CoqCertTank coQCertTank { get; set; }
    }

    public class CoQNonGasTanksDTO
    {
        public string TankName { get; set; }
        public List<COQTankNonGasBefore> BeforeTankMeasurements { get; set; }
        public List<COQTankNonGasAfter> AfterTankMeasurement { get; set; }
        // public List<CoQLiquidTankAfterReading> Difference { get; set; }
    }

    public class CoQCertTankNonGas
    {
        public string MeasurementType { get; set; }
        public double DIP { get; set; }
        public double WaterDIP { get; set; }
        public double TOV { get; set; }
        public double Vcf { get; set; }
        public double WaterVolume { get; set; }
        public double FloatRoofCorr { get; set; }
        public double GOV { get; set; }
        public decimal Tempearture { get; set; }
        public double Density { get; set; }
        public double GSV => GOV * Vcf;
        public double MTVAC { get; set; }
    }

    public class COQTankNonGasBefore
    {
        public int TankId { get; set; }
        public CoQCertTankNonGas CoQCertTankNonGas { get; set; }
    }

    public class COQTankNonGasAfter
    {
        public int TankId { get; set; }
        public CoQCertTankNonGas CoQCertTankNonGas { get; set; }
    }
}
