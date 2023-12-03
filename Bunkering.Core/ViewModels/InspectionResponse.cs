using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class InspectionResponse
	{
		public bool success { get; set; }
		public string message { get; set; }
		public int totalRecords { get; set; }
		public string responsecode { get; set; }
		public IEnumerable<InspectionDTO> data { get; set; }
	}

	public class InspectionDTO
	{
		public int ApplicationId { get; set; }
		public string Reference { get; set; }
		public string ApplicationType { get; set; }
		public string VesselType { get; set; }
		public string FacilityName { get; set; }
		//public string Address { get; set; }
	}
}
