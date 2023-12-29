using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class vDebitNote
    {
        public int Id { get; set; }
        public int COQId { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicationTypeId { get; set; }
        public string DepotName { get; set; }
        public string OrderId { get; set; }
        public string RRR { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Arrears { get; set; }
        public double ServiceCharge { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
