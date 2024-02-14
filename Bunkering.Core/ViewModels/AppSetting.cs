using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class AppSetting
    {
        public string ElpsUrl { get; set; }
        public string AppId { get; set; }
        public string AppEmail { get; set; }
        public string PublicKey { get; set; }
        public string DepotUrl { get; set; }
        public string DepotUri { get; set; }
        public string DepotListUri { get; set; }
        public string CvlUrl { get; set; }
        public string CvlUri { get; set; }
        public string CvlKey { get; set; }
        public string BOBName { get; set; }
        public string NMDPRABName { get; set; }
        public string MDGIFBName { get; set; }
        public string BOBankCode { get; set; }
        public string NMDPRABankCode { get; set; }
        public string MDGIFBankCode { get; set; }
        public string BOAccount { get; set; }
        public string MDGIFAccount { get; set; }
        public string NMDPRAAccount { get; set; }
        public string ServiceTypeId { get; set; }
        public string RemitaBase { get; set; }
        public string LoginUrl { get; set; }
        public string SAPBaseUrl { get; set; }
    }
}
