using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ProcessingPlantCOQ
    {
        [Key]
        public int ProcessingPlantCOQId { get; set; }
        public int PlantId { get; set; } //plant here refers to processing Facility Id
        public int ProductId { get; set; }
        public string Reference { get; set; }
        public string MeasurementSystem {  get; set; }
        public int? MeterTypeId { get; set; }
        public int? DipMethodId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set;}
        public string? ConsignorName { get; set; }
        public string? Consignee { get; set; }
        public string? Terminal { get; set; }
        public string? Destination { get; set;}
        public string? ShipmentNo { get; set; }
        public double? ShoreFigure { get; set; }
        public double? ShipFigure { get; set;}

        public double? PrevMCubeAt15Degree { get => PrevUsBarrelsAt15Degree * 6.294; set { } }
        public double? PrevUsBarrelsAt15Degree { get; set; }
        public double? PrevMTVac { get => (PrevMCubeAt15Degree * 769.79)/1000; set { } }
        public double? PrevMTAir { get => PrevMTVac * PrevWTAir; set { } }
        public double? PrevWTAir { get; set; }
        public double? PrevLongTonsAir { get => PrevMTAir * 0.984206; set { } }

        public double? LeftMCubeAt15Degree { get; set; }
        public double? LeftUsBarrelsAt15Degree { get; set; }
        public double? LeftMTVac { get; set; }
        public double? LeftMTAir { get; set; }
        public double? LeftLongTonsAir { get; set; }

        public double? TotalMCubeAt15Degree { get; set; }
        public double? TotalUsBarrelsAt15Degree { get; set; }
        public double? TotalMTVac { get; set; }
        public double? TotalMTAir { get; set; }
        public double? TotalLongTonsAir { get; set; }

        public double? DeliveredMCubeAt15Degree { get; set; }
        public double? DeliveredUsBarrelsAt15Degree { get; set; }
        public double? DeliveredMTVac { get; set; }
        public double? DeliveredMTAir { get; set; }
        public double? DeliveredLongTonsAir { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }


    }
}
