using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
	public class Payment
	{
		public int Id { get; set; }
		public int ApplicationId { get; set; }
		public string OrderId { get; set; }
		public int? ExtraPaymentId { get; set; }
		public string PaymentType { get; set; }
		public DateTime TransactionDate { get; set; }
		public DateTime PaymentDate { get; set; }
		public string TransactionId { get; set; }
		public string RRR { get; set; }
		public string Description { get; set; }
		public string AppReceiptId { get; set; }
		public decimal Amount { get; set; }
		public decimal Arrears { get; set; }
		public decimal ServiceCharge { get; set; }
		public string TxnMessage { get; set; }
		public int RetryCount { get; set; }
		public DateTime LastRetryDate { get; set; }
		public string Account { get; set; }
		public string BankCode { get; set; }
		public decimal LateRenewalPenalty { get; set; }
		public decimal NonRenewalPenalty { get; set; }
		public string Status { get; set; }
		[ForeignKey("ExtraPaymentId")]
		public ExtraPayment ExtraPayment { get; set; }
		//[NotMapped]

		//[ForeignKey(nameof(ApplicationId))]
		//public Application Application { get; set; }
	}
}