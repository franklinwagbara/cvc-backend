using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class SourceRecipientVessel
    {
        public int Id { get; set; }
        public int SourceVesselId { get; set; }
        public int DestinationVesselId { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(SourceVesselId))]
        public Facility? SourceVessel { get; set; }

        [ForeignKey(nameof(DestinationVesselId))]
        public Facility? DestinationVessel { get; set; }
    }
}
