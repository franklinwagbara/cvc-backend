using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VesselName { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }

    }
}
