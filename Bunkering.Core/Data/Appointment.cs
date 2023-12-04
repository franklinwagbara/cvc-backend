using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class Appointment
	{
		public int Id { get; set; }
		public int ApplicationId { get; set; }
		public string ScheduleType { get; set; }
		public string NominatedStaffId { get; set; }
		public DateTime ScheduleDate { get; set; }
		public DateTime AppointmentDate { get; set; }
		public string ScheduledBy { get; set; }
		public string ScheduleMessage { get; set; }
		public bool IsApproved { get; set; }
		public string? ApprovedBy { get; set; }
		public string? ApprovalMessage { get; set; }
		public bool IsAccepted { get; set; }
		public string? ClientMessage { get; set; }
		public string? ContactName { get; set; }
		public string? PhoneNo { get; set; }
		public DateTime ExpiryDate { get; set; }
		[ForeignKey(nameof(ApplicationId))]
		public Application Application { get; set; }
	}
}
