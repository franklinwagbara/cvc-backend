
namespace Bunkering.Access.DAL
{
	public class InspectionReportModel
	{
		public int ApplicationId { get; set; }
		public string Reference { get; set; }
		public string ScheduledBy { get; set; }
		public int NominatedStaffId { get; set; }
		public string Code { get; set; }
		public string IndicationOfSImilarFacilityWithin2km { get; set; }
		public string SiteDrainage { get; set; }
		public string SietJettyTopographicSurvey { get; set; }
		public string InspectionDocument { get; set; }
		public string Comment { get; set; }
	}
}
