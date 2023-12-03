using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class AppFee
	{
		public int Id { get; set; }
		public int ApplicationTypeId { get; set; }
		public int VesseltypeId { get; set; }
		public decimal ApplicationFee { get; set; }
		public decimal AccreditationFee { get; set; }
		public decimal VesselLicenseFee { get; set; }
		public decimal AdministrativeFee { get; set; }
		public decimal InspectionFee { get; set; }
		public decimal SerciveCharge { get; set; }
		[ForeignKey(nameof(ApplicationTypeId))]
		public ApplicationType ApplicationType { get; set; }
		[ForeignKey(nameof(VesseltypeId))]
		public VesselType VesselType { get; set; }
	}
}
