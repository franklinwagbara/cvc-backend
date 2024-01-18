using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class vFacilityPermit
	{
		public int Id { get; set; }
		public int ApplicationId { get; set; }
		//public int PermitId { get; set; }
		public string VesselName { get; set; }
		public string VesselType { get; set; }
		public string PermitNo { get; set; }
		public DateTime IssuedDate { get; set; }
		public DateTime ExpireDate { get; set; }

	}
}
