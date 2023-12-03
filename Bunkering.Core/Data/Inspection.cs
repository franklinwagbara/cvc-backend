using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class Inspection
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string ScheduledBy { get; set; }
        public string? NominatedStaffId { get; set; }
        public string IndicationOfSImilarFacilityWithin2km { get; set; }
        public string SiteDrainage { get; set; }
        public string SietJettyTopographicSurvey { get; set; }
        public string InspectionDocument { get; set; }
        public string Comment { get; set; }
        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; }

    }
}
