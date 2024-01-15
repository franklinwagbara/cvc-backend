using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{

    public class CoQsDataDTO
    {
        public Coq? coq { get; set; }
        public List<CreateCoQGasTankDTO> tankList { get; set; }
        public SubmittedDocument docs { get; set; }
    }

    public class Coq
    {
        public int? Id { get; set; }
        public int? AppId { get; set; }
        public int? PlantId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? DateOfVesselArrival { get; set; }
        public DateTime? DateOfVesselUllage { get; set; }
        public DateTime? DateOfSTAfterDischarge { get; set; }
        public int? QuauntityReflectedOnBill { get; set; }
        public int? ArrivalShipFigure { get; set; }
        public int? DischargeShipFigure { get; set; }
        public int? MT_VAC { get; set; }
        public int? MT_AIR { get; set; }
        public int? GOV { get; set; }
        public int? GSV { get; set; }
        public int? DepotPrice { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Reference { get; set; }
        public string? CurrentDesk { get; set; }
        public string? FADApproved { get; set; }
        public string? NameConsignee { get; set; }
        public Application? Application { get; set; }
        public string? Plant { get; set; }
        public string? MarketerName { get; set; }
        public string? MotherVessel { get; set; }
        public string? Jetty { get; set; }
        public string? LoadingPort { get; set; }
        public Vessel? Vessel { get; set; }
        public string? NominatedSurveyor { get; set; }
        public string? ProductType { get; set; }
    }

    
    public class Vessel
    {
        public string Name { get; set; }
        public string VesselType { get; set; }
    }
}
