using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class vAppVessel
	{
		public int id { get; set; }
		public string Status { get; set; }
		public string VesselName { get; set; }
		public string Reference { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime SubmittedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
		public bool IsDeleted { get; set; }
		public string AppTypeName { get; set; }
		public string Email { get; set; }
		public string CompanyName { get; set; }
		public bool IsLicensed { get; set; }
		public string VesselTypes { get; set; }
		public int NoOfTanks { get; set; }
		public decimal Capacity { get; set; }
		public int ApplicationTypeId { get; set; }
	}
}

