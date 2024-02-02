using Bunkering.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class TransferDetail
    {
        public int Id { get; set; }
        public string IMONumber { get; set; }
        public string VesselName { get; set; }
        public int ProductId { get; set; }
        public double OfftakeVolume { get; set; }
        public DateTime CreatedDate { get; set; }  
        public int TransferRecordId { get; set; }
        [ForeignKey(nameof(TransferRecordId))]
        [InverseProperty(nameof(Data.TransferRecord.TransferDetails))]
        public TransferRecord TransferRecord { get; set; }

    }
}
