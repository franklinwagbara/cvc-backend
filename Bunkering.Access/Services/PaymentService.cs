﻿using Azure.Core;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Bunkering.Access.Services
{
    public class PaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string User;
        ApiResponse _response;
        private readonly AppLogger _logger;
        private readonly string directory = "Payment";
        private readonly IElps _elps;
        private readonly AppSetting _appSetting;
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

       

        public PaymentService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            AppLogger logger,
            IElps elps,
            ApplicationContext context,
            IOptions<AppSetting> appSetting,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _logger = logger;
            _elps = elps;
            _context = context;
            _appSetting = appSetting.Value;
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreatePayment(int id)
        {
            if (id > 0)
            {
                var baseUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";
                //var IsExtraPayment = _appSettings.IsExtraPayment;
                var RemitaPayment_URL = $"{baseUrl}/api/payment/Remita";
                var ResponsePayment_URL = $"{baseUrl}/api/payment/RemitaResponse";
                //var FailedPayment_URL = baseUrl + _appSettings.FailedExtraPayment;

                try
                {
                    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "User.Company,Facility.VesselType,Payments");
                    if (app == null)
                        _response = new ApiResponse { Message = "This application record does not exist or has been removed from the system, kindly contact support.", StatusCode = HttpStatusCode.NotFound };
                    else
                    {
                        var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(x => x.ApplicationId == id);

                        if (payment != null && !string.IsNullOrEmpty(payment.RRR))
                            _response = new ApiResponse { Message = "Payment already exists", StatusCode = HttpStatusCode.BadRequest };
                        else
                        {
                            if (payment != null && string.IsNullOrEmpty(payment.RRR))
                            {
                                var fee = await _unitOfWork.AppFee.FirstOrDefaultAsync(x => x.ApplicationTypeId.Equals(app.ApplicationTypeId) && x.IsDeleted != true);
                                if (fee != null)
                                {
                                    int numOfDepots = 1;
                                    var appDepots = _unitOfWork.ApplicationDepot.GetAll("Depot").Result.Where(x => x.AppId == app.Id);
                                    if (appDepots != null)
                                    {
                                        numOfDepots = appDepots.Count();
                                    }
                                    var total = fee.ApplicationFee + fee.ProcessingFee + (fee.COQFee * numOfDepots) + fee.SerciveCharge + fee.NOAFee;

                                    var request = await _elps.GeneratePaymentReference($"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}", app, fee, numOfDepots);
                                    _logger.LogRequest("Creation of payment split for application with reference:" + app.Reference + "(" + app.User.Company.Name + ") by " + User, false, directory);

                                    if (request == null)
                                    {
                                        await _unitOfWork.Payment.Remove(payment);
                                        await _unitOfWork.SaveChangesAsync("system");
                                        _logger.LogRequest($"Payment output from Remita:: {request.Stringify()} by {User}", true, directory);
                                        _response = new ApiResponse { Message = "An error occured while generating this payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };

                                    }
                                    else
                                    {
                                        //var resp = request.Stringify().Parse<RemitaResponse>();

                                        if (!string.IsNullOrEmpty(request.RRR))
                                        {
                                            payment.RRR = request.RRR;
                                            await _unitOfWork.Payment.Update(payment);
                                            await _unitOfWork.SaveChangesAsync(app.UserId);

                                            _logger.LogRequest($"Payment table updated with RRR: {payment.RRR} by {User}", false, directory);

                                            #region Send Payment E-Mail To Company
                                            string subject = $"Generation Of Payment For Application With Ref:{app.Reference}";

                                            var emailBody = string.Format($"A Payment RRR: {payment.RRR} has been generated for your application with reference number: {app.Reference}" +
                                                "<br /><ul>" +
                                                "<li>Amount Generated: {0}</li>" +
                                                "<li>Remita RRR: {1}</li>" +
                                                "<li>Payment Status: {2}</li>" +
                                                "<li>Payment Description: {3}</li>" +
                                                "<li>Vessel Name: {4}</li>" +
                                                "<p>Kindly note that your application will be pending until this payment is completed. </p>",
                                                payment.Amount.ToString(), payment.RRR, payment.Status, payment.Description, $"{app.Facility.Name}-{app.Facility.ElpsId}");

                                            #endregion

                                            string successMsg = "RRR (" + payment.RRR + ") generated successfully for company: " + app.User.Company.Name + "; Facility: " + $"{app.Facility.Name}-{app.Facility.ElpsId}";
                                            _response = new ApiResponse
                                            {
                                                Message = successMsg,
                                                Data = new { rrr = payment.RRR },
                                                StatusCode = HttpStatusCode.OK,
                                                Success = true
                                            };
                                        }
                                        else
                                            _response = new ApiResponse
                                            {
                                                Message = "Unable to generate RRR, pls try again",
                                                StatusCode = HttpStatusCode.InternalServerError
                                            };
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogRequest($"An error {ex.Message} occured while trying to generate extra payment RRR for this application by {User}", true, directory);
                    _response = new ApiResponse { Message = "An error occured while generating this extra payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };
                }
            }
            return _response;
        }

        public async Task<ApiResponse> GenerateDebitNote(int id)
        {
            var baseUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";

            try
            {
                double total = 0;
                string orderid = string.Empty;
                string facName = string.Empty;
                string companyName = string.Empty;
                string companyEmail = string.Empty;
                string description = string.Empty;
                int compElpsId = 0;
                
                int? appId = null;

                var user = await _userManager.FindByEmailAsync(User);
                if (id < 0)
                    _response = new ApiResponse { Message = "This COQ record does not exist or has been removed from the system, kindly contact support.", StatusCode = HttpStatusCode.NotFound };
                else
                {
                    var coqRef = await _unitOfWork.CoQReference.FirstOrDefaultAsync(x => x.Id.Equals(id), "DepotCoQ.Application.User.Company, ProcessingPlantCOQ");
                    var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)));
                    var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(x => x.ApplicationTypeId.Equals(appType.Id) && x.COQId.Equals(coqRef.Id));

                    if (payment != null)
                        _response = new ApiResponse { Message = "Debit note already exists", StatusCode = HttpStatusCode.BadRequest };
                    else
                    {
                        if (payment == null)
                        {
                            string productType = string.Empty;
                            if (coqRef.PlantCoQId == null)
                            {
                                var prd = _context.ApplicationDepots.Include(p => p.Product).FirstOrDefault(x => x.AppId == coqRef.DepotCoQ.AppId);
                                productType = prd.Product?.ProductType ?? string.Empty;

                                orderid = coqRef.DepotCoQ.Reference;
                                appId = coqRef.DepotCoQ.AppId;
                                compElpsId = coqRef.DepotCoQ.Application.User.ElpsId;
                                facName = coqRef.DepotCoQ.Plant.Name;
                                companyName = coqRef.DepotCoQ.Application.User.Company.Name;
                                companyEmail = coqRef.DepotCoQ.Application.User.Email;
                                description = $"Debit note amount for CoQ with reference {orderid} for {companyName}({facName})";

                                total = productType.Equals(Enum.GetName(typeof(ProductTypes), ProductTypes.Gas)) ? coqRef.DepotCoQ.MT_VAC * coqRef.DepotCoQ.DepotPrice * 0.01 : coqRef.DepotCoQ.GSV * coqRef.DepotCoQ.DepotPrice * 0.01;
                            }
                            else
                            {
                                var processingPlant = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id.Equals(coqRef.ProcessingPlantCOQ.PlantId));
                                orderid = coqRef.ProcessingPlantCOQ.Reference;
                                compElpsId = (int)processingPlant.ElpsPlantId.Value;
                                facName = processingPlant.Name;
                                companyName = processingPlant.Company;
                                companyEmail = processingPlant.Email;
                                description = $"Debit note amount for CoQ with reference {orderid} for {companyName}({facName})";

                                total = coqRef.ProcessingPlantCOQ.TotalMTVac.Value * coqRef.ProcessingPlantCOQ.Price * 0.01;
                            }

                            payment = new Payment
                            {
                                Account = "",
                                Amount = total,
                                ApplicationId = appId,
                                ApplicationTypeId = appType.Id,
                                COQId = coqRef.Id,
                                Description = description,
                                OrderId = orderid,
                                RRR = string.Empty,
                                AppReceiptId = string.Empty,
                                TxnMessage = string.Empty,
                                PaymentType = "NGN",
                                TransactionDate = DateTime.UtcNow.AddHours(1),
                                BankCode = string.Empty,
                                Status = Enum.GetName(typeof(AppStatus), AppStatus.PaymentPending),
                                LateRenewalPenalty = 0,
                                NonRenewalPenalty = 0,
                                Arrears = 0,
                                LastRetryDate = DateTime.UtcNow.AddHours(1),
                                RetryCount = 0,
                                ServiceCharge = 0,
                                TransactionId = string.Empty,
                            };
                            await _unitOfWork.Payment.Add(payment);
                            await _unitOfWork.SaveChangesAsync(user.Id);

                            _logger.LogRequest($"Debit Note saved for {orderid} by {User}", false, directory);
                            #region Send Payment E-Mail To Company
                            string subject = $"Generation of Debit Note For COQ with reference: {orderid}";

                            //var emailBody = string.Format($"Debit Note has been generated for your COQ with reference number: {coq.Reference}" +
                            //    "<br /><ul>" +
                            //    "<li>Amount Generated: {0}</li>" +
                            //    //"<li>Remita RRR: {1}</li>" +
                            //    "<li>Payment Status: {1}</li>" +
                            //    "<li>Payment Description: {2}</li>" +
                            //    "<li>Vessel Name: {3}</li>" +
                            //    "<p>Kindly visit the <a hhref=''>portal to generate RRR for payment. </p>",
                            //    payment.Amount.ToString(), payment.Status, payment.Description, $"{coqRef.CoQ.Plant.Name}");

                            #endregion

                            //string successMsg = $"Debit Note RRR ({payment.RRR}) generated successfully for {coqRef.CoQ.Plant.Name}";
                            //_response = new ApiResponse
                            //{
                            //    Message = successMsg,
                            //    StatusCode = HttpStatusCode.OK,
                            //    Success = true
                            //};

                            var request = await _elps.GenerateDebitNotePaymentReference($"{baseUrl}", total, companyName, coqRef.DepotCoQ.Application.User.Email, orderid, facName, compElpsId, Enum.GetName(typeof(AppTypes), AppTypes.DebitNote), description);
                            _logger.LogRequest("Creation of payment split for application with reference:" + orderid + "(" + companyName + ") by " + User, false, directory);

                            if (request == null)
                            {
                                _logger.LogRequest($"Payment output from Remita:: {request.Stringify()} by {User}", true, directory);
                                _response = new ApiResponse { Message = "An error occured while generating this payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(request.RRR))
                                {
                                    #region Send Payment E-Mail To Company
                                    subject = $"Generation of Debit Note For COQ with reference: {orderid}";

                                    var emailBody = string.Format($"A Payment RRR: {payment.RRR} has been generated for your COQ with reference number: {orderid}" +
                                        "<br /><ul>" +
                                        "<li>Amount Generated: {0}</li>" +
                                        "<li>Remita RRR: {1}</li>" +
                                        "<li>Payment Status: {2}</li>" +
                                        "<li>Payment Description: {3}</li>" +
                                        "<li>Vessel Name: {4}</li>" +
                                        "<p>Kindly note that your application will be pending until this payment is completed. </p>",
                                        payment.Amount.ToString(), payment.RRR, payment.Status, description, $"{facName}");

                                    #endregion

                                    var successMsg = $"Debit Note RRR ({payment.RRR}) generated successfully for {facName}";
                                    _response = new ApiResponse
                                    {
                                        Message = successMsg,
                                        Data = new { rrr = payment.RRR },
                                        StatusCode = HttpStatusCode.OK,
                                        Success = true
                                    };
                                }
                                else
                                    _response = new ApiResponse
                                    {
                                        Message = "Unable to generate RRR, pls try again",
                                        StatusCode = HttpStatusCode.InternalServerError
                                    };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogRequest($"An error {ex.Message} occured while trying to generate extra payment RRR for this application by {User}", true, directory);
                _response = new ApiResponse { Message = "An error occured while generating this extra payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };
            }
            return _response;
        }

        public async Task<ApiResponse> GenerateDemandNotice(int id)
        {
            if (id > 0)
            {
                var baseUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";

                try
                {
                    var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.IsDeleted == false, "Application.User.Company,Facility.VesselType");
                    if (coq == null)
                        _response = new ApiResponse { Message = "This COQ record does not exist or has been removed from the system, kindly contact support.", StatusCode = HttpStatusCode.NotFound };
                    else
                    {
                        var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)));
                        var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(x => x.ApplicationTypeId.Equals(appType.Id) && x.COQId.Equals(coq.Id) && x.ApplicationId.Equals(coq.AppId));

                        if (payment == null)
                            _response = new ApiResponse { Message = "Debit note does not exist for this Depot", StatusCode = HttpStatusCode.BadRequest };
                        else
                        {
                            var total = payment.Amount * 0.10;
                            var reference = Utils.RefrenceCode();
                            var description = $"Payment for non-payment of Debit note generated for {coq.Plant.Name} after 21 days as regulated";

                            var request = await _elps.GenerateDebitNotePaymentReference($"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}", total, coq.Application.User.Company.Name, coq.Application.User.Email, coq.Reference, coq.Plant.Name, coq.Application.User.ElpsId, Enum.GetName(typeof(AppTypes), AppTypes.DemandNotice), description);
                            _logger.LogRequest($"Creation of Demand notice payment for application with reference: {reference}for ({coq.Application.User.Company.Name}) as specified in the Authority regulations", false, directory);

                            if (request == null)
                            {
                                _logger.LogRequest($"Payment output from Remita:: {request.Stringify()} by {User}", true, directory);
                                _response = new ApiResponse { Message = "An error occured while generating this payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };
                            }
                            else
                            {
                                var demandNotice = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DemandNotice)));
                                if (!string.IsNullOrEmpty(request.RRR))
                                {
                                    payment = new Payment
                                    {
                                        Account = "",
                                        Amount = total,
                                        ApplicationId = coq.AppId,
                                        ApplicationTypeId = demandNotice.Id,
                                        COQId = coq.Id,
                                        Description = description,
                                        OrderId = reference,
                                        RRR = request.RRR,
                                        PaymentType = "NGN",
                                        TransactionDate = DateTime.UtcNow.AddHours(1),
                                        Status = Enum.GetName(typeof(AppStatus), AppStatus.PaymentPending)
                                    };
                                    await _unitOfWork.Payment.Add(payment);
                                    await _unitOfWork.SaveChangesAsync(coq.Application.UserId);

                                    _logger.LogRequest($"Payment for debit note with RRR: {payment.RRR} saved for {coq.Reference} by {User}", false, directory);

                                    #region Send Payment E-Mail To Company
                                    string subject = $"Generation Of Payment For Application With Ref:{coq.Reference}";

                                    var emailBody = string.Format($"A Payment RRR: {payment.RRR} has been generated for your COQ request with reference number: {coq.Reference}" +
                                        "<br /><ul>" +
                                        "<li>Amount Generated: {0}</li>" +
                                        "<li>Remita RRR: {1}</li>" +
                                        "<li>Payment Status: {2}</li>" +
                                        "<li>Payment Description: {3}</li>" +
                                        "<li>Vessel Name: {4}</li>" +
                                        "<p>Kindly note that your application will be pending until this payment is completed. </p>",
                                        payment.Amount.ToString(), payment.RRR, payment.Status, payment.Description, $"{coq.Plant.Name}");

                                    #endregion

                                    string successMsg = $"Debit Note RRR ({payment.RRR}) generated successfully for {coq.Plant.Name}";
                                    _response = new ApiResponse
                                    {
                                        Message = successMsg,
                                        Data = new { rrr = payment.RRR },
                                        StatusCode = HttpStatusCode.OK,
                                        Success = true
                                    };
                                }
                                else
                                    _response = new ApiResponse
                                    {
                                        Message = "Unable to generate RRR, pls try again",
                                        StatusCode = HttpStatusCode.InternalServerError
                                    };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogRequest($"An error {ex.Message} occurred while trying to generate extra payment RRR for this application by {User}", true, directory);
                    _response = new ApiResponse { Message = "An error occurred while generating this extra payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };
                }
            }
            return _response;
        }
      
        public async Task<ApiResponse> ConfirmPayment(int id, string orderId)
        {
            try
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(id), "Payments");
                if (app != null)
                {
                    var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(x => x.ApplicationId == id && x.OrderId.Equals(orderId));
                    if (payment != null)
                    {
                        if (!payment.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted)) && !string.IsNullOrEmpty(payment.RRR))
                        {
                            //confirm payment status on remita via ELPS
                            var http = await Utils.Send(_appSetting.ElpsUrl, new HttpRequestMessage(HttpMethod.Get, $"/Payment/checkifpaid?id=r{payment.RRR}"));

                            if (http.IsSuccessStatusCode)
                            {
                                var content = http.Content.ReadAsStringAsync().Result;
                                if (content != null)
                                {
                                    var dic = content.Parse<Dictionary<string, string>>();
                                    if ((!string.IsNullOrEmpty(dic.GetValue("message").ToString()) && dic.GetValue("message").ToString().Equals("Successful"))
                                        || (!string.IsNullOrEmpty(dic.GetValue("status").ToString()) && dic.GetValue("status").ToString().Equals("00")))
                                    {
                                        payment.Status = Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted);
                                        payment.TransactionDate = Convert.ToDateTime(dic.GetValue("transactiontime"));
                                        payment.PaymentDate = Convert.ToDateTime(dic.GetValue("paymentDate"));
                                        payment.AppReceiptId = dic.GetValue("appreceiptid") != null ? dic.GetValue("appreceiptid") : "";
                                        payment.TxnMessage = dic.GetValue("message");
                                        //payment.tx = Convert.ToDecimal(dic.GetValue("amount"));
                                        //payment.Application.Status = Enum.GetName(typeof(AppStatus), 2);

                                        
                                        await _unitOfWork.Payment.Update(payment);
                                        await _unitOfWork.SaveChangesAsync(app.UserId);

                                        _response = new ApiResponse
                                        {
                                            Message = "Payment confirmed successfully",
                                            StatusCode = HttpStatusCode.OK,
                                            Success = true,
                                        };
                                    }
                                    else
                                        _response = new ApiResponse
                                        {
                                            Message = "Not Successful",
                                            StatusCode = HttpStatusCode.BadRequest
                                        };
                                }
                            }
                            else
                                _response = new ApiResponse
                                {
                                    Message = "Failed",
                                    StatusCode = HttpStatusCode.NotFound
                                };
                        }
                        else
                        {
                            _response = new ApiResponse
                            {
                                Message = "Payment already completed",
                                StatusCode = HttpStatusCode.OK,
                                Success = true
                            };
                        }

                    }

                    _logger.LogRequest($"\"Getting payment for company application -:{app.Reference}{" by"}{_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email)} {" - "}{DateTime.Now}", false, directory);
                }
            }
            catch (Exception ex)
            {
                _response = new ApiResponse { Message = "Internal error occured " + ex.ToString(), StatusCode = HttpStatusCode.InternalServerError };
                _logger.LogRequest($"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace}", true, directory);
            }
            return _response;
        }

        public async Task<ApiResponse> ConfirmOtherPayment(string orderId)
        {
            try
            {
                var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(x => x.OrderId.Equals(orderId));
                if (payment != null)
                {
                    if (!payment.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted)) && !string.IsNullOrEmpty(payment.RRR))
                    {
                        //confirm payme nt status on remita via ELPS
                        var http = await Utils.Send(_appSetting.ElpsUrl, new HttpRequestMessage(HttpMethod.Get, $"/Payment/checkifpaid?id=r{payment.RRR}"));

                        if (http.IsSuccessStatusCode)
                        {
                            var content = http.Content.ReadAsStringAsync().Result;
                            if (content != null)
                            {
                                var dic = content.Parse<Dictionary<string, string>>();
                                if ((!string.IsNullOrEmpty(dic.GetValue("message").ToString()) && dic.GetValue("message").ToString().Equals("Successful"))
                                    || (!string.IsNullOrEmpty(dic.GetValue("status").ToString()) && dic.GetValue("status").ToString().Equals("00")))
                                {
                                    payment.Status = Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted);
                                    payment.TransactionDate = Convert.ToDateTime(dic.GetValue("transactiontime"));
                                    payment.PaymentDate = Convert.ToDateTime(dic.GetValue("paymentDate"));
                                    payment.AppReceiptId = dic.GetValue("appreceiptid") != null ? dic.GetValue("appreceiptid") : "";
                                    payment.TxnMessage = dic.GetValue("message");
                                    //payment.tx = Convert.ToDecimal(dic.GetValue("amount"));
                                    //payment.Application.Status = Enum.GetName(typeof(AppStatus), 2);

                                    await _unitOfWork.Payment.Update(payment);
                                    await _unitOfWork.SaveChangesAsync(User);

                                    _response = new ApiResponse
                                    {
                                        Data = new { id = payment.Id },
                                        Message = "Payment confirmed successfully",
                                        StatusCode = HttpStatusCode.OK,
                                        Success = true,
                                    };
                                }
                                else
                                    _response = new ApiResponse
                                    {
                                        Message = "Not Successful",
                                        StatusCode = HttpStatusCode.BadRequest
                                    };
                            }
                        }
                        else
                            _response = new ApiResponse
                            {
                                Message = "Failed",
                                StatusCode = HttpStatusCode.NotFound
                            };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Message = "Payment already completed",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }

                }

                _logger.LogRequest($"\"Getting payment for company payment -:{payment.OrderId}{" by"}{_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email)} {" - "}{DateTime.Now}", false, directory);
            }
            catch (Exception ex)
            {
                _response = new ApiResponse { Message = "Internal error occured " + ex.ToString(), StatusCode = HttpStatusCode.InternalServerError };
                _logger.LogRequest($"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace}", true, directory);
            }
            return _response;
        }

        public async Task<ApiResponse> PaymentReport(PaymentReportViewModel model)
        {
            var payment = await _unitOfWork.vAppPayment.Find(a => a.TransactionDate >= model.Min && a.TransactionDate <= model.Max);

            if (!string.IsNullOrEmpty(model.AppStatus))
                payment = payment.Where(b => b.PaymentStatus.ToLower().Equals(model.AppStatus.ToLower())).ToList();

            if (payment != null)
            {
                _response = new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = payment.Select(x => new
                    {
                        x.Id,
                        x.ApplicationId,
                        x.AppType,
                        x.AppReference,
                        x.Description,
                        x.OrderId,
                        x.TransactionDate,
                        x.PaymentDate,
                        x.PaymentType,
                        x.Account,
                        x.RRR,
                        x.TxnMessage,
                        x.Amount,
                        x.ServiceCharge,
                        x.PaymentStatus,
                    }).ToList(),
                };

                return _response;
            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "Payment not found",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false

                };
            }

            return _response;
        }

        public async Task<ApiResponse> GetAllPayments()
        {
            var payments = await _unitOfWork.vPayment.GetAll();
            return new ApiResponse
            {
                Data = payments,
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };

        }

        public async Task<ApiResponse> GetPaymentById(int id)
        {
            var paymentById = await _unitOfWork.vPayment.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (paymentById == null)
            {
                _response = new ApiResponse
                {
                    Message = "Payment not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            }
            _response = new ApiResponse
            {
                Data = paymentById,
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
            return _response;
        }

        public async Task<ApiResponse> GetDebitNotesByAppId(int id)
        {
            try
            {
                var debitnoteId = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)));
                var debitnotes = await _unitOfWork.vDebitNote.Find(x => x.ApplicationId.Equals(id) && x.ApplicationTypeId.Equals(debitnoteId.Id));

                if (debitnotes == null)
                    throw new Exception("Debit note does not exist for this application!");

                //var depot = await _unitOfWork.Depot.FirstOrDefaultAsync(x => x.Id.Equals(appDepot.Id));  
                //var product = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id.Equals(appDepot.ProductId));

                return new ApiResponse
                {
                    Data = debitnotes.Select(x => new
                    {
                        x.Id,
                        x.COQId,
                        x.OrderId,
                        x.ApptypeName,
                        x.TransactionDate,
                        x.Status,
                        x.DepotName,
                        x.Description,
                        x.PaymentDate,
                        x.RRR,
                        x.Amount,
                        x.ServiceCharge,
                        x.Arrears
                    }),
                    Message = "Successfull.",
                    StatusCode = HttpStatusCode.OK,
                    Success = false
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                };
            }
        }

        public async Task<ApiResponse> GetPendingPaymentsByAppId(int id)
        {
            try
            {
                var payments = await _unitOfWork.vDebitNote.Find(x => x.ApplicationId.Equals(id) && x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentPending)));

                if (payments == null)
                    throw new Exception("No pending payment found for this application!");

                return new ApiResponse
                {
                    Data = payments.Select(x => new
                    {
                        x.Id,
                        x.COQId,
                        x.OrderId,
                        x.ApptypeName,
                        x.TransactionDate,
                        x.Status,
                        x.DepotName,
                        x.Description,
                        x.PaymentDate,
                        x.RRR,
                        x.Amount,
                        x.ServiceCharge,
                        x.Arrears
                    }),
                    Message = "Successfull",
                    StatusCode = HttpStatusCode.OK,
                    Success = false
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                };
            }
        }
    }
}
