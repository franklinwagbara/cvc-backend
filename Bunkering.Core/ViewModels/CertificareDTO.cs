using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class CertificareDTO
    {
        public string PermitNo { get; set; }
        public string Vessel { get; set; }
        public DateTime ETA { get; set; }
        public string LoadPort { get; set; }
        public string Jetty { get; set; }
        public string Surveyor { get; set; }
        public string QRCode { get; set; }
        public string Signature { get; set; }
        public DateTime DateIssued { get; set; }
        public List<DepotDTO> Destinations { get; set; }
        
    }

    public class DepotDTO 
    {
        public string Name { get; set; }
        public decimal Volume { get; set; }
        public string Product { get; set; }
        public string? DischargeId { get; set; }
    }
}
