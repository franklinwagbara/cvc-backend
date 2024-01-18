
namespace Bunkering.Core.Utils
{
	public class RemitaSplit
	{
		public string serviceTypeId { get; set; }
		public string totalAmount { get; set; }
		public string payerName { get; set; }
		public string payerEmail { get; set; }
		public string payerPhone { get; set; }
		public string orderId { get; set; }
		public List<RPartner> lineItems { get; set; }
		public string ServiceCharge { get; set; }
		public string AmountDue { get; set; }
		//public string Amount { get; set; }
		public string ReturnSuccessUrl { get; set; }
		public string ReturnFailureUrl { get; set; }
		public string ReturnBankPaymentUrl { get; set; }
		public List<int> DocumentTypes { get; set; }
		public string CategoryName { get; set; }
		public string IGRFee { get; set; }
		public List<ApplicationItem> ApplicationItems { get; set; }
		public List<CustomField> CustomFields { get; set; }
	}

	public class RPartner
	{
		public string lineItemsId { get; set; }
		public string beneficiaryName { get; set; }
		public string beneficiaryAccount { get; set; }
		public string bankCode { get; set; }
		public string beneficiaryAmount { get; set; }
		public string deductFeeFrom { get; set; }
	}

	public class ApplicationItem
	{
		public string Group { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}

	public class CustomField
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string Type { get; set; }
	}

	public class RemitaResponse
	{
		public int Id { get; set; }
		public string statusmessage { get; set; }
		public string merchantId { get; set; }
		public string status { get; set; }
		public string RRR { get; set; }
		public string Amount { get; set; }
		public string transactiontime { get; set; }
		public string orderId { get; set; }
		public string statuscode { get; set; }
	}

	public class PrePaymentResponse
	{
		public string StatusMessage { get; set; }
		public string AppId { get; set; }
		public string Status { get; set; }
		public string RRR { get; set; }
		public string Transactiontime { get; set; }
		public string TransactionId { get; set; }
	}

	public class NewRemitaResponse
	{
		public string orderId { get; set; }
		public string RRR { get; set; }
		public string status { get; set; }
		public string message { get; set; }
		public string transactiontime { get; set; }
		public string amount { get; set; }
	}

}
