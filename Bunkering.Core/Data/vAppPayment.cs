using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class vAppPayment
	{
		public int Id { get; set; }
		public int ApplicationId { get; set; }
		public string AppType { get; set; }
		public string AppReference { get; set; }
		public string Description { get; set; }
		public string OrderId { get; set; }
		public DateTime TransactionDate { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentType { get; set; }
		public string Account { get; set; }
		public decimal ServiceCharge { get; set; }
		public decimal Amount { get; set; }
		public string RRR { get; set; }
		public string TxnMessage { get; set; }
		public string PaymentStatus { get; set; }






	}
}
