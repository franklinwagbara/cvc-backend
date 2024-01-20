using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class JettyFieldOfficerViewModel
    {
        public int JettyID { get; set; }
        public Guid UserID { get; set; }
        public string? JettyName { get; set; }
        public string? OfficerName { get; set; }
    }
}
