using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class vAppUser
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Location { get; set; }
		public string Office { get; set; }
		public int StateId { get; set; }
		public string role { get; set; }
	}
}
