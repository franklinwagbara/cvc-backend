using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string Description { get; set; }
        public string RRR { get; set; }
        public string Status { get; set; }
        public string DepotName { get; set; }
        public List<PayType> PaymentTypes { get; set; }
    }

    public class PayType
    {
        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public string CreatedDate { get; set; }
    }
}
