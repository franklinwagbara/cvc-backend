using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class ViewApplicationsByFieldOfficerDTO
    {

        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public int DeportStateId { get; set; } = 0;
        public int DepotId { get; set; }
        public bool HasCleared { get; set; }
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
        public DateTime? ETA { get; set; }
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
        //public int FacilityId { get; set; }
        public int CompanyId { get; set; }
        public int ElpsId { get; set; }
        public int VesselTypeId { get; set; }
        public string Name { get; set; }
        public string IMONumber { get; set; }
        public string? CallSIgn { get; set; }
        public string? Flag { get; set; }
        public int? YearOfBuild { get; set; }
        public string? PlaceOfBuild { get; set; }
        public bool IsLicensed { get; set; } = false;

        public decimal? DeadWeight { get; set; }
        public decimal? Capacity { get; set; }
        public string? Operator { get; set; }
        public string? DischargeId { get; set; }
    }

    //public class Facility
    //{
       
    //}
}
