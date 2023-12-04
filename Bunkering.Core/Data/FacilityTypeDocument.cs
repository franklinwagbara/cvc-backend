using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class FacilityTypeDocument
	{
		public int Id { get; set; }
		public int DocumentTypeId { get; set; }
		public int VesselTypeId { get; set; }
		public int ApplicationTypeId { get; set; }
		//public int FacilityTypeId { get; set; }
		public bool IsFADDoc { get; set; }
		public string Name { get; set; }
		public string DocType { get; set; }
		[ForeignKey("ApplicationTypeId")]
		public ApplicationType ApplicationType { get; set; }
		[ForeignKey("VesselTypeId")]
		public VesselType VesselType { get; set; }
	}
}
