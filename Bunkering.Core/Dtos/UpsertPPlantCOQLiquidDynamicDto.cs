using Bunkering.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Bunkering.Core.Dtos
{
    public class UpsertProcessingPlantCOQDynamicReadingsDto
    {
        public int Batch { get; set; }
        public double Temperature { get; set; } = 0;
        public double Density { get; set; } = 0;
        public double MeterFactor { get; set; } = 0;
        public double Ctl { get; set; } = 0;
        public double Cpl { get; set; } = 0;

        public double WTAir { get; set; } = 0;
    }

    public class BatchReadingDto
    {
        public int MeterId { get; set; }
        public UpsertProcessingPlantCOQDynamicReadingsDto MeterReading { get; set; }
        public List<MeterReadingDto> MeterReadingList { get; set;}
    }

    public class MeterReadingDto
    {
        public double MCube { get; set; } = 0;

    }

    public class UpsertPPlantCOQLiquidDynamicDto
    {
        public int PlantId { get; set; } //plant here refers to processing Facility Id
        public int ProductId { get; set; }
        public string MeasurementSystem { get; set; }
        public int? MeterTypeId { get; set; }
        public int? DipMethodId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? ConsignorName { get; set; }
        public string? Consignee { get; set; }
        public string? Terminal { get; set; }
        public string? Destination { get; set; }
        public string? ShipmentNo { get; set; }
        public double? ShoreFigure { get; set; }
        public double? ShipFigure { get; set; }

        public double? PrevMCubeAt15Degree { get; set; }
        public double? PrevUsBarrelsAt15Degree { get; set; }
        public double? PrevMTVac { get; set; }
        public double? PrevMTAir { get; set; }
        public double? PrevWTAir { get; set; }
        public double? PrevLongTonsAir { get; set; }

        public double? LeftMCubeAt15Degree { get; set; }
        public double? LeftUsBarrelsAt15Degree { get; set; }
        public double? LeftMTVac { get; set; }
        public double? LeftMTAir { get; set; }
        public double? LeftLongTonsAir { get; set; }

        public List<BatchReadingDto> BatchReading { get; set; }
        public List<SubmitDocumentDto> SubmitDocuments { get; set; }
    }
    
}
