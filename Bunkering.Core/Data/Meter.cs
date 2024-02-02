using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class Meter
	{
		public int Id { get; set; }
		public int PlantId { get; set; }
		public string Name { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
