using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class LicenseValidationViewModel
	{
		public string Company_Name { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string License_Number { get; set; }
		public DateTime Date_Issued { get; set; }
		public DateTime Date_Expired { get; set; }
		public string Facility_Name { get; set; }
		public ICollection<TankModel> Tanks { get; set; }
		public string StateName { get; set; }
		public string City { get; set; }
	}

	public class TankModel
	{
		public string Name { get; set; }
		public string Capacity { get; set; }
		public string Product { get; set; }

	}
}
