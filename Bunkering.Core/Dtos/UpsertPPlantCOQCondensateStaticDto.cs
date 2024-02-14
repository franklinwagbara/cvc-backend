using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;

namespace Bunkering.Core.Dtos
{
    public class UpsertProcessingPlantCOQCondensateTankReadingsDto
    {
        public string MeasurementType { get; set; }
        public double Ullage { get; set; } = 0;
        public double TankTemp { get; set; } = 0;
        public double Tov { get; set; } = 0;
        public double Bsw { get; set; } = 0;
        public double WaterGuage { get; set; } = 0;
        public double ObsvWater { get; set; } = 0;
        public double ApiAt60 { get; set; } = 0;
        public double Vcf { get; set; } = 0;
        public double LtBblFactor { get; set; } = 0;
    }

    public class TankBeforeReadingCondensateStatic
    {
        public int TankId { get; set; }
        public UpsertProcessingPlantCOQCondensateTankReadingsDto TankReading { get; set; }
    }

    public class TankAfterReadingCondensateStatic
    {
        public int TankId { get; set; }
        public UpsertProcessingPlantCOQCondensateTankReadingsDto TankReading { get; set; }
    }

    public class ProcessingPlantCOQCondensateBatchDto
    {
        public int BatchId { get; set; }
        public List<TankBeforeReadingCondensateStatic> TankBeforeReadings { get; set; }
        public List<TankAfterReadingCondensateStatic> TankAfterReadings { get; set; }
    }

    public class UpsertPPlantCOQCondensateStaticDto
    {
        //public int PlantId { get; set; } //plant here refers to processing Facility Id
        //public int ProductId { get; set; }
        //public string MeasurementSystem { get; set; }
        ////public int? MeterTypeId { get; set; }
        //public int? DipMethodId { get; set; }
        //public DateTime? StartTime { get; set; }
        //public DateTime? EndTime { get; set; }
        //public string? ConsignorName { get; set; }
        //public string? Consignee { get; set; }
        //public string? Terminal { get; set; }
        //public string? Destination { get; set; }
        //public string? ShipmentNo { get; set; }
        ////public double? ShoreFigure { get; set; }
        //public double? ShipFigure { get; set; }

        //public double? PrevMCubeAt15Degree { get; set; }
        //public double? PrevUsBarrelsAt15Degree { get; set; }
        ////public double? PrevMTVac { get; set; }
        ////public double? PrevMTAir { get; set; }
        //public double? PrevWTAir { get; set; }
        ////public double? PrevLongTonsAir { get; set; }

        //public double? DeliveredMCubeAt15Degree { get; set; }
        //public double? DeliveredUsBarrelsAt15Degree { get; set; }
        //public double? DeliveredMTVac { get; set; }
        //public double? DeliveredMTAir { get; set; }
        //public double? DeliveredLongTonsAir { get; set; }
        //public double Price { get; set; } = 0;

        public List<ProcessingPlantCOQCondensateBatchDto> COQBatches { get; set; }
       
       
    }
    
}
