using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class DestinationVesselDTO
    {
        public int Id { get; set; }
        public string IMONumber { get; set; }
        public string? VesselName { get; set; }
        public string? MotherVessel { get; set; }
        public string? LoadingPort { get; set; }
        public DateTime TransferDate { get; set; }
        public double TotalVolume { get; set; }
        public int VesselTypeId { get; set; }


        public List<DestinationVessel> DestinationVessels { get; set; }
    }

    public class DestinationVessel
    {
        public int Id { get; set; }
        public string IMONumber { get; set; }
        public string VesselName { get; set; }
        public int ProductId { get; set; }
        public double OfftakeVolume { get; set; }
    }
}
