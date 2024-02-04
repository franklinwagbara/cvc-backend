using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class CoQReference
    {
        public int Id { get; set; }
        public int? DepotCoQId { get; set; }
        public int? PlantCoQId { get; set; }
        public string CoQType { get; set; }
        [ForeignKey(nameof(DepotCoQId))]
        public CoQ? CoQ { get; set; }
        [ForeignKey(nameof(PlantCoQId))]
        public ProcessingPlantCOQ? ProcessingPlantCOQ { get; set; }
    }
}
