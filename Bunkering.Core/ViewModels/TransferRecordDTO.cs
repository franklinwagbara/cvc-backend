using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class TransferRecordDTO
    {
        public int TransferId { get; set; }
        public int IMONumber { get; set; }
        public int VessellID { get; set; }
        public string VessellName { get; set; }
        public string? MotherVessel { get; set; }
        public string? LoadingPort { get; set; }
        public DateTime TransferDate { get; set; }
        public string Product {  get; set; }
        public double Quantity { get; set; }

        public List<TransferDetailDTO> Details { get; set; }
    }

    public class TransferDetailDTO
    {
        public int TransferDetailId { get; set; }
        public int IMONumber { get; set; }
        public int VessellID { get; set; }
        public string? LoadingPort { get; set; }
        public int ProductId { get; set; }
        public double VolumeToTransfer { get; set; }
    }
}
