using Bunkering.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class VesselType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<TankViewModel> Tanks { get; set; }


	}
}
