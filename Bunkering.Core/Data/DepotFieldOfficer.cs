using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class DepotFieldOfficer
    {
        public int ID { get; set; }
        public int DepotStateID { get; set; }
        public int OfficerID { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
