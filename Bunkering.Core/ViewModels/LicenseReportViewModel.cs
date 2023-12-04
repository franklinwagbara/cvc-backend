using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class LicenseReportViewModel
	{
		public DateTime? StartDate { get; set; }
		public DateTime? ToDate { get; set; }
		[NotMapped]
		public DateTime Min => StartDate != null ? StartDate.Value : DateTime.UtcNow.AddHours(1).AddDays(-30);
		[NotMapped]
		public DateTime Max => ToDate == null ? DateTime.UtcNow.AddHours(1) : ToDate.Value;




	}
}
