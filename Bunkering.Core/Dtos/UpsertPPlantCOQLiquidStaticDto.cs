using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;

namespace Bunkering.Core.Dtos
{
    public class UpsertProcessingPlantCOQTankReadingsDto
    {
        public string MeasurementType { get; set; }
        public double ReadingM { get; set; } = 0;
        public double Temperature { get; set; } = 0;
        public double Density { get; set; } = 0;
        public double SpecificGravityObs { get; set; } = 0;
        public double BarrelsAtTankTables { get; set; } = 0;
        public double VolumeCorrectionFactor { get; set; } = 0;
        public double WTAir { get; set; } = 0;
    }

    public class TankBeforeReadingLiquidStatic
    {
        public int TankId { get; set; }
        public UpsertProcessingPlantCOQTankReadingsDto TankReading { get; set; }
    }

    public class TankAfterReadingLiquidStatic
    {
        public int TankId { get; set; }
        public UpsertProcessingPlantCOQTankReadingsDto TankReading { get; set; }
    }

    public class ProcessingPlantCOQBatchDto
    {
        public int BatchId { get; set; }
        public List<TankBeforeReadingLiquidStatic> TankBeforeReadings { get; set; }
        public List<TankAfterReadingLiquidStatic> TankAfterReadings { get; set; }
    }

    public class UpsertPPlantCOQLiquidStaticDto
    {
        public int PlantId { get; set; } //plant here refers to processing Facility Id
        public int ProductId { get; set; }
        public string MeasurementSystem { get; set; }
        //public int? MeterTypeId { get; set; }
        public int? DipMethodId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? ConsignorName { get; set; }
        public string? Consignee { get; set; }
        public string? Terminal { get; set; }
        public string? Destination { get; set; }
        public string? ShipmentNo { get; set; }
        //public double? ShoreFigure { get; set; }
        public double? ShipFigure { get; set; }

        //public double? PrevMCubeAt15Degree { get; set; }
        public double? PrevUsBarrelsAt15Degree { get; set; }
        //public double? PrevMTVac { get; set; }
        //public double? PrevMTAir { get; set; }
        public double? PrevWTAir { get; set; }
        //public double? PrevLongTonsAir { get; set; }

        public double? DeliveredMCubeAt15Degree { get; set; }
        public double? DeliveredUsBarrelsAt15Degree { get; set; }
        public double? DeliveredMTVac { get; set; }
        public double? DeliveredMTAir { get; set; }
        public double? DeliveredLongTonsAir { get; set; }
        public double Price { get; set; } = 0;

        public List<ProcessingPlantCOQBatchDto> COQBatches { get; set; }
       
        public List<SubmitDocumentDto> SubmitDocuments { get; set; }
    }
    
}
