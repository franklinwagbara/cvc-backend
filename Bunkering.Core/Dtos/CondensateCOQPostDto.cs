using Bunkering.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Dtos
{
    public class CondensateCOQPostDto
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
        public double? ShipFigure { get; set; }
        public double Price { get; set; } = 0;
        public double? AverageBsw { get; set; }
        public double? ApiGravity { get; set; }
        public string? Location { get; set; }
        public UpsertPPlantCOQCondensateStaticDto Static { get; set; }
        public UpsertPPlantCOQCondensateDynamicDto Dynamic { get; set; }
        public List<SubmitDocumentDto> SubmitDocuments { get; set; }
    }
}
