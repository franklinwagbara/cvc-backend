using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class CoQ
    {
        public int Id { get; set; }
        public int AppId { get; set; }  
        public int DepotId { get; set; }
        public int PlantId { get; set; }
        public DateTime DateOfVesselArrival { get; set; }   
        public DateTime DateOfVesselUllage { get; set; }
        public DateTime DateOfSTAfterDischarge { get; set; }
        public double QuauntityReflectedOnBill { get; set; } = 0;
        public double ArrivalShipFigure { get; set; } = 0;
        public double DischargeShipFigure { get; set; } = 0;
        //public double DifferenceBtwShipAndShoreFigure { get; set; } = 0;
        //public double PercentageDifference { get; set; } = 0;
        public double MT_VAC { get; set; } = 0;
        public double MT_AIR { get; set; } = 0;
        public double GOV { get; set; } = 0;
        public double GSV { get; set; } = 0;
        public double DepotPrice { get; set; } = 0;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; }
        public string? Reference { get; set; } = string.Empty;
        public string? CurrentDeskId { get; set; }
        public bool? FADApproved { get; set; }
        public string NameConsignee { get; set; }

        [ForeignKey("AppId")]
        public Application? Application { get; set; }
        [ForeignKey("DepotId")]
        public Depot? Depot { get; set; }
    }
}
