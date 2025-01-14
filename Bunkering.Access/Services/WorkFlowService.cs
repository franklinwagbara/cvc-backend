﻿using Bunkering.Core.Data;
using Bunkering.Access;
using Bunkering.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text;
using Bunkering.Access.IContracts;
using Bunkering.Access;
using Bunkering.Core.ViewModels;
using Bunkering.Access.DAL;
using AutoMapper.Internal;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bunkering.Access.Services
{
    public class WorkFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _env;
        private readonly MailSettings _mailSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSetting _appSetting;
        private readonly PaymentService _paymentService;
        private readonly ApplicationContext _context;
        //private readonly Location _location;
        //private readonly Office _office;

        //private readonly string directory = "WorkFlow";
        //private readonly GeneralLogger _generalLogger;

        public WorkFlowService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment env,
            IOptions<MailSettings> mailSettings,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSetting> appSetting,
            PaymentService paymentService,
            ApplicationContext context
            //Location location,
            //Office office
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _env = env;
            _mailSetting = mailSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _appSetting = appSetting.Value;
            _paymentService = paymentService;
            _context = context;
            //_location = location;
            //_office = office;
        }

        public async Task<(bool, string)> AppWorkFlow(int appid, string action, string comment, string currUserId = null, string delUserId = null)
        {
            var res = false;
            ApplicationUser nextprocessingofficer = null;
            var wkflow = new WorkFlow();
            string processingMsg = string.Empty;
            try
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == appid, "User,Facility.VesselType,Payments");
                //var loc = await _unitOfWork.Location.Find(x => x.Id);
                //var off = await _unitOfWork.Office.Find(x => x.Id);
                if (app != null)
                {

                    currUserId = string.IsNullOrEmpty(currUserId) ? app.CurrentDeskId : currUserId;
                    var currentUser = _userManager.Users
                        .Include(x => x.Company)
                        .Include(ol => ol.Office)
                        .Include(lo => lo.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Include(lo => lo.Location)
                        .FirstOrDefault(x => x.Id.Equals(currUserId));

                    var currentuserRoles = currentUser.UserRoles
                        .Where(x => !x.Role.Name.Equals("Staff")).FirstOrDefault().Role.Id;

                    if (currentUser != null)
                    {
                        wkflow = await GetWorkFlow(action, currentUser, app.Facility.VesselTypeId, app.ApplicationTypeId);
                        if (wkflow != null) //get next processing staff
                            nextprocessingofficer = await GetNextStaff(appid, action, wkflow, currentUser, delUserId);

                        if (nextprocessingofficer != null)
                        {
                            //update application
                            app.CurrentDeskId = nextprocessingofficer.Id;
                            app.Status = wkflow.Status;
                            app.FlowId = wkflow.Id;
                            app.ModifiedDate = DateTime.UtcNow.AddHours(1);
                            app.DeskMovementDate = DateTime.Now;
                            if (action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit)))
                                app.SubmittedDate = DateTime.Now.AddHours(1);

                            if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Approve).ToLower()))
                                processingMsg = "Application processed successfully and moved to the next processing staff";
                            else if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Submit).ToLower()))
                                processingMsg = "Application submitted successfully";
                            else if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit).ToLower()))
                                processingMsg = "Application re-submitted successfully";
                            else
                                processingMsg = "Application has been returned for review";

                            nextprocessingofficer.LastJobDate = DateTime.UtcNow.AddHours(1);
                            await _userManager.UpdateAsync(nextprocessingofficer);
                            //save action to history
                            await SaveHistory(action, appid, wkflow, currentUser, nextprocessingofficer, comment);
                            res = true;

                            //Generate permit number on final approval
                            if (wkflow.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Completed)))
                            {
                                //await _unitOfWork.Application.Update(app);
                                var nextSUrveyor = await _unitOfWork.NominatedSurveyor.GetNextAsync();
                                if (nextSUrveyor != null)
                                {
                                    app.SurveyorId = nextSUrveyor.Id;
                                    var appDepot = await _unitOfWork.ApplicationDepot.Find(x => x.AppId == app.Id);
                                    var volume = appDepot.Sum(x => x.Volume);

                                    nextSUrveyor.NominatedVolume += volume;
                                    var appSurveyor = new ApplicationSurveyor()
                                    {
                                        ApplicationId = app.Id,
                                        NominatedSurveyorId = nextSUrveyor.Id,
                                        Volume = volume,
                                        CreatedAt = DateTime.UtcNow.AddHours(1),
                                    };
                                    await _unitOfWork.ApplicationSurveyor.Add(appSurveyor);
                                    
                                }
                                   
                                await _unitOfWork.SaveChangesAsync(currentUser.Id);

                                var permit = await GeneratePermit(appid, currentUser.Id);
                                var s = await PostDischargeId(app.Id);
                                if (s is false)
                                {
                                    res = false;
                                    processingMsg = "Discharge Id not generated";

                                }
                                else
                                {
                                    if (permit.Item1)
                                        processingMsg = $"Application with reference {app.Reference} has been approved and clearance number {permit.Item2} has been generated successfully";
                                }

                               
                            }
                            //send and save notification
                            await SendNotification(app, action, nextprocessingofficer, processingMsg);
                        }
                        else
                        {
                            res = false;
                            processingMsg = "Next processing staff is not profiled";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_generalLogger.LogRequest($"{"Internal server error occurred while trying to fetch staff dashboard"}{"-"}{DateTime.Now}", true, directory);
            }
            return (res, processingMsg);
        }

        public async Task<(bool, string)> CoqWorkFlow(int coqId, string action, string comment, string currUserId = null, string delUserId = null)
        {
            try
            {
                var isProcessingPlant = false;
                    //return (false, $"Application with Id={coq.AppId} was not found.");
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id.Equals(coqId), "Application.User.Company,Application.Facility.VesselType") ?? throw new Exception($"COQ with Id={coqId} was not found.");
                if (coq is not null && coq.AppId is null)
                {
                    isProcessingPlant = true;
                }
                var message = string.Empty;
                //var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == coq.AppId, );
                if(!isProcessingPlant && coq.Application is null) throw new Exception($"Application with Id={coq.AppId} was not found.");

                currUserId = string.IsNullOrEmpty(currUserId) ? coq.CurrentDeskId : currUserId;
                var currentUser = _userManager.Users.Include(x => x.Company).Include(ol => ol.Office).Include(lo => lo.UserRoles).ThenInclude(r => r.Role)
                        .Include(lo => lo.Location).FirstOrDefault(x => x.Id.Equals(currUserId)) ?? throw new Exception($"User with Id={currUserId} was not found.");
                var apptypes = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(a => a.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.COQ)));
                var workFlow = (await GetWorkFlow(action, currentUser, coq.Application?.Facility?.VesselTypeId ?? 1, apptypes.Id)) ?? throw new Exception($"Work Flow for this action was not found.");
                var nextProcessingOfficer = (await GetNextStaffCOQ(coqId, action, workFlow, currentUser, delUserId)) ?? throw new Exception($"No processing staff for this action was not found.");

                coq.CurrentDeskId = nextProcessingOfficer.Id;
                coq.Status = workFlow.Status;
                coq.DateModified = DateTime.UtcNow.AddHours(1);
                
                if(action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit)))
                    coq.SubmittedDate = DateTime.UtcNow.AddHours(1);

                if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Approve).ToLower()))
                    message = "COQ processed successfully and moved to the next processing staff";
                else if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Submit).ToLower()))
                    message = "COQ submitted successfully";
                else if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit).ToLower()))
                    message = "COQ re-submitted successfully";
                else
                    message = "COQ has been returned for review";

                await _unitOfWork.CoQ.Update(coq);
                await _unitOfWork.SaveChangesAsync(currentUser.Id);
                nextProcessingOfficer.LastJobDate = DateTime.UtcNow.AddHours(1);
                await _userManager.UpdateAsync(nextProcessingOfficer);
                //save action to history
                await SaveCOQHistory(action, coqId, workFlow, currentUser, nextProcessingOfficer, comment);

                //Generate permit number on final approval
                if (workFlow.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Completed)))
                {
                    var plant = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id.Equals(coq.PlantId));
                    var certificate = await GenerateCOQCertificate(coqId, currentUser.Id, plant.CompanyElpsId.ToString());

                    //add CoQId to CoQReference rcord
                    var coqReference = await _unitOfWork.CoQReference.Add(new CoQReference
                    {
                        DepotCoQId = coqId,
                    });

                    await _unitOfWork.CoQReference.Add(coqReference);
                    await _unitOfWork.SaveChangesAsync(currentUser.Id);

                    //generate debitnote
                    var debitNote = await _paymentService.GenerateDebitNote(coqReference.Id);

                    //send debit note to SAP
                    var notifySAP = await SendDebitNoteToSAP((Payment)debitNote.Data, plant, coq);

                    if (certificate.Item1)
                        message = $"COQ Application has been approved and certificate {certificate.Item2} has been generated successfully.";
                }
                //send and save notification
                await SendCOQNotification(coq, action, nextProcessingOfficer, message);
                return (true, message);
                
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        private async Task<(bool, Payment)> SendDebitNoteToSAP(Payment debitNote, Plant plant, CoQ coq = null, ProcessingPlantCOQ plantCoQ = null)
        {
            try
            {
                var debitNoteSAPPostRequest = new DebitNoteSAPRequestDTO();
                if(coq != null)
                    debitNoteSAPPostRequest = await SAPRequestDTO(debitNote, plant, coq);
                else
                    debitNoteSAPPostRequest = await SAPRequestDTO(debitNote, plant, plantCoQ);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/DebitNote/CreateDebitNote")
                {
                    Content = new StringContent(debitNoteSAPPostRequest.Stringify(), Encoding.UTF8, "application/json")
                };
                httpRequest.Headers.Add("X-API-Key", _appSetting.SAPKey);
                var notifySAP = await Utils.Send(_appSetting.SAPBaseUrl, httpRequest);

                if (notifySAP.IsSuccessStatusCode)
                {
                    var content = await notifySAP.Content.ReadAsStringAsync();
                    var response = content.Parse<SAPCreateDNoteResponse>();
                    debitNote.SAPNotifyResponse = content;
                    debitNote.SAPDocumentNo = $"{response.sapDocNum}";
                }

                return (true, debitNote);
            }
            catch(Exception ex)
            {

            }
            return (false, debitNote);
        }

        internal async Task<DebitNoteSAPRequestDTO> SAPRequestDTO(Payment debitNote, Plant plant, CoQ coq)
        {
            var product = await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(ad => ad.Application.Id.Equals(coq.AppId) && ad.DepotId.Equals(plant.Id), "Product");

            return new DebitNoteSAPRequestDTO
            {
                bankAccount = _appSetting.NMDPRAAccount,
                directorate = Enum.GetName(typeof(DirectorateEnum), DirectorateEnum.DSSRI),
                customerAddress = coq.Application.User.Company.Address,
                customerCode = $"{coq.Application.User.ElpsId}",
                customerEmail = coq.Application.User.Email,
                customerName = coq.Application.User.Company.Name,
                documentCurrency = "NGN",
                id = coq.Reference,
                customerState = plant.State,
                debitNoteType = "0.5%",
                location = plant.State,
                paymentAmount = double.Parse(debitNote.Amount.ToString("###.##")),
                postingDate = DateTime.UtcNow.AddHours(1).Date.ToString("yyyy-MM-dd"),
                customerPhoneNumber1 = coq.Application.User.PhoneNumber,
                lines = new List<DebitNoteLine>
                {
                    new DebitNoteLine
                    {
                        appliedFactor = 1,
                        motherVesselName = coq.Application.MotherVessel,
                        daughterVesselName = coq.Application.Facility.Name,
                        depot = plant.Name,
                        directorate = Enum.GetName(typeof(DirectorateEnum), DirectorateEnum.DSSRI),
                        revenueDescription = debitNote?.Description,
                        shoreVolume = product.Product.ProductType.Equals(Enum.GetName(typeof(ProductTypes), ProductTypes.Gas)) ? coq.MT_VAC : coq.GSV,
                        revenueCode = product.Product.RevenueCode,
                        wholeSalePrice = coq.DepotPrice
                    }
                },
                contacts = new List<DebitNoteContact> 
                {
                    new DebitNoteContact
                    {
                        firstName = coq.Application.User.FirstName,
                        lastName = coq.Application.User.LastName,
                        phoneNumber = coq.Application.User.PhoneNumber
                    } 
                }
            };
        }

        internal async Task<DebitNoteSAPRequestDTO> SAPRequestDTO(Payment debitNote, Plant plant, ProcessingPlantCOQ coq)
        {
            var company = await _userManager.Users.Include(c => c.Company).FirstOrDefaultAsync(u => u.ElpsId.Equals(plant.CompanyElpsId));
            return new DebitNoteSAPRequestDTO
            {
                bankAccount = _appSetting.NMDPRAAccount,
                directorate = Enum.GetName(typeof(DirectorateEnum), DirectorateEnum.HPPITI),
                customerAddress = company.Company.Address,
                customerCode = $"{company.ElpsId}",
                customerEmail = company.Email,
                customerName = company.Company.Name,
                documentCurrency = "NGN",
                id = coq.Reference,
                customerState = plant.State,
                debitNoteType = "0.5%",
                location = plant.State,
                paymentAmount = double.Parse(debitNote.Amount.ToString("###.##")),
                postingDate = DateTime.UtcNow.AddHours(1).Date.ToString("yyyy-MM-dd"),
                customerPhoneNumber1 = company.PhoneNumber,
                lines = new List<DebitNoteLine>
                {
                    new DebitNoteLine
                    {
                        appliedFactor = 1,
                        depot = plant.Name,
                        directorate = Enum.GetName(typeof(DirectorateEnum), DirectorateEnum.HPPITI),
                        revenueDescription = debitNote.Description,
                        shoreVolume = coq.TotalMTVac.Value,
                        revenueCode = coq.Product.RevenueCode,
                        wholeSalePrice = coq.Price
                    }
                }
            };
        }

        public async Task<(bool, string)> PPCoqWorkFlow(int coqId, string action, string comment, string currUserId = null, string delUserId = null)
        {
            try
            {
                var isProcessingPlant = false;
                //return (false, $"Application with Id={coq.AppId} was not found.");
                var coq = await _unitOfWork.ProcessingPlantCoQ.FirstOrDefaultAsync(x => x.ProcessingPlantCOQId.Equals(coqId), "Product") ?? throw new Exception($"Processing COQ with Id={coqId} was not found.");

                var message = string.Empty;

                currUserId = string.IsNullOrEmpty(currUserId) ? coq.CurrentDeskId : currUserId;
                var currentUser = _userManager.Users
                        .Include(x => x.Company)
                        .Include(ol => ol.Office)
                        .Include(lo => lo.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Include(lo => lo.Location)
                        .FirstOrDefault(x => x.Id.Equals(currUserId)) ?? throw new Exception($"User with Id={currUserId} was not found.");

                var applicationType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals("COQ"));
                var vesselType = await _unitOfWork.VesselType.FirstOrDefaultAsync(x => x.Name.Equals("Vessel"));

                var workFlow = (await GetWorkFlow(action, currentUser, vesselType.Id, applicationType.Id)) ?? throw new Exception($"Work Flow for this action was not found.");
                var nextProcessingOfficer = (await GetNextStaffCOQPP(coqId, action, workFlow, currentUser, delUserId)) ?? throw new Exception($"No processing staff for this action was not found.");

                coq.CurrentDeskId = nextProcessingOfficer.Id;
                coq.Status = workFlow.Status;
                coq.DateModified = DateTime.UtcNow.AddHours(1);

                if (action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit)))
                    coq.SubmittedDate = DateTime.UtcNow.AddHours(1);

                if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Approve).ToLower()))
                    message = "COQ processed successfully and moved to the next processing staff";
                else if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Submit).ToLower()))
                    message = "COQ submitted successfully";
                else if (action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit).ToLower()))
                    message = "COQ re-submitted successfully";
                else
                    message = "COQ has been returned for review";

                await _unitOfWork.ProcessingPlantCoQ.Update(coq);
                await _unitOfWork.SaveChangesAsync(currentUser.Id);
                nextProcessingOfficer.LastJobDate = DateTime.UtcNow.AddHours(1);
                await _userManager.UpdateAsync(nextProcessingOfficer);

                //save action to history
                await SavePPCOQHistory(action, coqId, workFlow, currentUser, nextProcessingOfficer, comment);

                //Generate permit number on final approval
                if (workFlow.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Completed)))
                {
                    var plant = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id.Equals(coq.PlantId));
                    var certificate = await GenerateCOQCertificate(coqId, currentUser.Id, plant.CompanyElpsId.ToString());
                    //var debitnote = await _paymentService.GenerateDebitNote(coq.Id);

                    //add CoQId to CoQReference rcord
                    var coqReference = await _unitOfWork.CoQReference.Add(new CoQReference
                    {
                        PlantCoQId = coqId,
                    });

                    await _unitOfWork.CoQReference.Add(coqReference);
                    await _unitOfWork.SaveChangesAsync(currentUser.Id);

                    //generate debitnote
                    var debitNote = await _paymentService.GenerateDebitNote(coqReference.Id);

                    //send debit note to SAP
                    var notifySAP = await SendDebitNoteToSAP((Payment)debitNote.Data, plant, null, coq);

                    if (certificate.Item1)
                        message = $"COQ Application has been approved and certificate {certificate.Item2} has been generated successfully.";

                    if (notifySAP.Item1)
                    {
                        message += "Debitnote has been generated and SAP notified accordingly.";
                        await _unitOfWork.Payment.Update(notifySAP.Item2);
                        await _unitOfWork.SaveChangesAsync(currentUser.Id);
                    }
                }
                //send and save notification
                //await SendCOQNotification(coq, action, nextProcessingOfficer, message);
                return (true, message);

            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<WorkFlow> GetWorkFlow(string action, ApplicationUser currentuser, int VesselTypeId, int ApplicationTypeId)
        => action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Submit).ToLower()) || action.ToLower().Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit).ToLower())
            ? await _unitOfWork.Workflow.FirstOrDefaultAsync(x => x.Action.ToLower().Trim().Equals(action.ToLower().Trim())
                    && currentuser.UserRoles.FirstOrDefault().Role.Id.ToLower().Trim().Equals(x.TriggeredByRole.ToLower().Trim())
                    && x.ApplicationTypeId.Equals(ApplicationTypeId))
            : await _unitOfWork.Workflow.FirstOrDefaultAsync(x => x.Action.ToLower().Trim().Equals(action.ToLower().Trim())
                    && currentuser.UserRoles.FirstOrDefault().Role.Id.ToLower().Trim().Equals(x.TriggeredByRole.ToLower().Trim())
                    && currentuser.LocationId == x.FromLocationId && x.VesselTypeId == VesselTypeId
                    && x.ApplicationTypeId.Equals(ApplicationTypeId));

        public async Task<bool> SaveHistory(string action, int appid, WorkFlow flow, ApplicationUser user, ApplicationUser nextUser, string comment = null)
        {
            await _unitOfWork.ApplicationHistory.Add(new ApplicationHistory
            {
                Action = action,
                Date = DateTime.Now.AddHours(1),
                ApplicationId = appid,
                TargetedTo = nextUser.Id,
                TargetRole = nextUser.UserRoles.Where(x => !x.Role.Id.Equals("Staff")).FirstOrDefault().Role.Id,
                TriggeredBy = user.Id,
                TriggeredByRole = user.UserRoles.Where(x => !x.Role.Name.Equals("Staff")).FirstOrDefault().Role.Id,
                Comment = comment
            });
            var res = await _unitOfWork.SaveChangesAsync(user.Id);
            return res > 0;
        }

        public async Task<bool> SaveCOQHistory(string action, int coqId, WorkFlow flow, ApplicationUser user, ApplicationUser nextUser, string comment)
        {
            await _unitOfWork.COQHistory.Add(new COQHistory
            {
                Action = action,
                Date = DateTime.Now.AddHours(1),
                COQId = coqId,
                TargetedTo = nextUser.Id,
                TargetRole = nextUser.UserRoles.Where(x => !x.Role.Id.Equals("Staff")).FirstOrDefault().Role.Id,
                TriggeredBy = user.Id,
                TriggeredByRole = user.UserRoles.Where(x => !x.Role.Name.Equals("Staff")).FirstOrDefault().Role.Id,
                Comment = comment
            });
            var res = await _unitOfWork.SaveChangesAsync(user.Id);
            return res > 0;
        }

        public async Task<bool> SavePPCOQHistory(string action, int coqId, WorkFlow flow, ApplicationUser user, ApplicationUser nextUser, string comment)
        {
            await _unitOfWork.PPCOQHistory.Add(new PPCOQHistory
            {
                Action = action,
                Date = DateTime.Now.AddHours(1),
                COQId = coqId,
                TargetedTo = nextUser.Id,
                TargetRole = nextUser.UserRoles.Where(x => !x.Role.Id.Equals("Staff")).FirstOrDefault().Role.Id,
                TriggeredBy = user.Id,
                TriggeredByRole = user.UserRoles.Where(x => !x.Role.Name.Equals("Staff")).FirstOrDefault().Role.Id,
                Comment = comment
            });
            var res = await _unitOfWork.SaveChangesAsync(user.Id);
            return res > 0;
        }

        public async Task<ApplicationUser> GetNextStaff(int appid, string action, WorkFlow wkflow, ApplicationUser currentUser, string delUserId = null)
        {
            ApplicationUser nextprocessingofficer = null;
            if (!string.IsNullOrEmpty(delUserId))
                return _userManager.Users.Include(x => x.Company)
                    .Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                    .Include(lo => lo.Location).Include(ol => ol.Office)
                    .FirstOrDefault(x => x.Id.Equals(delUserId) && x.IsActive);
            else
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == appid, "User.UserRoles.Role");
                //if (, AppActions.Submit)) || action.Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit)) && wkflow != null)
                //{

                //}
                if (action.Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit)) || action.Equals(Enum.GetName(typeof(AppActions), AppActions.Reject)))
                {
                    var historylist = await _unitOfWork.ApplicationHistory.Find(x => x.ApplicationId == appid
                                        && currentUser.UserRoles.FirstOrDefault().Role.Id.Equals(x.TargetRole)
                                        && x.TargetedTo.Equals(currentUser.Id)
                                        && x.TriggeredByRole.Equals(wkflow.TargetRole));

                    //if (action.Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit)))
                    //	historylist = await _unitOfWork.ApplicationHistory.Find(x => x.ApplicationId == appid
                    //					   && currentUser.UserRoles.FirstOrDefault().Role.Id.Equals(x.TriggeredByRole)
                    //					   && x.Action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit))
                    //					   && x.TriggeredByRole.Equals(wkflow.TriggeredByRole));

                    var history = historylist.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (history != null)
                    {
                        nextprocessingofficer = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).Include(lo => lo.Location).Include(ol => ol.Office)
                                                    .FirstOrDefault(x => x.Id.Equals(history.TriggeredBy) && x.UserRoles.FirstOrDefault().Role.Id.Equals(wkflow.TargetRole));
                        if (nextprocessingofficer != null && !nextprocessingofficer.IsActive)
                        {
                            var users = _userManager.Users
                                            .Include(x => x.Company).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).Include(lo => lo.Location).Include(ol => ol.Office)
                                            .Where(x => x.UserRoles.Where(y => y.Role.Id.Equals(wkflow.TargetRole)) != null
                                            && x.IsActive).ToList();
                            nextprocessingofficer = users.OrderBy(x => x.LastJobDate).FirstOrDefault();
                        }
                    }
                }
                if (wkflow != null && nextprocessingofficer == null)
                {
                    if (wkflow.TargetRole.Equals(currentUser.UserRoles.FirstOrDefault().Role.Id))
                        nextprocessingofficer = currentUser;
                    else if (wkflow.TargetRole.Equals(app.User.UserRoles.FirstOrDefault().Role.Id))
                        nextprocessingofficer = app.User;
                    else
                    {
                        var users = !action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit))
                            ? _userManager.Users.Include(x => x.Company).Include(f => f.Company).Include(ur => ur.UserRoles)
                                .ThenInclude(r => r.Role)
                                .Include(lo => lo.Location)
                                .Include(ol => ol.Office)
                                .Where(x => x.UserRoles.Any(y => y.Role.Id.ToLower().Trim().Equals(wkflow.TargetRole.ToLower().Trim()))
                                && x.LocationId == wkflow.ToLocationId && x.IsActive && x.OfficeId == currentUser.OfficeId).ToList()
                            : _userManager.Users.Include(x => x.Company).Include(f => f.Company).Include(ur => ur.UserRoles)
                                .ThenInclude(r => r.Role)
                                .Include(lo => lo.Location)
                                .Include(ol => ol.Office)
                                .Where(x => x.UserRoles.Any(y => y.Role.Id.ToLower().Trim().Equals(wkflow.TargetRole.ToLower().Trim()))
                                && x.LocationId == wkflow.ToLocationId && x.IsActive).ToList();
                        nextprocessingofficer = users.OrderBy(x => x.LastJobDate).FirstOrDefault()!;
                        //foreach (var user in users)
                        //{
                        //    if (!user.UserRoles.Any(c => c.Role.Name == RoleConstants.COMPANY) && user.LocationId != wkflow.ToLocationId)
                        //    {
                        //        users.Remove(user);
                        //    }
                        //}
                    }
                }
                return nextprocessingofficer!;
            }
        }

        public async Task<ApplicationUser> GetNextStaffCOQ(int coqId, string action, WorkFlow wkflow, ApplicationUser currentUser, string delUserId = null)
        {
            ApplicationUser nextprocessingofficer = null;
            if (!string.IsNullOrEmpty(delUserId))
                return _userManager.Users.Include(x => x.Company)
                    .Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                    .Include(lo => lo.Location).Include(ol => ol.Office)
                    .FirstOrDefault(x => x.Id.Equals(delUserId) && x.IsActive)!;
            else
            {
                // var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == appid, "User.UserRoles.Role");
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id.Equals(coqId), "Application.User.UserRoles.Role") ?? throw new Exception($"Unable to find COQ with ID={coqId}.");

                if (action.Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit)) || action.Equals(Enum.GetName(typeof(AppActions), AppActions.Reject)))
                {
                    var historylist = await _unitOfWork.COQHistory.Find(x => x.COQId == coqId
                                        && currentUser.UserRoles.FirstOrDefault().Role.Id.Equals(x.TargetRole)
                                        && x.TargetedTo.Equals(currentUser.Id)
                                        && x.TriggeredByRole.Equals(wkflow.TargetRole));

                    var history = historylist.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (history != null)
                    {
                        nextprocessingofficer = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).Include(lo => lo.Location).Include(ol => ol.Office)
                                                    .FirstOrDefault(x => x.Id.Equals(history.TriggeredBy))!;
                        if (nextprocessingofficer != null && !nextprocessingofficer.IsActive)
                        {
                            var users = _userManager.Users
                                            .Include(x => x.Company).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).Include(lo => lo.Location).Include(ol => ol.Office)
                                            .Where(x => x.UserRoles.Where(y => y.Role.Id.Equals(wkflow.TargetRole)) != null
                                            && x.IsActive).ToList();
                            nextprocessingofficer = users.OrderBy(x => x.LastJobDate).FirstOrDefault();
                        }
                    }
                }
                if (wkflow != null && nextprocessingofficer == null)
                {
                    if (wkflow.TargetRole.Equals(currentUser.UserRoles.FirstOrDefault().Role.Id))
                        nextprocessingofficer = currentUser;
                    else if (wkflow.TargetRole.Equals(coq.Application.User.UserRoles.FirstOrDefault().Role.Id))
                        nextprocessingofficer = coq.Application.User;
                    else
                    {
                        var users = !action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit))
                            ? _userManager.Users.Include(x => x.Company).Include(f => f.Company).Include(ur => ur.UserRoles)
                                .ThenInclude(r => r.Role)
                                .Include(lo => lo.Location)
                                .Include(ol => ol.Office)
                                .Where(x => x.UserRoles.Any(y => y.Role.Id.ToLower().Trim().Equals(wkflow.TargetRole.ToLower().Trim()))
                                && x.IsActive && x.OfficeId == currentUser.OfficeId).ToList()
                            : _userManager.Users.Include(x => x.Company).Include(f => f.Company).Include(ur => ur.UserRoles)
                                .ThenInclude(r => r.Role)
                                .Include(lo => lo.Location)
                                .Include(ol => ol.Office)
                                .Where(x => x.UserRoles.Any(y => y.Role.Id.ToLower().Trim().Equals(wkflow.TargetRole.ToLower().Trim()))
                                && x.IsActive).ToList();

                        nextprocessingofficer = users.OrderBy(x => x.LastJobDate).FirstOrDefault();
                    }
                }
                return nextprocessingofficer;
            }
        }

        public async Task<ApplicationUser> GetNextStaffCOQPP(int coqId, string action, WorkFlow wkflow, ApplicationUser currentUser, string delUserId = null)
        {
            ApplicationUser nextprocessingofficer = null;
            if (!string.IsNullOrEmpty(delUserId))
                return _userManager.Users.Include(x => x.Company)
                    .Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                    .Include(lo => lo.Location).Include(ol => ol.Office)
                    .FirstOrDefault(x => x.Id.Equals(delUserId) && x.IsActive)!;
            else
            {
                // var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == appid, "User.UserRoles.Role");
                var coq = await _unitOfWork.ProcessingPlantCoQ.FirstOrDefaultAsync(x => x.ProcessingPlantCOQId.Equals(coqId)) ?? throw new Exception($"Unable to find COQ with ID={coqId}.");

                if (action.Equals(Enum.GetName(typeof(AppActions), AppActions.Resubmit)) || action.Equals(Enum.GetName(typeof(AppActions), AppActions.Reject)))
                {
                    var historylist = await _unitOfWork.PPCOQHistory.Find(x => x.COQId == coqId
                                        && currentUser.UserRoles.FirstOrDefault().Role.Id.Equals(x.TargetRole)
                                        && x.TargetedTo.Equals(currentUser.Id)
                                        && x.TriggeredByRole.Equals(wkflow.TargetRole));

                    var history = historylist.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (history != null)
                    {
                        nextprocessingofficer = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).Include(lo => lo.Location).Include(ol => ol.Office)
                                                    .FirstOrDefault(x => x.Id.Equals(history.TriggeredBy))!;
                        if (nextprocessingofficer != null && !nextprocessingofficer.IsActive)
                        {
                            var users = _userManager.Users
                                            .Include(x => x.Company).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).Include(lo => lo.Location).Include(ol => ol.Office)
                                            .Where(x => x.UserRoles.Where(y => y.Role.Id.Equals(wkflow.TargetRole)) != null
                                            && x.IsActive).ToList();
                            nextprocessingofficer = users.OrderBy(x => x.LastJobDate).FirstOrDefault();
                        }
                    }
                }
                if (wkflow != null && nextprocessingofficer == null)
                {
                    if (wkflow.TargetRole.Equals(currentUser.UserRoles.FirstOrDefault().Role.Id))
                        nextprocessingofficer = currentUser;
                    // else if (wkflow.TargetRole.Equals(app.User.UserRoles.FirstOrDefault().Role.Name))
                    //     nextprocessingofficer = app.User;
                    else
                    {
                        var users = !action.Equals(Enum.GetName(typeof(AppActions), AppActions.Submit))
                            ? _userManager.Users.Include(x => x.Company).Include(f => f.Company).Include(ur => ur.UserRoles)
                                .ThenInclude(r => r.Role)
                                .Include(lo => lo.Location)
                                .Include(ol => ol.Office)
                                .Where(x => x.UserRoles.Any(y => y.Role.Id.ToLower().Trim().Equals(wkflow.TargetRole.ToLower().Trim()))
                                && x.IsActive && x.OfficeId == currentUser.OfficeId).ToList()
                            : _userManager.Users.Include(x => x.Company).Include(f => f.Company).Include(ur => ur.UserRoles)
                                .ThenInclude(r => r.Role)
                                .Include(lo => lo.Location)
                                .Include(ol => ol.Office)
                                .Where(x => x.UserRoles.Any(y => y.Role.Id.ToLower().Trim().Equals(wkflow.TargetRole.ToLower().Trim()))
                                && x.IsActive).ToList();

                        foreach (var user in users)
                        {
                            if (!user.UserRoles.Any(c => c.Role.Name == RoleConstants.COMPANY) && user.LocationId != wkflow.ToLocationId)
                            {
                                users.Remove(user);
                            }
                        }
                        nextprocessingofficer = users.OrderBy(x => x.LastJobDate).FirstOrDefault();
                    }
                }
                return nextprocessingofficer;
            }
        }

        internal async Task<(bool, string)> GeneratePermit(int id, string userid)
        {
            var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "Facility.VesselType,ApplicationType");
            //select surveyor for NOA

            if (app != null)
            {
                var year = DateTime.Now.Year.ToString();
                var pno = $"NMDPRA/DSSRI/CVC/{app.ApplicationType.Name.Substring(0, 1).ToUpper()}/{year.Substring(2)}/{app.Id}";
                var qrcode = Utils.GenerateQrCode($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Licenses/ValidateQrCode?id={id}");
                //license.QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length);
                //save permit to elps and portal
                var permit = new Permit
                {
                    ApplicationId = id,
                    ExpireDate = DateTime.UtcNow.AddHours(1).AddMonths(12),
                    IssuedDate = DateTime.Now,
                    PermitNo = pno,
                    Signature = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/wwwroot/assets/fa.png",
                    QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length)
                };

                var req = await Utils.Send(_appSetting.ElpsUrl, new HttpRequestMessage(HttpMethod.Post, $"api/Permits/{app.User.ElpsId}/{_appSetting.AppEmail}/{Utils.GenerateSha512($"{_appSetting.AppEmail}{_appSetting.AppId}")}")
                {
                    Content = new StringContent(new
                    {
                        Permit_No = pno,
                        OrderId = app.Reference,
                        Company_Id = app.User.ElpsId,
                        Date_Issued = permit.IssuedDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                        Date_Expire = permit.ExpireDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                        CategoryName = $"CVC ({app.Facility.VesselType.Name})",
                        Is_Renewed = app.ApplicationType.Name,
                        LicenseId = id,
                        Expired = false
                    }.Stringify(), Encoding.UTF8, "application/json")
                });

                if (req.IsSuccessStatusCode)
                {
                    var content = await req.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        var dic = content.Parse<Dictionary<string, string>>();
                        permit.ElpsId = int.Parse(dic.GetValue("id"));
                        await _unitOfWork.Permit.Add(permit);
                        await _unitOfWork.SaveChangesAsync(userid);

                        return (true, pno);
                    }
                }

            }
            return (false, null);
        }

        internal async Task<(bool, string)> GenerateCOQCertificate(int id, string userid, string CompanyElpsId)
        {
            var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id == id, "Application.ApplicationType,Application.User,Application.Facility.VesselType");
            if (coq != null)
            {
                var year = DateTime.Now.Year.ToString();
                var pno = $"NMDPRA/DSSRI/COQ/{year.Substring(2)}/{coq.Id}";
                var qrcode = Utils.GenerateQrCode($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/License/ValidateQrCode/{id}");
                //license.QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length);
                //save certificate to elps and portal
                var certificate = new COQCertificate
                {
                    COQId = id,
                    ExpireDate = DateTime.UtcNow.AddHours(1).AddMonths(12),
                    IssuedDate = DateTime.UtcNow.AddHours(1),
                    CertifcateNo = pno,
                    Signature = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/wwwroot/assets/fa.png",
                    QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length),
                    ProductId = coq.ProductId
                };

                var req = await Utils.Send(_appSetting.ElpsUrl, new HttpRequestMessage(HttpMethod.Post, $"api/Permits/{CompanyElpsId}/{_appSetting.AppEmail}/{Utils.GenerateSha512($"{_appSetting.AppEmail}{_appSetting.AppId}")}")
                {
                    Content = new StringContent(new
                    {
                        Permit_No = pno,
                        OrderId = coq.Reference,
                        Company_Id = CompanyElpsId,
                        Date_Issued = certificate.IssuedDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                        Date_Expire = certificate.ExpireDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                        CategoryName = "COQ",
                        Is_Renewed = coq.Application?.ApplicationType.Name,
                        LicenseId = id,
                        Expired = false
                    }.Stringify(), Encoding.UTF8, "application/json")
                });

                if (req.IsSuccessStatusCode)
                {
                    var content = await req.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        var dic = content.Parse<Dictionary<string, string>>();
                        certificate.ElpsId = int.Parse(dic.GetValue("id"));
                        await _unitOfWork.COQCertificate.Add(certificate);
                        await _unitOfWork.SaveChangesAsync(userid);

                        return (true, pno);
                    }
                }
            }
            return (false, null);
        }

        //internal async Task<(bool, string)> GeneratePPCOQCertificate(int id, string userid, string CompanyElpsId)
        //{
        //    var coq = await _unitOfWork.ProcessingPlantCoQ.FirstOrDefaultAsync(x => x.ProcessingPlantCOQId == id, "Plant,Product");
        //    if (coq != null)
        //    {
        //        var year = DateTime.Now.Year.ToString();
        //        var pno = $"NMDPRA/HPPITI/COQ/{year.Substring(2)}/{coq.ProcessingPlantCOQId}";
        //        var qrcode = Utils.GenerateQrCode($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/License/ValidateQrCode/{id}");
        //        //license.QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length);
        //        //save certificate to elps and portal
        //        var certificate = new PPCOQCertificate
        //        {
        //            COQId = id,
        //            ExpireDate = DateTime.UtcNow.AddHours(1).AddMonths(12),
        //            IssuedDate = DateTime.Now,
        //            CertifcateNo = pno,
        //            Signature = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/wwwroot/assets/fa.png",
        //            QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length),
        //            ProductId = coq.ProductId
        //        };

        //        var req = await Utils.Send(_appSetting.ElpsUrl, new HttpRequestMessage(HttpMethod.Post, $"api/Permits/{CompanyElpsId}/{_appSetting.AppEmail}/{Utils.GenerateSha512($"{_appSetting.AppEmail}{_appSetting.AppId}")}")
        //        {
        //            Content = new StringContent(new
        //            {
        //                Permit_No = pno,
        //                OrderId = coq.Reference,
        //                Company_Id = CompanyElpsId,
        //                Date_Issued = certificate.IssuedDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
        //                Date_Expire = certificate.ExpireDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
        //                CategoryName = "COQ",
        //                Is_Renewed = coq.Application?.ApplicationType.Name,
        //                LicenseId = id,
        //                Expired = false
        //            }.Stringify(), Encoding.UTF8, "application/json")
        //        });

        //        if (req.IsSuccessStatusCode)
        //        {
        //            var content = await req.Content.ReadAsStringAsync();
        //            if (!string.IsNullOrEmpty(content))
        //            {
        //                var dic = content.Parse<Dictionary<string, string>>();
        //                certificate.ElpsId = int.Parse(dic.GetValue("id"));
        //                await _unitOfWork.COQCertificate.Add(certificate);
        //                await _unitOfWork.SaveChangesAsync(userid);

        //                return (true, pno);
        //            }
        //        }
        //    }
        //    return (false, null);
        //}

        public async Task SendNotification(Application app, string action, ApplicationUser user, string comment)
        {
            string content = $"Application with reference {app.Reference} has been submitted to your desk for further processing";
            string subject = $"Application with reference {app.Reference} Submitted";
            switch (action)
            {
                case "Reject":
                    content = $"Application with reference {app.Reference} has been rejected and <br/>returned to your desk for further processing. Below is for your information - <br/>{comment}";
                    break;
                case "Approve":
                    content = $"Application with reference {app.Reference} has been endorsed and <br/> move to your desk for further processing";
                    subject = $"Application with reference {app.Reference} pushed for processing";
                    break;
                default:
                    break;
            }
            //send and save notification

            var body = Utils.ReadTextFile(_env.WebRootPath, "GeneralTemplate.txt");
            var url = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host + "/assets/nmdpraLogo.png";
            body = string.Format(body, content, DateTime.Now.Year, url);
            Utils.SendMail(_mailSetting.Stringify().Parse<Dictionary<string, string>>(), user.Email, subject, body);

            //await _unitOfWork.Message.Add(new Message
            //{
            //    ApplicationId = app.Id,
            //    Content = body,
            //    Date = DateTime.Now.AddHours(1),
            //    Subject = subject,
            //    UserId = user.Id
            //});
            //await _unitOfWork.SaveChangesAsync(user.Id);
        }

        public async Task SendCOQNotification(CoQ coq, string action, ApplicationUser user, string comment)
        {
            string content = $"COQ Application with reference {coq.Reference} has been submitted to your desk for further processing";
            string subject = $"COQ Application with reference {coq.Reference} Submitted";
            switch (action)
            {
                case "Reject":
                    content = $"COQ Application with reference {coq.Reference} has been rejected and <br/>returned to your desk for further processing. Below is for your information - <br/>{comment}";
                    break;
                case "Approve":
                    content = $"COQ Application with reference {coq.Reference} has been endorsed and <br/> move to your desk for further processing";
                    subject = $"COQ Application with reference {coq.Reference} pushed for processing";
                    break;
                default:
                    break;
            }
            //send and save notification

            var body = Utils.ReadTextFile(_env.WebRootPath, "GeneralTemplate.txt");
            var url = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host + "/assets/nmdpraLogo.png";
            body = string.Format(body, content, DateTime.Now.Year, url);
            Utils.SendMail(_mailSetting.Stringify().Parse<Dictionary<string, string>>(), user.Email, subject, body);

            //await _unitOfWork.Message.Add(new Message
            //{
            //    COQId = coq.Id,
            //    IsCOQ = true,
            //    Content = body,
            //    Date = DateTime.Now.AddHours(1),
            //    Subject = subject,
            //    UserId = user.Id
            //});
            //await _unitOfWork.SaveChangesAsync(user.Id);
        }

        public string GenerateDischargeID(string product)
        {
            Random random = new Random();
            string _token = random.Next(100001, 999999).ToString();

            return $"{product.ToUpper()}/{_token}";
        }

        public async Task<bool> PostDischargeId(int id)
        {
            var appDepots = await _context.ApplicationDepots.Where(a => a.AppId == id).Include(x => x.Product).ToListAsync();
            foreach (var appDepot in appDepots)
            {
                //appDepot.DischargeId = GenerateDischargeID(appDepot.Product.Name);
                var num = appDepot.Id.ToString("D6");
                appDepot.DischargeId = $"{appDepot.Product.Name}/{num}";
            }

            _context.ApplicationDepots.UpdateRange(appDepots);
            var s = await _context.SaveChangesAsync();
            if (s > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
