using Bunkering.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace Bunkering.Core.ViewModels
{
	public class ApplictionViewModel
	{
        public int ApplicationTypeId { get; set; }
        public int VesselTypeId { get; set; }
        public string IMONumber { get; set; }     
        public string VesselName { get; set; }
        public string LoadingPort { get; set; }

        public string MarketerName { get; set; }
        public int DeportStateId { get; set; }

        //public List<TankViewModel> TankList { get; set; }
        public  List<AppDepotViewModel> DepotList { get; set; }
        public DateTime ETA { get; set; }
    }
}
