using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class FacilitySource
	{
		public int Id { get; set; }
		public int FacilityId { get; set; }
		public int FacilityTypeId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string LicenseNumber { get; set; }
		public int LgaId { get; set; }
		[ForeignKey(nameof(FacilityId))]
		public Facility Facility { get; set; }
		[ForeignKey(nameof(LgaId))]
		public LGA LGA { get; set; }
	}
}
