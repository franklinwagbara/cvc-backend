using Bunkering.Core.ViewModels;

namespace Bunkering.Core.Dtos
{
    public class UpsertProcessingPlantCOQCondensateDynamicReadingsDto
    {
        public int MeterId { get; set; }
        public double Temperature { get; set; } = 0;
        public double Pressure { get; set; } = 0;
        public double MeterFactor { get; set; } = 0;
        public double Ctl { get; set; } = 0;
        public double Cpl { get; set; } = 0;

        public double ApiAt60 { get; set; } = 0;
        public double Vcf { get; set; } = 0;
        public double Bsw { get; set; } = 0;
        public double GrossLtBblFactor { get; set; } = 0;

        public CondensateMeterBeforReadingDto MeterBeforReadingDto { get; set; }
        public CondensateMeterAfterReadingDto MeterAfterReadingDto { get; set; }
    }

    public class CondensateBatchReadingDto
    {
        public int BatchId { get; set; }
        public List<UpsertProcessingPlantCOQCondensateDynamicReadingsDto> MeterReadings { get; set; }
        
    }

    public class CondensateMeterAfterReadingDto
    {
        public double MReadingBbl { get; set; } = 0;
    }

    public class CondensateMeterBeforReadingDto
    {
        public double MReadingBbl { get; set; } = 0;

    }

    public class UpsertPPlantCOQCondensateDynamicDto
    {       

        public List<CondensateBatchReadingDto> COQBatches { get; set; }
    }
    
}
