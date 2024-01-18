using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ApplicationHistory
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string TriggeredBy { get; set; }
        public string TriggeredByRole { get; set; }
        public string Action { get; set; }
        public string TargetedTo { get; set; }
        public string TargetRole { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
    }
}
