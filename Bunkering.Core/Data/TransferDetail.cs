using Bunkering.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class TransferDetail
    {
        public int TransferDetailId { get; set; }
        public int IMONumber { get; set; }
        public int VessellID { get; set; }
        public string? LoadingPort { get; set; }
        public int ProductId { get; set; }
        public double VolumeToTransfer { get; set; }
        public DateTime CreatedDate { get; set; }      
        public TransferRecord TransferRecord { get; set; }

    }
}
