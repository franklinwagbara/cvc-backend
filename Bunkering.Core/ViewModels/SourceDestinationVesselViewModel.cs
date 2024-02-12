using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class SourceDestinationVesselViewModel
    {
        public int SourceVesselId { get; set; }
        public int DestinationVesselId { get; set; }
        public string? SourceVesselName { get; set; }
        public string? DestinationVesselName { get; set; }
    }
}
