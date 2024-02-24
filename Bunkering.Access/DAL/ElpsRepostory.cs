using System.Data;
using System.Text;
using Bunkering.Core.Data;
using Microsoft.EntityFrameworkCore;
using Bunkering.Core.ViewModels;
using Bunkering.Access.IContracts;
using Bunkering.Core.Utils;
using Microsoft.Extensions.Options;

namespace Bunkering.Access.DAL
{
	public class ElpsRepostory : IElps
	{
		private readonly ApplicationContext _context;
		public IUnitOfWork _unitOfWork;
		private readonly AppSetting _appSetting;

		public ElpsRepostory(
			ApplicationContext context,
			IUnitOfWork unitOfWork,
			IOptions<AppSetting> appSetting)
		{
			_context = context;
			_unitOfWork = unitOfWork;
			_appSetting = appSetting.Value;
		}

		public Dictionary<string, string> GetCompanyDetailByEmail(string email)
		{
			var dic = new Dictionary<string, string>();
			try
			{
				var content = CallElps($"/api/Company/{email}/", HttpMethod.Get);

				if (!string.IsNullOrEmpty(content))
					dic = content.Parse<Dictionary<string, string>>();
			}
			catch (Exception ex)
			{

			}
			return dic;
		}

		public RegisteredAddress GetCompanyRegAddressById(int id)
		{
			var dic = new RegisteredAddress();
			try
			{
				var content = CallElps($"/api/Address/ById/{id}/", HttpMethod.Get);

				if (!string.IsNullOrEmpty(content))
				{
					var response = content.Parse<RegisteredAddress>();
					dic = response;
				}
			}
			catch (Exception ex)
			{

			}
			return dic;
		}

		public List<RegisteredAddress> GetCompanyRegAddress(int id)
		{
			var dic = new List<RegisteredAddress>();
			try
			{
				var content = CallElps($"/api/Address/{id}/", HttpMethod.Get);

				if (!string.IsNullOrEmpty(content))
				{
					var response = content.Parse<List<RegisteredAddress>>();
					dic = response;
				}
			}
			catch (Exception ex)
			{

			}
			return dic;
		}

		public bool UpdateCompanyRegAddress(List<RegisteredAddress> model)
		{
			try
			{
				var content = CallElps($"/api/Address/{_appSetting.AppEmail}/{HttpHash()}", HttpMethod.Put, model);
				if (!string.IsNullOrEmpty(content))
					return true;
			}
			catch (Exception ex)
			{

			}
			return false;
		}

		public object AddCompanyRegAddress(List<RegisteredAddress> model, int companyId)
		{
			try
			{
				var content = CallElps($"/api/Address/{companyId}/", HttpMethod.Post, model);
				if (!string.IsNullOrEmpty(content))
					return content.Parse<List<RegisteredAddress>>();
			}
			catch (Exception ex)
			{

			}
			return false;
		}

		public List<DirectorModel> GetCompanyDirectors(int id)
		{
			var dic = new List<DirectorModel>();
			try
			{
				var content = CallElps($"/api/Directors/{id}/", HttpMethod.Get);

				if (!string.IsNullOrEmpty(content))
					dic = content.Parse<List<DirectorModel>>();

				//var docs = CallElps($"/api/Documents/Types/", HttpMethod.Get);
			}
			catch (Exception ex)
			{

			}
			return dic;
		}

		public object UpdateCompanyDetails(CompanyModel model, string email, bool update)
		{
			try
			{
				var content = CallElps("/api/Company/Edit/", HttpMethod.Put,
					new
					{
						company = model,
						companyMedicals = (string)null,
						companyExpatriateQuotas = (string)null,
						companyNsitfs = (string)null,
						companyProffessionals = (string)null,
						companyTechnicalAgreements = (string)null
					});
				if (!string.IsNullOrEmpty(content) && update)
				{
					var company = model.Stringify().Parse<CompanyModel>();
					if (company != null)
					{
						var res = UpdateCompanyNameEmail(new
						{
							Name = company.name,
							RC_Number = company.rC_Number,
							Business_Type = company.business_Type,
							emailChange = true,
							CompanyId = company.id,
							NewEmail = email
						});
						if (!string.IsNullOrEmpty(res))
							return company.Stringify().Parse<object>();
					}
				}
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public string UpdateCompanyNameEmail(object model)
		{
			try
			{
				var content = CallElps("/api/Accounts/ChangeEmail/", HttpMethod.Post, model);
				if (!string.IsNullOrEmpty(content))
					return content;
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public List<DocumentType> GetDocumentTypes(string type = null)
		{
			var docs = new List<DocumentType>();
			try
			{
				string requestUri = string.IsNullOrEmpty(type) ?
					$"/api/Documents/Types/{_appSetting.AppEmail}/{HttpHash()}"
					: $"/api/Documents/Facility/{_appSetting.AppEmail}/{HttpHash()}/{type}";
				var resp = Utils.Send(_appSetting.ElpsUrl, new HttpRequestMessage(HttpMethod.Get, requestUri))
					.Result;
				if (resp.IsSuccessStatusCode)
				{
					var content = resp.Content.ReadAsStringAsync().Result;
					if (!string.IsNullOrEmpty(content))
						docs = content.Parse<List<DocumentType>>();
				}
			}
			catch (Exception ex)
			{

			}
			return docs;
		}

		public dynamic GetCompanyDocuments(int id, string type = null)
		{
			string req = string.Empty;

			if (!string.IsNullOrEmpty(type) && type.ToLower().Equals("facility"))
				req = CallElps($"/api/FacilityFiles/{id}/", HttpMethod.Get);
			else
				req = CallElps($"api/CompanyDocuments/{id}/", HttpMethod.Get);

			if (!string.IsNullOrEmpty(req))
			{
				if (!string.IsNullOrEmpty(req))
				{
					if (!string.IsNullOrEmpty(type) && type.ToLower().Equals("facility"))
						return req.Parse<List<FacilityDocument>>();
					else
					{
						return req.Parse<List<Document>>();
					}
				}
			}
			//return null;

			//var docs = new List<CompanyDocument>();
			//try
			//{
			//    var content = CallElps($"/api/CompanyDocuments/{id}/", HttpMethod.Get);
			//    if (!string.IsNullOrEmpty(content))
			//        docs = content.Parse<List<CompanyDocument>>();
			//}
			//catch (Exception ex)
			//{

			//}
			return null;
		}

		public object GetFacilityDocuments(int id)
		{
			var docs = new List<CompanyDocument>();
			try
			{
				var content = CallElps($"/api/FacilityDocuments/{id}/", HttpMethod.Get);
				if (!string.IsNullOrEmpty(content))
					docs = content.Parse<List<CompanyDocument>>();
			}
			catch (Exception ex)
			{

			}
			return docs;

		}

		

		private string CallElps(string requestUri, HttpMethod method, object body = null)
		{
			var resp = new HttpResponseMessage();
			if (body != null)
				resp = Utils.Send(
					_appSetting.ElpsUrl,
					new HttpRequestMessage(method, $"{requestUri}{_appSetting.AppEmail}/{HttpHash()}")
					{
						Content = new StringContent(body.Stringify(), Encoding.UTF8, "application/json")
					}).Result;
			else
				resp = Utils.Send(
					_appSetting.ElpsUrl,
					new HttpRequestMessage(method, $"{requestUri}{_appSetting.AppEmail}/{HttpHash()}") { }).Result;

			if (resp.IsSuccessStatusCode)
			{
				var result = resp.Content.ReadAsStringAsync().Result;
				//_context.Logs.Add(new Log
				//{
				//    Action = $"HTTP Request - {method.Method}",
				//    Date = DateTime.UtcNow.AddHours(1),
				//    Error = result
				//});
				return result;
			}

			//_context.Logs.Add(new Log
			//{
			//    Action = $"HTTP Request - {method.Method} \n {resp.RequestMessage.Stringify()}",
			//    Date = DateTime.UtcNow.AddHours(1),
			//    Error = resp.ReasonPhrase
			//});

			return null;
		}

		private string HttpHash() => $"{_appSetting.AppEmail}{_appSetting.AppId}".GenerateSha512();
		//public List<MailTemplate> GetMailMessages() => _context.MailTemplates.ToList();

		public int PushPermitToElps(PermitAPIModel item)
		{
			int elpsid = 0;
			try
			{
				var resp = CallElps($"/api/Permits/{item.Company_Id}/", HttpMethod.Post, item);

				if (!string.IsNullOrEmpty(resp))
				{
					var content = resp.Parse<PermitAPIModel>();
					if (content.Id > 0)
						elpsid = content.Id;
				}
			}
			catch (Exception ex)
			{

			}
			return elpsid;
		}

		public string CreateElpsFacility(object item)
		{
			try
			{
				var content = CallElps("/api/Facility/Add/", HttpMethod.Post, item);
				if (!string.IsNullOrEmpty(content))
					return content;
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public string UpdateElpsFacility(object item)
		{
			try
			{
				var content = CallElps("/api/Facility/EditFacility/", HttpMethod.Put, item);
				if (!string.IsNullOrEmpty(content))
					return content;
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public Dictionary<string, object> ByPassPayment(int id)
		{
			try
			{
				var payment = _context.Payments.FirstOrDefault(x => x.ApplicationId == id);
				var dic = new Dictionary<string, object>
				{
					{"message", "Approved"},
					{"status", "01"}
				};
				if (payment != null)
				{
					payment.Status = "Approved";
					payment.TxnMessage = "Payment Confirmed";

				}
				else
				{
					payment = new Payment
					{
						Account = "TEST",
						Amount = 0,
						ApplicationId = id,
						Arrears = 0,
						Description = "Test",
						Status = "Approved",
						BankCode = "000",
						PaymentType = "By-pass",
						ServiceCharge = 0,
						TransactionDate = DateTime.UtcNow.AddHours(1),
						TransactionId = "Test",
						TxnMessage = "Payment By-passed",
						LateRenewalPenalty = 0,
						NonRenewalPenalty = 0,
						RRR = "218562"
					};
					_context.Payments.Add(payment);
				}
				_context.SaveChanges();

				return dic;
			}
			catch (Exception ex)
			{

			}
			return null;
		}
		//public string PostReferenceToIGR(string baseUrl, string requestUri, object payload)
		//{
		//    try
		//    {
		//        var resp = Utils.Utils.Send(baseUrl, new HttpRequestMessage(HttpMethod.Post, requestUri)
		//        {
		//            Content = new StringContent(payload.Stringify(), Encoding.UTF8, "application/json"),
		//            Headers = { Authorization = 
		//                new AuthenticationHeaderValue("Bearer", _appConfig.GetBearerToken().ToString())}
		//        }).Result;
		//        if (resp.IsSuccessStatusCode)
		//            return resp.Content.ReadAsStringAsync().Result;
		//    }
		//    catch (Exception ex)
		//    {

		//    }
		//    return null;
		//}

		public Dictionary<string, object> ConfirmEtxraRemitaStatus(int id)
		{
			try
			{
				var payment = _context.Payments.Include("ExtraPayment").FirstOrDefault(x => x.ApplicationId == id && x.ExtraPaymentId != null);
				if (payment != null)
				{
					var resp = Utils.Send(_appSetting.ElpsUrl,
						new HttpRequestMessage(HttpMethod.Get, $"/Payment/checkifpaid?id=r{payment.RRR}") { }).Result;
					if (resp.IsSuccessStatusCode)
					{
						var dic = resp.Content.ReadAsStringAsync().Result?.Parse<Dictionary<string, object>>();
						var message = dic.GetValue("message");
						var status = dic.GetValue("status");
						//if (dic.GetValue("message").Equals("Approved") || dic.GetValue("status").Equals("01"))
						if ((message != null && message.Equals("Successful")) || (status != null && status.Equals("01")))
						{
							payment.Status = "Approved";
							payment.TxnMessage = "Payment Confirmed";

							_context.SaveChanges();
						}
						return dic;
					}
				}
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public Dictionary<string, object> ConfirmRemitaStatus(int id)
		{
			try
			{
				var payment = _context.Payments.FirstOrDefault(x => x.ApplicationId == id);
				if (payment != null)
				{
					var resp = Utils.Send(_appSetting.ElpsUrl,
						new HttpRequestMessage(HttpMethod.Get, $"/Payment/checkifpaid?id=r{payment.RRR}") { }).Result;
					if (resp.IsSuccessStatusCode)
					{
						var dic = resp.Content.ReadAsStringAsync().Result?.Parse<Dictionary<string, object>>();

						if (dic.GetValue("message").Equals("Approved") ||
							dic.GetValue("status").Equals("01"))
						{
							payment.Status = "Approved";
							payment.TxnMessage = "Payment Confirmed";

							_context.SaveChanges();
						}
						return dic;
					}
				}
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public async Task<RemitaResponse> GeneratePaymentReference(string baseUrl, Application application, AppFee fee, int depots)
		{
			try
			{
				//var docs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.ApplicationType.Equals(application.ApplicationType.Name));
				// var pay = _context.PaymentLogs.FirstOrDefault(x => x.ApplicationId == application.Id);
				// if (pay != null && !string.IsNullOrEmpty(pay.RRR))
				// {
				//     exist = true;
				//     return pay.RRR;
				// }

				//Charge for IGR payments

				//var type = application.Facility.FacilityType;
				var totalAmount = fee.NOAFee + (fee.COQFee * depots) + fee.ApplicationFee + fee.SerciveCharge;
				var amountdue = fee.NOAFee;
				var remitaObject = new
				{
					serviceTypeId = _appSetting.ServiceTypeId,
					categoryName = DefaultValues.AppName,
					totalAmount = Decimal.ToInt32(totalAmount).ToString(),
					payerName = TruncateText(application.User.Company.Name, 25),
					payerEmail = application.User.Email,
					serviceCharge = Decimal.ToInt32(fee.SerciveCharge).ToString(),
					amountDue = Decimal.ToInt32(amountdue).ToString(),
					orderId = application.Reference,
					returnSuccessUrl = $"{baseUrl}/api/Payment/Remita?id={application.Payments.FirstOrDefault().Id}",
					returnFailureUrl = $"{baseUrl}/api/Payment/Remita?id={application.Payments.FirstOrDefault().Id}",
					returnBankPaymentUrl = $"{baseUrl}/api/Payment/Remita?id={application.Payments.FirstOrDefault().Id}",
					lineItems = new List<RPartner>
					{
						new RPartner
						{
							lineItemsId = "1",
							beneficiaryName = _appSetting.NMDPRABName,
							bankCode = _appSetting.NMDPRABankCode,
							beneficiaryAccount = _appSetting.NMDPRAAccount,
							beneficiaryAmount = $"{(double)(totalAmount - fee.SerciveCharge) + ((double)fee.SerciveCharge * 0.5)}",
							deductFeeFrom = "0"
						},
						new RPartner
						{
							lineItemsId = "2",
							beneficiaryName = _appSetting.BOBName,
							bankCode = _appSetting.BOBankCode,
							beneficiaryAccount = _appSetting.BOAccount,
							beneficiaryAmount = $"{(double)fee.SerciveCharge * 0.5}",
							deductFeeFrom = "1"
						}
					},
					customFields = new List<CustomField>
					{
						//new CustomField
						//{
						//	Name = "STATE",
						//	Value = $"{application.Facility.LGA.State.Name} State",
						//	Type = "All"
						//},
						new CustomField
						{
							Name = "COMPANY BRANCH",
							Value = application.Facility.Name,
							Type = "All"
						}
						//new CustomField
						//{
						//	Name = "FACILITY ADDRESS",
						//	Value = application.Facility.Address,
						//	Type = "All"
						//},
                        //new
                        //{
                        //    name = "Field/Zonal Office",
                        //    value = /*(from s in _context.States join f in _context.FieldLocations on s.FieldLocationId equals f.Id where s.Code.Equals(application.Facility.StateCode) select f.Description).FirstOrDefault()*/"",
                        //    type = "ALL"
                        //}
                    },
					documentTypes = (string)null,
					applicationItems = new List<ApplicationItem>
					{
						new ApplicationItem { Group = DefaultValues.AppName, Name = application.Facility.Name, Description = $"{application.Facility.Name} Facility payment" },
						new ApplicationItem { Group = "Vessel Details", Name = $"{application.Facility.Name}-{application.Facility.ElpsId}", Description = application.Facility.VesselType.Name },
						new ApplicationItem { Group = "Payment", Name = "Payment Description: ", Description = application.Payments.FirstOrDefault().Description }
					}
				};

				var result = CallElps($"/api/Payments/{application.User.ElpsId}/", HttpMethod.Post,
					remitaObject);
				if (result != null)
					return result.Parse<RemitaResponse>();
			}
			catch (Exception ex)
			{

			}
			return null;
		}

        public async Task<RemitaResponse> GenerateDebitNotePaymentReference(string baseUrl, double totalAmount, string companyName, string companyEmail, string appRef, string depotName, int compElpsId, string paymentType, string paymentDescription)
        {
            try
            {
                var remitaObject = new
                {
                    serviceTypeId = _appSetting.ServiceTypeId,
                    categoryName = DefaultValues.AppName,
                    totalAmount = totalAmount.ToString("###.##"),
                    payerName = TruncateText(companyName, 25),
                    payerEmail = companyEmail,
                    serviceCharge = Decimal.ToInt32(0).ToString(),
                    amountDue = (totalAmount * 0.5).ToString("###.##"),
                    orderId = appRef,
                    returnSuccessUrl = $"{baseUrl}/api/Payment/update-payment-status?appref={appRef}",
                    returnFailureUrl = $"{baseUrl}/api/Payment/update-payment-status?appref= {appRef}",
                    returnBankPaymentUrl = $"{baseUrl}/api/Payment/update-payment-status?appref= {appRef}",
                    lineItems = new List<RPartner>
                    {
                        new RPartner
                        {
                            lineItemsId = "1",
                            beneficiaryName = _appSetting.NMDPRABName,
                            bankCode = _appSetting.NMDPRABankCode,
                            beneficiaryAccount = _appSetting.NMDPRAAccount,
                            beneficiaryAmount = (totalAmount * 0.5).ToString("###.##"),
                            deductFeeFrom = "0"
                        },
                        new RPartner
                        {
                            lineItemsId = "2",
                            beneficiaryName = _appSetting.MDGIFBName,
                            bankCode = _appSetting.MDGIFBankCode,
                            beneficiaryAccount = _appSetting.MDGIFAccount,
                            beneficiaryAmount = (totalAmount * 0.5).ToString("###.##"),
                            deductFeeFrom = "1"
                        }
                    },
                    customFields = new List<CustomField>
                    {
						//new CustomField
						//{
						//	Name = "STATE",
						//	Value = $"{application.Facility.LGA.State.Name} State",
						//	Type = "All"
						//},
						new CustomField
                        {
                            Name = "COMPANY BRANCH",
                            Value = depotName,
                            Type = "All"
                        },
						new CustomField
						{
							Name = paymentType,
							Value = paymentDescription,
							Type = "All"
						},
                        //new
                        //{
                        //    name = "Field/Zonal Office",
                        //    value = /*(from s in _context.States join f in _context.FieldLocations on s.FieldLocationId equals f.Id where s.Code.Equals(application.Facility.StateCode) select f.Description).FirstOrDefault()*/"",
                        //    type = "ALL"
                        //}
                    },
                    documentTypes = (string)null,
                    applicationItems = new List<ApplicationItem>
                    {
                        new ApplicationItem { Group = paymentType, Name = depotName, Description = $"Debit Note payment for {depotName}" },
                        //new ApplicationItem { Group = "Payment", Name = "Payment Description: ", Description = application.Payments.FirstOrDefault().Description }
                    }
                };

                var result = CallElps($"/api/Payments/{compElpsId}/", HttpMethod.Post,
                    remitaObject);
                if (result != null)
                    return result.Parse<RemitaResponse>();
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Payment> GenerateExtraPaymentReference(string baseUrl, Application application, Payment payment, decimal totalAmount, decimal serviceCharge)
		{
			//try
			//{
			//    var docs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.FacilityTypeId.Equals(application.Facility.FacilityTypeId));

			//    //Charge for IGR payments

			//    var partnerCharge = 0;
			//    var tsa = (double)totalAmount  - partnerCharge;

			//    var type = application.LicenseTypeId == 1 ? "Distributors" : "Retailers";
			//    var remitaObject = new
			//    {
			//        serviceTypeId = _appConfig.Config().GetValue("servicetypeid"),
			//        categoryName = "LSSL",
			//        totalAmount = Decimal.ToInt32(totalAmount).ToString(),
			//        payerName = TruncateText(application.User.Company.Name, 25),
			//        payerEmail = application.User.Email,
			//        serviceCharge =  (string)null,
			//        amountDue = Decimal.ToInt32(totalAmount - serviceCharge).ToString(),
			//        orderId = payment.ExtraPayment.Reference,
			//        returnSuccessUrl = $"{baseUrl}/Payment/Remita",
			//        returnFailureUrl = $"{baseUrl}/Payment/Remita",
			//        returnBankPaymentUrl = $"{baseUrl}/Payment/Remita",
			//        lineItems = new object[]
			//        {
			//            new
			//            {
			//                lineItemsId = "1",
			//                beneficiaryName = "Beneficiary IGR",
			//                beneficiaryAccount = _appConfig.GetIGRAccount(),
			//                bankCode = _appConfig.GetRemitaBankCode(),
			//                beneficiaryAmount = tsa.ToString("#"),
			//                deductFeeFrom = "1"
			//            },
			//            new
			//            {
			//                lineItemsId = "2",
			//                beneficiaryName = "Beneficiary Target",
			//                beneficiaryAccount = _appConfig.GetTargetAccount(),
			//                bankCode = _appConfig.GetTargetBankCode(),
			//                beneficiaryAmount = "0",
			//                deductFeeFrom = "0"
			//            }
			//        },
			//        customFields = new object[]
			//        {
			//            new
			//            {
			//                name = "STATE",
			//                value = $"{application.Facility.State.Name} State",
			//                type = "ALL"
			//            },
			//            new
			//            {
			//                name = "COMPANY BRANCH",
			//                value = application.Facility.Name,
			//                type = "ALL"
			//            },
			//            new
			//            {
			//                name = "FACILITY ADDRESS",
			//                value = application.Facility.Address,
			//                type = "ALL"
			//            },
			//            new
			//            {
			//                name = "Field/Zonal Office",
			//                value = (from s in _context.States join f in _context.FieldLocations on s.FieldLocationId equals f.Id where s.Code.Equals(application.Facility.StateCode) select f.Description).FirstOrDefault(),
			//                type = "ALL"
			//            }
			//        },
			//        documentTypes = docs.Select(x => x.DocumentTypeId).ToList(),
			//        applicationItems = new object[]
			//        {
			//            new
			//            {
			//                name = "Bunkering",
			//                description = $"Extra Payment for {application.ApplicationType.Name}",
			//                group = $"Bunkering License for {type} Facility"
			//            }
			//        }
			//    };

			//    var result = CallElps($"/api/Payments/{application.User.ElpsId}/", HttpMethod.Post,
			//        remitaObject);
			//    if (result != null)
			//    {
			//        var response = result.Parse<RemitaResponse>();
			//        if (!string.IsNullOrEmpty(response.rrr))
			//        {
			//            payment.RRR = response.rrr;
			//            payment.AppReceiptId = response.appId;
			//            payment.TransactionId = response.transactionId;

			//            // var extrapay = _context.ExtraPayments.Add(payment.ExtraPayment);
			//            // _context.SaveChanges();

			//            _context.PaymentLogs.Add(payment);
			//            _context.SaveChanges();
			//            exist = true;

			//            return payment;

			//        }
			//    }
			//}
			//catch (Exception ex)
			//{

			//}
			return null;
		}

		public List<Staff> GetAllStaff()
		{
			try
			{
				var resp = CallElps("/api/Accounts/Staff/", HttpMethod.Get);
				if (!string.IsNullOrEmpty(resp))
					return resp.Parse<List<Staff>>();
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public Staff GetStaff(string email)
		{
			try
			{
				var resp = CallElps($"/api/Accounts/Staff/{email}/", HttpMethod.Get);
				if (!string.IsNullOrEmpty(resp))
					return resp.Parse<Staff>();
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public String TruncateText(String text, int length)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Length > length)
					text = text.Substring(0, length);
			}
			else
				text = "";

			return text;
		}

		public Dictionary<string, string> ChangePassword(object model, string useremail)
		{
			try
			{
				var resp = CallElps($"/api/Accounts/ChangePassword/{useremail}/",
					HttpMethod.Post, model);

				if (!string.IsNullOrEmpty(resp))
					return resp.Parse<Dictionary<string, string>>();
			}
			catch (Exception ex)
			{

			}

			return null;
		}

		//public MistdoResponse VerifyMISTDOCert(string cert)
		//{
		//    var req = Utils.Send(
		//        _appConfig.MISTDOBase(),
		//        new HttpRequestMessage(HttpMethod.Get, $"/home/verifymistdocertificate?certificateid={cert}")).Result;
		//    if (req.IsSuccessStatusCode)
		//    {
		//        var resp = req.Content.ReadAsStringAsync().Result;
		//        if (!string.IsNullOrEmpty(resp))
		//            return resp.Parse<MistdoResponse>();
		//    }

		//    return null;
		//}

	}
}