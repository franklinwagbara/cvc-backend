﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class ApplicationReportViewModel
	{
		public DateTime? StartDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string? Status { get; set; }
		public int? ApplicationTypeId { get; set; }
		[NotMapped]

		public DateTime Min => StartDate == null ? DateTime.UtcNow.AddHours(1).AddDays(-30) : StartDate.Value;
		[NotMapped]
		public DateTime Max => ToDate == null ? DateTime.UtcNow.AddHours(1) : ToDate.Value;
	}
}
