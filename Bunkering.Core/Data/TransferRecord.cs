using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class TransferRecord
    {
        public int TransferId { get; set; }
        public int IMONumber { get; set; }
        public int VessellID { get; set; }
        public string? MotherVessel { get; set; }
        public string? LoadingPort { get; set; }
        public DateTime TransferDate { get; set; }
        public string Product { get; set; }
        public double Quantity { get; set; }


        public ICollection<TransferDetail> TransferDetails { get; set; }

    }
}
