using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class Application
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public int DeportStateId { get; set; } = 0;
        public string UserId { get; set; }
        public int FacilityId { get; set; }
        public string Reference { get; set; }
        public string CurrentDeskId { get; set; }
        public string? FADStaffId { get; set; }
        public bool FADApproved { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeskMovementDate { get; set; }
        public DateTime? ETA {  get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public int? FlowId { get; set; }
        public int? SurveyorId { get; set; }
        public string VesselName { get; set; }
        public string? MotherVessel { get; set; }
        public string? Jetty { get; set; }
       // public string IMONumber { get; set; }
        public string LoadingPort { get; set; }
        public string MarketerName { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("FacilityId")]
        public Facility Facility { get; set; }
        [ForeignKey(nameof(ApplicationTypeId))]
        public ApplicationType ApplicationType { get; set; }
        [ForeignKey("FlowId")]
        public WorkFlow WorkFlow { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        //public virtual ICollection<SubmittedDocument> SubmittedDocuments { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<ApplicationHistory> Histories { get; set; }

        public bool IsVesselCleared { get; set; }
    }
}
