using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class JettyFieldOfficer
    {
        public int ID { get; set; }
        public int JettyId { get; set; }
        public string OfficerID { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(JettyId))]
        public Jetty? Jetty { get; set; }

        [ForeignKey(nameof(OfficerID))]
        public ApplicationUser? Officer { get; set; }
    }
}
