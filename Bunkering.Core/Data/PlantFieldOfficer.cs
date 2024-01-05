using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class PlantFieldOfficer
    {
        public int ID { get; set; }
        public int PlantID { get; set; }
        public Guid OfficerID { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(PlantID))]
        public Plant? Plant { get; set; }
    }
}
