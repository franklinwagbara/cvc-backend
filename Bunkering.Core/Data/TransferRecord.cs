using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class TransferRecord
    {
      
        public int Id { get; set; }
        public string IMONumber { get; set; }
        public string VesselName { get; set; }
        public int VesselTypeId { get; set; }
        public string? MotherVessel { get; set; }
        public string? LoadingPort { get; set; }
        public DateTime TransferDate { get; set; }
        public double TotalVolume { get; set; }
        public string UserId { get; set; }


        public ICollection<TransferDetail> TransferDetails { get; set; }

    }
}
