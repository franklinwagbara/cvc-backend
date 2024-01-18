using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class vPayment
    {
        public int Id { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyName { get; set; }
        public string VesselName { get; set; }
        public string RRR { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string AppReference { get; set; }
        //public string ExtraPaymentReference { get; set; }
        public DateTime PaymentDate { get; set; }


    }
}
