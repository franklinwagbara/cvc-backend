using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class ScheduleViewModel
    {
        public int? Id { get; set; }
        public int ApplicationId { get; set; }
        public string ScheduleType { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string? NominatedStaffId { get; set; }
        public string ScheduleMessage { get; set; }
        public string? ApprovalMessage { get; set; }
        public bool IsAccepted { get; set; }
        public string? ClientMessage { get; set; }
        public string? ContactName { get; set; }
        public string? PhoneNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Act { get; set; }
    }
}
