using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class State
	{
		public int Id { get; set; }
		public int CountryId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		[ForeignKey("CountryId")]
		public Country Country { get; set; }
		public ICollection<LGA> LGAs { get; set; }

	}
}
