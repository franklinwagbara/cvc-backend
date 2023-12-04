using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class Tank
	{
		public int Id { get; set; }
		public int FacilityId { get; set; }
		public int ProductId { get; set; }
		public string Name { get; set; }
		public decimal Capacity { get; set; }
		[ForeignKey(nameof(FacilityId))]
		public Facility Facility { get; set; }
		[ForeignKey(nameof(ProductId))]
		public Product Product { get; set; }
	}
}
