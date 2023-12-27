using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class CoQ
    {
        public int Id { get; set; }
        public int AppId { get; set; }  
        public int DepotId { get; set; }
        public DateTime DateOfVesselArrival { get; set; }   
        public DateTime DateOfVesselUllage { get; set; }
        public DateTime DateOfSTAfterDischarge { get; set; }
        public Decimal MT_VAC { get; set; }
        public Decimal MT_AIR { get; set; }
        public Decimal GOV { get; set; }    
        public Decimal GSV { get; set; }
        public Decimal DepotPrice { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public string? Status { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; }
        public string? Reference { get; set; } = string.Empty;
        public string? CurrentDeskId { get; set; }
        public bool? FADApproved { get; set; }

        [ForeignKey("AppId")]
        public Application? Application { get; set; }
        [ForeignKey("DepotId")]
        public Depot? Depot { get; set; }
    }
}
