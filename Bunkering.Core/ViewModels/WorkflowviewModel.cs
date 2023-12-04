﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class WorkflowviewModel
	{
		public int Id { get; set; }
		public string VesselType { get; set; }
		public string ApplicationType { get; set; }
		public string FromLocation { get; set; }
		public string ToLocation { get; set; }
		public string TriggeredByRole { get; set; }
		public string Action { get; set; }
		public string TargetRole { get; set; }
		public string Rate { get; set; }
		public string Status { get; set; }
		public bool IsArchived { get; set; }
	}
}
