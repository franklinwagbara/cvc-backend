using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class JettyFieldOfficerViewModel
    {
        public int JettyFieldOfficerID { get; set; }
        public int JettyID { get; set; }
        public string UserID { get; set; }
        public string? JettyName { get; set; }
        public string? OfficerName { get; set; }
    }
}
