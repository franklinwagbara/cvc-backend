using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class CreateCoQLiquidTankDto
    {        
        [Required]
        public double DIP { get; set; }
        [Required]
        public double WaterDIP { get; set; }
        [Required]
        public double TOV { get; set; }
        [Required]
        public double WaterVolume { get; set; }
        [Required]
        public double FloatRoofCorr { get; set; }
        public double GOV { get; set; }
        [Required]
        public decimal Tempearture { get; set; }
        [Required]
        public double Density { get; set; }
        [Required]
        public double VCF { get; set; }
        [NotMapped]
        public string? MeasurementType { get; set; }
        [NotMapped]
        public string? TankName { get; set; }
        //private double GSV  => VCF * GOV;
        //private double MTVAC => Density * GOV;
    }

    public class TankBeforeReadingLiquidProducts
    {
        public int TankId { get; set; }
        public CreateCoQLiquidTankDto coQTankDTO { get; set; }
    }

    public class TankAfterReadingLiquidProducts
    {
        public int TankId { get; set; }
        public CreateCoQLiquidTankDto coQTankDTO { get; set; }
    }

    public class CreateCoQLiquidDto
    {
        public int PlantId { get; set; }
        public int? NoaAppId { get; set; }
        public int? ProductId { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public DateTime DateOfVesselUllage { get; set; }
        public DateTime DateOfSTAfterDischarge { get; set; }
        public double DepotPrice { get; set; }
       // public string NameConsignee { get; set; }
        public List<TankBeforeReadingLiquidProducts> TankBeforeReadings { get; set; }
        public List<TankAfterReadingLiquidProducts> TankAfterReadings { get; set; }
        public List<SubmitDocumentDto> SubmitDocuments { get; set; }
    }
}
