using Bunkering.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace Bunkering.Core.ViewModels
{
	public class ApplictionViewModel
	{
		[Display(Name = "Application Type")]
		public int ApplicationTypeId { get; set; }
		[Display(Name = "Bunker (Facility) Name")]
		public string FacilityName { get; set; }
		public int VesselTypeId { get; set; }
        public string IMONumber { get; set; }
        public string CallSIgn { get; set; }
        public string Flag { get; set; }
        public int YearOfBuild { get; set; }
        public string PlaceOfBuild { get; set; }
        public decimal Capacity { get; set; }
		public decimal DeadWeight { get; set; }
		public string Operator { get; set; }
		public List<FacilitySourceDto> FacilitySources { get; set; }
		public List<TankViewModel> TankList { get; set; }
	}
}
