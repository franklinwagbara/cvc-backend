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

		public decimal ApplicationFee { get; set; } = 0.0m;
		public decimal ProcessingFee { get; set; } = 0.0m;
        public decimal SerciveCharge { get; set; } = 0.0m;
		public decimal NOAFee { get; set; } = 0.0m;
		public decimal COQFEE { get; set; } = 0.0m;

		public bool IsDeleted { get; set; } = false;
		
	}
}
