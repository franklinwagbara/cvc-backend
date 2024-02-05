using Bunkering.Core.ViewModels;

namespace Bunkering.Core.Dtos
{
    public class UpsertProcessingPlantCOQDynamicReadingsDto
    {
        public int MeterId { get; set; }
        public double Temperature { get; set; } = 0;
        public double Density { get; set; } = 0;
        public double MeterFactor { get; set; } = 0;
        public double Ctl { get; set; } = 0;
        public double Cpl { get; set; } = 0;

        public double WTAir { get; set; } = 0;

        public MeterBeforReadingDto MeterBeforReadingDto { get; set; }
        public MeterAfterReadingDto MeterAfterReadingDto { get; set; }
    }

    public class BatchReadingDto
    {
        public int BatchId { get; set; }
        public List<UpsertProcessingPlantCOQDynamicReadingsDto> MeterReadings { get; set; }
        
    }

    public class MeterAfterReadingDto
    {
        public double MCube { get; set; } = 0;
    }

    public class MeterBeforReadingDto
    {
        public double MCube { get; set; } = 0;

    }

    public class UpsertPPlantCOQLiquidDynamicDto
    {
        public int PlantId { get; set; } //plant here refers to processing Facility Id
        public int ProductId { get; set; }
        public string MeasurementSystem { get; set; }
        public int? MeterTypeId { get; set; }
        //public int? DipMethodId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? ConsignorName { get; set; }
        public string? Consignee { get; set; }
        public string? Terminal { get; set; }
        public string? Destination { get; set; }
        public string? ShipmentNo { get; set; }
        public double? ShipFigure { get; set; }
        public double? PrevMCubeAt15Degree { get; set; }
        public double? PrevUsBarrelsAt15Degree { get; set; }
        public double? PrevMTVac { get; set; }
        public double? PrevWTAir { get; set; }
        public double? LeftMCubeAt15Degree { get; set; }
        public double? LeftUsBarrelsAt15Degree { get; set; }
        public double? LeftMTVac { get; set; }
        public double? LeftMTAir { get; set; }
        public double? LeftLongTonsAir { get; set; }
        public double Price { get; set; } = 0;

        public List<BatchReadingDto> COQBatches { get; set; }
        public List<SubmitDocumentDto> SubmitDocuments { get; set; }
    }
    
}
