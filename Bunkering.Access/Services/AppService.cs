﻿using AutoMapper;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Drawing.Text;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Bunkering.Access.Query;
using System.ComponentModel.Design;
using System.Collections.Generic;

namespace Bunkering.Access.Services
{
    public class AppService
    {
        private readonly IElps _elps;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly WorkFlowService _flow;
        private readonly IMapper _mapper;
        ApiResponse _response;
        private string User;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppLogger _logger;
        private readonly AppSetting _setting;
        private readonly string directory = "Application";
        private readonly IConfiguration _configuration;
        private readonly ApplicationQueries _appQueries;
        private readonly MessageService _messageService;
        private readonly ApplicationContext _context;

        public AppService(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            WorkFlowService flow,
            IMapper mapper,
            IElps elps,
            IHttpContextAccessor httpContextAccessor,
            AppLogger logger,
            IOptions<AppSetting> setting,
            IConfiguration configuration,
            ApplicationQueries appQueries,
            MessageService messageService,
            ApplicationContext context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _flow = flow;
            _mapper = mapper;
            _elps = elps;
            _httpContextAccessor = httpContextAccessor;
            User = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _logger = logger;
            _setting = setting.Value;
            _configuration = configuration;
            _appQueries = appQueries;
            _messageService = messageService;
            _context = context;
        }

        private async Task<Facility> CreateFacility(ApplictionViewModel model, ApplicationUser user)
        {
            try
            {
                var facility = await _unitOfWork.Facility.FirstOrDefaultAsync(f => f.IMONumber.ToLower().Equals(model.IMONumber.ToLower()));
                if(facility == null)
                {
                    facility = new Facility
                    {
                        Name = model.VesselName,
                        CompanyId = user.CompanyId.Value,
                        VesselTypeId = model.VesselTypeId,
                        IMONumber = model.IMONumber
                    };
                    var lga = await _unitOfWork.LGA.FirstOrDefaultAsync(x => x.State.Name.ToLower().Contains("lagos"), "State");

                    var facElps = _elps.CreateElpsFacility(new
                    {
                        Name = model.VesselName,
                        StreetAddress = $"{model.VesselName} - {user.ElpsId}",
                        CompanyId = user.ElpsId,
                        DateAdded = DateTime.UtcNow.AddHours(1),
                        City = lga.Name,
                        lga.StateId,
                        LGAId = lga.Id,
                        FacilityType = "CVC Facility",
                        FacilityDocuments = (string)null,
                        Id = (string)null
                    })?.Parse<Dictionary<string, object>>();

                    if (facElps?.Count > 0)
                        facility.ElpsId = int.Parse($"{facElps.GetValue("id")}");

                    await _unitOfWork.Facility.Add(facility);
                    await _unitOfWork.SaveChangesAsync(user.Id);
                }
                return facility;
            }
            catch (Exception ex)
            {
                _logger.LogRequest($"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}", true, directory);
            }
            return null;
        }

        public async Task<ApiResponse> Apply(ApplictionViewModel model)
        {
            try
            {
                //querying the database to retrieve a user along with their associated roles
                var user = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(x => x.Email.Equals(User));

                if (user == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "User is not authorised",
                        Success = false
                    };
                }
                // get app configuration for notice of arrival
                var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Utils.NOA));

                if (appType == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "NOA not configured as application type, please contact support",
                        Success = false
                    };
                }

                //var surveyor = await _unitOfWork.NominatedSurveyor.GetNextAsync();
                //if (surveyor is null)
                //{
                //    return new ApiResponse
                //    {
                //        StatusCode = HttpStatusCode.NotFound,
                //        Message = "Surveyors not configured, please contact support",
                //        Success = false
                //    };
                //}

                //var user = _userManager.Users.Include(c => c.Company).FirstOrDefault(x => x.Email.ToLower().Equals(User.Identity.Name));
                //if ((await _unitOfWork.Application.Find(x => x.Facility.VesselTypeId.Equals(model.VesselTypeId) && x.UserId.Equals(user.Id))).Any())
                //    _response = new ApiResponse
                //    {
                //        Message = "There is an existing application for this facility, you are not allowed to use license the same vessel twice",
                //        StatusCode = HttpStatusCode.Found,
                //        Success = false
                //    };
                //else
                //{

                var facility = await CreateFacility(model, user);
                if (facility != null)
                {
                    var app = new Application
                    {
                        ApplicationTypeId = appType.Id,
                        CreatedDate = DateTime.UtcNow.AddHours(1),
                        CurrentDeskId = user.Id,
                        Reference = Utils.RefrenceCode(),
                        UserId = user.Id,
                        FacilityId = facility.Id,
                        Status = Enum.GetName(typeof(AppStatus), AppStatus.Initiated),
                        VesselName = model.VesselName,
                        LoadingPort = model.LoadingPort,
                        MarketerName = model.MarketerName,
                        //IMONumber = model.IMONumber,
                        MotherVessel = model.MotherVessel,
                        Jetty = model.Jetty,
                        ETA = model.ETA,

                    };

                    var newApp = await _unitOfWork.Application.Add(app);
                    var volume = model.DepotList.Sum(c => c.Volume);
                    await _unitOfWork.SaveChangesAsync(app.UserId);
                    //var depot = await AppDepots(model.DepotList, app.Id);
                    var depotList = new List<ApplicationDepot>();
                    if (model.DepotList.Any() && newApp != null)
                    {
                        model.DepotList.ForEach(d =>
                        {
                            depotList.Add(new ApplicationDepot
                            {
                                AppId = app.Id,
                                DepotId = d.DepotId,
                                DischargeId = "",
                                ProductId = d.ProductId,
                                Volume = d.Volume
                            });
                        });
                        //var depotList = _mapper.Map<List<ApplicationDepot>>(model.DepotList);
                        await _unitOfWork.ApplicationDepot.AddRange(depotList);
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }
                    else
                        throw new Exception("Depot List must be provided!");

                    //surveyor.NominatedVolume += volume;
                    //var appSurveyor = new ApplicationSurveyor()
                    //{
                    //    ApplicationId = newApp.Id,
                    //    NominatedSurveyorId = surveyor.Id,
                    //    Volume = volume
                    //};
                    //await _unitOfWork.ApplicationSurveyor.Add(appSurveyor);
                    //await _unitOfWork.SaveChangesAsync(user.Id);
                    return new ApiResponse
                    {
                        Message = "Application initiated successfully",
                        StatusCode = HttpStatusCode.OK,
                        Data = new { appId = app.Id },
                        Success = true
                    };

                    //save app tanks
                    // var tank = await AppTanks(model.TankList, facility.Id);

                    // if (tank != null)
                    // {
                    //     await _unitOfWork.Application.Add(app);
                    //     await _unitOfWork.SaveChangesAsync(app.UserId);

                    //     _response = new ApiResponse
                    //     {
                    //         Message = "Application initiated successfully",
                    //         StatusCode = HttpStatusCode.OK,
                    //         Data = new { appId = app.Id },
                    //         Success = true
                    //     };

                    // }
                    // else
                    // {
                    //     _response = new ApiResponse
                    //     {
                    //         Message = "unable to apply",
                    //         StatusCode = HttpStatusCode.NotFound,
                    //         Success = false
                    //     };

                    // }


                    //await _flow.AppWorkFlow(app.Id, Enum.GetName(typeof(AppActions), AppActions.Initiate), "Application Created");
                }
                else
                {
                    return new ApiResponse
                    {
                        Message = "An error occured. Pls try again.",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        //save tank information
        private async Task<List<Tank>> AppTanks(List<TankViewModel> tank, int facilityId)
        {
            var tanks = await _unitOfWork.Tank.Find(x => x.FacilityId == facilityId);

            if (tanks.Count() > 0)
            {
                await _unitOfWork.Tank.RemoveRange(tanks.ToList());
                await _unitOfWork.SaveChangesAsync("");
            }

            var tankList = new List<Tank>();
            tank.ForEach(x =>
            {
                tankList.Add(new Tank
                {
                    Capacity = x.Capacity,
                    FacilityId = facilityId,
                    Name = x.Name,
                    ProductId = x.ProductId,
                });
            });

            if (tankList.Count > 0)
            {
                await _unitOfWork.Tank.AddRange(tankList);
                await _unitOfWork.SaveChangesAsync("");
            }

            return tankList;

        }
        // private Task<List<Depot>> AppDepots(List<Depot> depot, int appId)
        // {

        //     var depotList = new List<ApplicationDepot>();
        //     depot.ForEach(x =>
        //     {
        //         depotList.Add(new ApplicationDepot
        //         {
        //             AppId = appId,
        //             DepotId = x.Id
        //         });
        //     });

        //      return depotList;

        // }

        public async Task<ApiResponse> GetDepotsByAppId(int id)
        {
            if (id > 0)
            {
                var appDepots = await _unitOfWork.ApplicationDepot.GetDepotsAsync(id);
                return new()
                {
                    Success = true,
                    Message = "Depots fetched successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = appDepots
                };
            }
            return new()
            {
                Success = false,
                Message = "Invalid application ID",
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        public async Task<ApiResponse> GetTanksByAppId(int id)
        {
            List<TankViewModel> tankList = new List<TankViewModel>();

            if (id > 0)
            {
                try
                {
                    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(id), "Facility.VesselType");
                    if (app != null)
                    {
                        if (app.Facility.Name.Equals("Fixed"))
                        {
                            var licenseResponse = (await _unitOfWork.ValidatiionResponse.Find(x => x.UserId.Equals(app.UserId))).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (licenseResponse != null)
                            {
                                var dic = licenseResponse.Response.Parse<Dictionary<string, object>>();
                                var tankObj = dic.GetValue("Tanks");
                                if (tankObj != null)
                                {
                                    var tanks = tankObj.Stringify().Parse<List<Dictionary<string, string>>>();

                                    foreach (var t in tanks)
                                    {
                                        tankList.Add(new TankViewModel
                                        {
                                            Capacity = decimal.Parse(t.GetValue("Capacity")),
                                            Name = t.GetValue("Name"),
                                            FacilityId = app.FacilityId
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            var tanks = await _unitOfWork.Tank.Find(x => x.FacilityId.Equals(app.FacilityId));
                            if (tanks.Count() > 0)
                                tankList = _mapper.Map<List<TankViewModel>>(tanks);
                        }
                        _response = new ApiResponse
                        {
                            Message = "Success",
                            StatusCode = HttpStatusCode.OK,
                            Success = true,
                            Data = new
                            {
                                AppId = id,
                                Tanks = tankList.Stringify()
                            }
                        };
                    }
                    else
                        _response = new ApiResponse
                        {
                            Success = false,
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "Application not found"
                        };
                }
                catch (Exception ex)
                {
                    _response = new ApiResponse
                    {
                        Message = ex.Message,
                        StatusCode = HttpStatusCode.InternalServerError,
                        Success = false,
                    };
                }
            }
            else
                _response = new ApiResponse
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "ApplicationID invalid"
                };

            return _response;
        }

        public async Task<ApiResponse> AddTanks(List<TankViewModel> model)
        {
            try
            {
                var app = await _unitOfWork.Facility.FirstOrDefaultAsync(x => x.Id.Equals(model.FirstOrDefault().FacilityId));
                if (app != null)
                {
                    var facid = model.FirstOrDefault().FacilityId;
                    var tankList = await AppTanks(model, facid);
                    if (tankList.Count > 0)
                        _response = new ApiResponse
                        {
                            Success = true,
                            Message = "Tanks/Vessels added successfully",
                            StatusCode = HttpStatusCode.OK,
                            Data = model.FirstOrDefault().FacilityId
                        };
                }
                else
                    _response = new ApiResponse
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "ApplicationID invalid"
                    };
            }
            catch (Exception ex)
            {
                _response = new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }

            return _response;
        }

        public async Task<ApiResponse> Payment(int id)
        {
            if (id > 0)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(User);
                    if (user is null) return new ApiResponse()
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "User not found",
                        Success = false
                    };
                    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(id), "ApplicationType,Facility.VesselType,Payments");
                    var appDepots = (await _unitOfWork.ApplicationDepot.Find(x => x.AppId.Equals(id), "Depot")).Select(c => c.Depot).ToList();
                    if (app is null) return new ApiResponse()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "application not found",
                        Success = false
                    };
                    
                    var fee = await _unitOfWork.AppFee.FirstOrDefaultAsync(x => x.ApplicationTypeId.Equals(app.ApplicationTypeId) && x.IsDeleted != true);
                    if (fee is null) return new ApiResponse()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "App fee not found",
                        Success = false
                    };

                    var total = (double)(fee.COQFee * appDepots.Count) + (double)fee.SerciveCharge + (double)fee.NOAFee;

                    var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(x => x.ApplicationId.Equals(id));
                    if (payment == null)
                    {
                        payment = new Payment
                        {
                            Amount = total,
                            Account = _setting.NMDPRAAccount,
                            ApplicationId = id,
                            OrderId = app.Reference,
                            BankCode = _setting.NMDPRAAccount,
                            Description = $"Payment for CVC & COQ ({app.Facility.Name}) |" +
                            $" NoA Fee: ₦ {fee.NOAFee:#,#.##} |" +
                            $" Depots: {string.Join(", ",appDepots.Select(x => x.Name).ToList())} |" +
                            $" CoQ Fee (per Depot): ₦ {fee.COQFee:#,#.##} |" +
                            $" Total CoQ Fee: ₦ {(fee.COQFee * appDepots.Count):#,#.##} |" +
                            $" Total Amount: ₦ {total:#,#.##}",
                            PaymentType = "NGN",
                            Status = Enum.GetName(typeof(AppStatus), AppStatus.PaymentPending),
                            TransactionDate = DateTime.UtcNow.AddHours(1),
                            ServiceCharge = (double)fee.SerciveCharge,
                            AppReceiptId = "",
                            RRR = "",
                            TransactionId = "",
                            TxnMessage = "Payment initiated",
                        };
                        await _unitOfWork.Payment.Add(payment);
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(payment.RRR))
                        {
                            payment.Amount = total;
                            payment.OrderId = app.Reference;
                            payment.Description = $"Payment for CVC & COQ ({app.Facility.Name})";
                            payment.Status = Enum.GetName(typeof(AppStatus), AppStatus.PaymentPending);
                            payment.TransactionDate = DateTime.UtcNow.AddHours(1);

                            await _unitOfWork.Payment.Update(payment);
                            await _unitOfWork.SaveChangesAsync(user.Id);
                        }
                    }

                    _response = new ApiResponse
                    {
                        Message = "Payment generated for application successfully",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                        Data = new
                        {
                            AppReference = app.Reference,
                            VesselName = app.VesselName,
                            FacilityType = app.Facility.Name,
                            ApplicationType = app.ApplicationType.Name,
                            ApplicationFee = fee.ApplicationFee,
                            fee.COQFee,
                            fee.NOAFee,
                            ServiceCharge = fee.SerciveCharge,
                            Total = total,
                            payment.RRR,
                            PaymentStatus = payment.Status,
                            payment.Description
                        }
                    };
                }
                catch (Exception ex)
                {
                    _response = new ApiResponse
                    {
                        Message = ex.Message,
                        StatusCode = HttpStatusCode.InternalServerError,
                        Success = false
                    };
                }
            }
            else
                _response = new ApiResponse
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "ApplicationID Invalid"
                };

            return _response;
        }

        public async Task<ApiResponse> DocumentUpload(int id)
        {

            if (id > 0)
            {
                var docList = new List<SubmittedDocument>();
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "User,Facility.VesselType,ApplicationType");
                if (app != null)
                {
                    var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(c => c.Name == Utils.NOA);
                    if (appType == null)
                    {
                        return new()
                        {
                            Data = null!,
                            Message = "Application type is not configured",
                            StatusCode = HttpStatusCode.BadRequest,
                            Success = false
                        };
                    }
                    var factypedocs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.ApplicationTypeId.Equals(app.ApplicationTypeId));
                    if (factypedocs != null && factypedocs.Count() > 0)
                    {
                        var compdocs = _elps.GetCompanyDocuments(app.User.ElpsId, "company").Stringify().Parse<List<Document>>();
                        var facdocs = _elps.GetCompanyDocuments(app.Facility.ElpsId, "facility").Stringify().Parse<List<FacilityDocument>>();
                        var appdocs = await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId == id);

                        factypedocs.ToList().ForEach(x =>
                        {
                            if (x.DocType.ToLower().Equals("company"))
                            {
                                if (compdocs != null && compdocs.Count > 0)
                                {

                                    var doc = compdocs.FirstOrDefault(y => int.Parse(y.document_type_id) == x.DocumentTypeId);
                                    if (doc != null)
                                    {
                                        docList.Add(new SubmittedDocument
                                        {
                                            DocId = x.DocumentTypeId,
                                            DocName = x.Name,
                                            DocType = x.DocType,
                                            FileId = doc.id,
                                            DocSource = doc.source,
                                            ApplicationId = id,
                                            ApplicationTypeId = appType.Id
                                        });
                                    }
                                    else
                                    {
                                        docList.Add(new SubmittedDocument
                                        {
                                            DocId = x.DocumentTypeId,
                                            DocName = x.Name,
                                            DocType = x.DocType,
                                            ApplicationTypeId = appType.Id
                                        });
                                    }
                                }
                                else
                                    docList.Add(new SubmittedDocument
                                    {
                                        DocId = x.DocumentTypeId,
                                        DocName = x.Name,
                                        DocType = x.DocType,
                                        ApplicationTypeId = appType.Id
                                    });
                            }
                            else
                            {
                                if (facdocs != null && facdocs.Count > 0)
                                {
                                    var doc = facdocs.FirstOrDefault(y => y.Document_Type_Id == x.DocumentTypeId);
                                    if (doc != null)
                                    {
                                        docList.Add(new SubmittedDocument
                                        {
                                            DocId = x.DocumentTypeId,
                                            DocName = x.Name,
                                            DocType = x.DocType,
                                            FileId = doc.Id,
                                            DocSource = doc.Source,
                                            ApplicationId = id,
                                            ApplicationTypeId = appType.Id
                                        });
                                    }
                                    else
                                    {
                                        docList.Add(new SubmittedDocument
                                        {
                                            DocId = x.DocumentTypeId,
                                            DocName = x.Name,
                                            DocType = x.DocType,
                                            ApplicationTypeId = appType.Id
                                        });
                                    }
                                }
                                else
                                    docList.Add(new SubmittedDocument
                                    {
                                        DocId = x.DocumentTypeId,
                                        DocName = x.Name,
                                        DocType = x.DocType,
                                        ApplicationTypeId = appType.Id
                                    });
                            }
                        });
                    }
                    _response = new ApiResponse
                    {
                        Message = "Facility Type Documents fetched",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                        Data = new
                        {
                            Docs = docList,
                            ApiData = new
                            {
                                CompanyElpsId = app.User.ElpsId,
                                FacilityElpsId = app.Facility.ElpsId,
                                ApiEmail = _setting.AppEmail,
                                ApiHash = $"{_setting.AppEmail}{_setting.AppId}".GenerateSha512()
                            }
                        }
                    };
                }
                else
                    _response = new ApiResponse
                    {
                        Message = "ApplicationID invalid",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
            }
            else
                _response = new ApiResponse
                {
                    Message = "ApplicationID invalid",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return _response;
        }

        public async Task<ApiResponse> AddDocuments(int id)
        {

            if (id > 0)
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "Facility.VesselType");
                var user = await _userManager.FindByEmailAsync(User);
                if (app != null)
                {
                    var facTypeDocs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.ApplicationTypeId.Equals(app.ApplicationTypeId) && x.VesselTypeId.Equals(app.Facility.VesselTypeId));

                    if (facTypeDocs.Count() > 0)
                    {
                        var compdocs = (List<Document>)_elps.GetCompanyDocuments(user.ElpsId);
                        var facdocs = (List<FacilityDocument>)_elps.GetCompanyDocuments(app.Facility.ElpsId, "facility");
                        var docs = new List<SubmittedDocument>();

                        foreach (var item in facTypeDocs.ToList())
                        {
                            if (item.DocType.ToLower().Equals("company"))
                            {
                                var doc = compdocs.FirstOrDefault(x => int.Parse(x.document_type_id) == item.DocumentTypeId);
                                if (doc != null)
                                    docs.Add(new SubmittedDocument
                                    {
                                        ApplicationId = app.Id,
                                        DocId = item.DocumentTypeId,
                                        DocName = item.Name,
                                        DocSource = doc.source,
                                        DocType = item.DocType,
                                        FileId = doc.id,
                                        ApplicationTypeId = app.ApplicationTypeId,
                                    });
                            }
                            else
                            {
                                var doc = facdocs.FirstOrDefault(x => x.Document_Type_Id == item.DocumentTypeId);
                                if (doc != null)
                                    docs.Add(new SubmittedDocument
                                    {
                                        ApplicationId = app.Id,
                                        DocId = item.DocumentTypeId,
                                        DocName = item.Name,
                                        DocSource = doc.Source,
                                        DocType = item.DocType,
                                        FileId = doc.Id,
                                        ApplicationTypeId = app.ApplicationTypeId,
                                    });
                            }
                        }

                        if (docs.Count > 0)
                        {
                            var appdocs = (await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId.Equals(app.Id))).ToList();
                            if (appdocs.Count() > 0)
                                await _unitOfWork.SubmittedDocument.RemoveRange(appdocs);

                            await _unitOfWork.SubmittedDocument.AddRange(docs);
                            await _unitOfWork.SaveChangesAsync(user.Id);
                        }
                    }

                    var submit = app.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentRejected)) || app.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Rejected))
                        ? await _flow.AppWorkFlow(id, Enum.GetName(typeof(AppActions), AppActions.Resubmit), "Application re-submitted")
                        : await _flow.AppWorkFlow(id, Enum.GetName(typeof(AppActions), AppActions.Submit), "Application Submitted");
                    if (submit.Item1)
                    {
                        var message = new MessageModel
                        {
                            ApplicationId = app.Id,
                            Subject = $"Application with reference {app.Reference} Submitted",
                            Content = $"Application with reference {app.Reference} has been submitted to your desk for further processing",
                            UserId = user.Id,
                        };

                        _messageService.CreateMessageAsync(message);

                        _response = new ApiResponse
                        {
                            Message = submit.Item2,
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Message = submit.Item2,
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Success = false
                        };

                    }
                }
                else
                    _response = new ApiResponse
                    {
                        Message = "ApplicationID invalid",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
            }
            else
                _response = new ApiResponse
                {
                    Message = "Application not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            return _response;
        }

        public async Task<ApiResponse> CheckLicense(string license)
        {
            string baseUrl = _setting.DepotUrl;
            var reqUri = _setting.DepotUri;
            //var factype = await _unitOfWork.FacilityType.Find(x => x.Id.Equals(id));

            try
            {
                //check license on local system
                var lno = await _unitOfWork.ValidatiionResponse.FirstOrDefaultAsync(x => x.LicenseNo.ToLower().Equals(license.ToLower()));
                //var dicResponse = await _unitOfWork.Permit.FirstOrDefaultAsync(y => y.IssuedDate.ToString("MMM dd, yyyy HH:mm:ss") && y.ExpireDate.ToString("MMM dd, yyyy HH:mm:ss"));

                var dicResponse = new LicenseValidationViewModel()
                {
                    Facility_Name = "",
                    License_Number = license.ToLower(),
                    Date_Issued = DateTime.UtcNow,
                    Date_Expired = DateTime.UtcNow.AddHours(1),


                };

                if (lno == null)
                {
                    var resp = await Utils.Send(baseUrl, new HttpRequestMessage(HttpMethod.Get, reqUri));
                    if (resp.IsSuccessStatusCode)
                    {
                        var content = await resp.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                            dicResponse = content.Parse<LicenseValidationViewModel>();
                    }

                    _response = new ApiResponse
                    {
                        Data = dicResponse,
                        Message = "License verified successfully",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                    return _response;
                }
                else
                    dicResponse = lno.Response.Parse<LicenseValidationViewModel>();

                if (dicResponse.Date_Expired <= DateTime.UtcNow.AddHours(1))
                    _response = new ApiResponse
                    {
                        Message = "License not valid",
                        StatusCode = HttpStatusCode.OK,
                        Success = false
                    };

                /*else
				{
					//var data = new
					//{
					//	Company_Name = dicResponse.GetValue("Company_Name"),
					//	Date_Issued = Convert.ToDateTime(dicResponse.GetValue("Date_Issued")).ToString("MMM dd, yyyy HH:mm:ss"),
					//	Date_Expire = expiry.ToString("MMM dd, yyyy HH:mm:ss"),
					//};
					_response = new ApiResponse
					{
						Data = dicResponse,
						Message = "License verified successfully",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				*/
            }
            catch (Exception ex)
            {
                _response = new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
            return _response;
        }

        public async Task<ApiResponse> AllApps()
        {
            var user = await _userManager.FindByEmailAsync(User);

            var apps = await _userManager.IsInRoleAsync(user, "Company")
                ? await _unitOfWork.Application.Find(a => a.UserId.Equals(user.Id), "User.Company,ApplicationType,Facility.VesselType,Payments")
                : await _unitOfWork.Application.GetAll("User.Company,ApplicationType,Facility.VesselType,Payments");

            if (apps.Count() != null)
                _response = new ApiResponse
                {
                    Message = "Applications fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = apps.OrderByDescending(x => x.CreatedDate).Select(x => new
                    {
                        x.Id,
                        CompanyEmail = x.User.Email,
                        CompanyName = x.User.Company.Name,
                        VesselName = x.Facility.Name,
                        VesselType = x.Facility.VesselType.Name,
                        x.Facility.Capacity,
                        x.Reference,
                        x.Status,
                        PaymnetStatus = x.Payments.Count > 0 && x.Payments.FirstOrDefault().Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted))
                        ? "Payment confirmed" : x.Payments.Count > 0 && x.Payments.FirstOrDefault().Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentRejected)) ? "Payment rejected" : "Payment pending",
                        x.Payments.FirstOrDefault()?.RRR,
                        PaymentId = x.Payments?.FirstOrDefault()?.Id,
                        CreatedDate = x.CreatedDate.ToString("MMMM dd, yyyy HH:mm:ss")
                    })
                };
            else
                _response = new ApiResponse
                {
                    Message = "No Application was found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            return _response;
        }

        public async Task<ApiResponse> GetLGAbyStateId(int id)
        {
            var lga = await _unitOfWork.LGA.Find(x => x.StateId == id);
            if (lga != null)
            {
                var data = lga.Select(x => new { Text = x.Name, Value = x.Id.ToString() }).ToList();
                _response = new ApiResponse
                {
                    Message = "LGA list return",
                    StatusCode = HttpStatusCode.OK,
                    Data = lga.Select(x => new { x.Id, x.Name }).ToList(),
                    Success = true
                };
            }
            else
                _response = new ApiResponse
                {
                    Message = "No LGA found for the state",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            return _response;
        }

        public async Task<ApiResponse> MyDesk()
        {
            try
            {
                var user = await _userManager.Users.Include(x => x.Location).Include(ur => ur.UserRoles).ThenInclude(u => u.Role).FirstOrDefaultAsync(x => x.Email.Equals(User));
                if (user.UserRoles.FirstOrDefault().Role.Name.Equals("Company"))
                    return await GetMyDeskOthers(user);
                else if(user.Location?.Name == LOCATION.FO)
                    return await GetMyDeskFAD(user);
                //if (user.Location?.Name == LOCATION.FO && !user.UserRoles.FirstOrDefault().Role.Name.Equals("FAD"))
                //    return await GetMyDeskFO(user);
                //else if (user.UserRoles.FirstOrDefault().Role.Name.Equals("FAD"))
                //    return await GetMyDeskFAD(user);
                else
                    return await GetMyDeskOthers(user);
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = true,
                };
            }
        }

        public async Task<IEnumerable<NavalLetterDto>> GetClearedVessels()
        {
            var apps = (await _unitOfWork.Application.Find(c => c.HasCleared)).ToList();
            var data = new List<NavalLetterDto>();
            foreach (var app in apps)
            {
                var appDepot = (await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.AppId == app.Id, "Depot"));
                var product = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id.Equals(appDepot.ProductId));
                data.Add(new NavalLetterDto(app.MarketerName, product.Name, appDepot.Volume,
                    app.VesselName, app.MotherVessel, app.Jetty, app.ETA, app.LoadingPort, appDepot.Depot.Name
                ));
            }
            return data;
        }

        private async Task<ApiResponse> GetMyDeskOthers(ApplicationUser? user)
        {
            var apps = await _unitOfWork.Application.Find(x => x.CurrentDeskId.Equals(user.Id), "User.Company,Facility.VesselType,ApplicationType,WorkFlow,Payments");
            if (await _userManager.IsInRoleAsync(user, "Company"))
                apps = await _unitOfWork.Application.Find(x => x.UserId.Equals(user.Id) && x.IsDeleted != true, "User.Company,Facility.VesselType,ApplicationType,WorkFlow,Payments");
            return new ApiResponse
            {
                Message = "Applications fetched successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = apps.OrderByDescending(x => x.CreatedDate).Select(x => new
                {
                    x.Id,
                    CompanyEmail = x.User.Email,
                    CompanyName = x.User.Company.Name,
                    VesselName = x.Facility.Name,
                    VesselType = x.Facility.VesselType.Name,
                    x.Reference,
                    x.Status,
                    PaymentStatus = x.Payments.Count != 0 && x.Payments.FirstOrDefault().Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted))
                        ? "Payment confirmed" : x.Payments.Count != 0 && x.Payments.FirstOrDefault().Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentRejected)) ? "Payment rejected" : "Payment pending",
                    x.Payments.FirstOrDefault()?.RRR,
                    PaymentId = x.Payments?.FirstOrDefault()?.Id,
                    CreatedDate = x.CreatedDate.ToString("MMMM dd, yyyy HH:mm:ss"),
                    ApplicationType = x.ApplicationType.Name
                }).ToList(),
            };
        }

        private async Task<ApiResponse> GetMyDeskFAD(ApplicationUser? user)
        {
            //checked 
            var coq = await _unitOfWork.CoQ.Find(x => x.CurrentDeskId.Equals(user.Id), "Plant,Application.User.Company,Application.Facility");
            //var coq = await _unitOfWork.CoQ.Find(x => x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Completed)));
            var processingPlantCoQs = await _unitOfWork.ProcessingPlantCoQ.Find(x => x.CurrentDeskId.Equals(user.Id), "Plant,Product");

            ApplicationType? apptype = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)));
            //&& !x.Application.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)));

            var payment = await _unitOfWork.Payment.Find(x => x.ApplicationTypeId.Equals(apptype.Id));

            if (payment.Count() > 0)
            {
                var pendingCoqs = coq.Where(c => payment.ToList().Any(p => !p.COQId.Equals(c.Id)));

                if (coq != null)
                    return new ApiResponse
                    {
                        Message = "Successful",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                        Data = new
                        {
                            CoQ = pendingCoqs.Select(c => new
                            {
                                c.Id,
                                c.Reference,
                                CompanyName = c.Application?.User.Company.Name,
                                DepotName = c.Plant?.Name,
                                VesselName = c.Application?.Facility.Name,
                                c.DepotPrice,
                                DateOfArrival = c.DateOfVesselArrival.ToString("yyyy-MM-dd hh:mm:ss"),
                                DischargeDate = c.DateOfSTAfterDischarge.ToString("yyyy-MM-dd hh:mm:ss"),
                                SubmittedDate = c.SubmittedDate.Value.ToString("yyyy-MM-dd hh:mm:ss"),
                                Volume = c.GSV == 0 ? c.MT_VAC.ToString("N2") : c.GSV.ToString("N2"),
                                DebitNote = c.GSV == 0 ? (c.MT_VAC * c.DepotPrice * 0.1).ToString("N2") : (c.GSV * c.DepotPrice * 0.1).ToString("N2"),
                                c.Status
                            }),

                            ProcessingPlantCOQ = processingPlantCoQs.OrderByDescending(c => c.CreatedAt).Select(x => new
                            {
                                x.ProcessingPlantCOQId,
                                PlantName = x.Plant.Name,
                                x.Reference,
                                x.Status,
                                x.ConsignorName,
                                x.Consignee,
                                x.ShipmentNo,
                                x.StartTime,
                                x.EndTime,
                                MT_VAC = x.TotalMTVac,
                                MT_AIR = x.TotalMTAir,
                                //GOV = x.GOV,
                                //GSV = x.GSV,
                                x.Price,
                                CreatedBy = x.CreatedBy,
                                SubmittedDate = x.SubmittedDate,
                                ApplicationType = "Processing Plant COQ"
                            }).ToList(),
                        }
                    };
            }

            return new ApiResponse
            {
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = new
                {
                    CoQ = coq.OrderByDescending(c => c.DateCreated).Select(c => new
                    {
                        c.Reference,
                        c.Application?.User.Company.Name,
                        DepotName = c.Plant?.Name,
                        DepotPrice = c.DepotPrice,
                        c.GSV,
                        DebitNote = (c.GSV * c.DepotPrice * 0.01),

                    }),
                    ProcessingPlantCOQ = processingPlantCoQs.OrderByDescending(c => c.CreatedAt).Select(x => new
                    {
                        x.ProcessingPlantCOQId,
                        PlantName = x.Plant.Name,
                        x.Reference,
                        x.Status,
                        x.ConsignorName,
                        x.Consignee,
                        x.ShipmentNo,
                        x.StartTime,
                        x.EndTime,
                        MT_VAC = x.TotalMTVac,
                        MT_AIR = x.TotalMTAir,
                        //GOV = x.GOV,
                        //GSV = x.GSV,
                        x.Price,
                        CreatedBy = x.CreatedBy,
                        SubmittedDate = x.SubmittedDate,
                        ApplicationType = "Processing Plant COQ"
                    }),
                }

            };
        }

        private async Task<ApiResponse> GetMyDeskFO(ApplicationUser? user)
            {
                var coqs = await _unitOfWork.CoQ.Find(x => x.CurrentDeskId.Equals(user.Id) && x.IsDeleted != true, "Application.ApplicationType,Application.User.Company,Plant");
                var processingPlantCoQs = await _unitOfWork.ProcessingPlantCoQ.Find(x => x.CurrentDeskId.Equals(user.Id), "Plant,Product");
                // if (await _userManager.IsInRoleAsync(user, "FAD"))
                //     coqs = await _unitOfWork.CoQ.Find(x => x.FADStaffId.Equals(user.Id) && !x.FADApproved && x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Processing)));
                if (await _userManager.IsInRoleAsync(user, "Company"))
                {
                    coqs = await _unitOfWork.CoQ.Find(x => x.CurrentDeskId.Equals(user.Id) && x.IsDeleted != true, "Application.ApplicationType,Application.User.Company,Plant");
                    processingPlantCoQs = await _unitOfWork.ProcessingPlantCoQ.Find(x => x.CurrentDeskId.Equals(user.Id), "Plant,Product");
                }

                return new ApiResponse
                {
                    Message = "Applications fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = new
                    {
                        CoQ = coqs.OrderByDescending(c => c.DateCreated).Select(x => new
                            {
                                x.Id,
                                x.AppId,
                                CompanyEmail = x.Application?.User.Email,
                                CompanyName = x.Application?.User.Company.Name,
                                x.Application?.VesselName,
                                x.Reference,
                                x.Status,
                                DepotName = x.Plant?.Name,
                                DepotId = x.PlantId,
                                DateOfVesselArrival = x.DateOfVesselArrival.ToShortDateString(),
                                DateOfVesselUllage = x.DateOfVesselUllage.ToShortDateString(),
                                DateOfSTAfterDischarge = x.DateOfSTAfterDischarge.ToShortDateString(),
                                DateOfVesselArrivalISO = x.DateOfVesselArrival.ToString("MM/dd/yyyy"),
                                DateOfVesselUllageISO = x.DateOfVesselUllage.ToString("MM/dd/yyyy"),
                                DateOfSTAfterDischargeISO = x.DateOfSTAfterDischarge.ToString("MM/dd/yyyy"),
                                x.MT_VAC,
                                x.MT_AIR,
                                x.GOV,
                                x.GSV,
                                x.DepotPrice,
                                x.CreatedBy,
                                x.SubmittedDate,
                                ApplicationType = "COQ"
                            }).ToList(),
                        ProcessingPlantCOQ = processingPlantCoQs.OrderByDescending(c => c.CreatedAt).Select(x => new
                        {
                            x.ProcessingPlantCOQId,
                            PlantName = x.Plant.Name,
                            x.Reference,
                            x.Status,
                            x.ConsignorName, 
                            x.Consignee,
                            x.ShipmentNo,
                            x.StartTime,
                            x.EndTime,
                            MT_VAC = x.TotalMTVac,
                            MT_AIR = x.TotalMTAir,
                            //GOV = x.GOV,
                            //GSV = x.GSV,
                            x.Price,
                            x.CreatedBy,
                            x.SubmittedDate,
                            ApplicationType = "Processing Plant COQ"
                        }).ToList(),
                    }
                };
            }

        public async Task<ApiResponse> ViewApplication(int id)
        {

            if (id > 0)
            {
                try
                {
                    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(id), "User.Company,Appointment,ApplicationType,Payments,Facility.VesselType,WorkFlow,Histories,Facility.Tanks.Product,Facility.FacilitySources.LGA.State");
                    var appDepot = await _unitOfWork.ApplicationDepot.GetDepotsAsync(id);
                    if (app != null)
                    {
                        var users = _userManager.Users.Include(c => c.Company).Include(ur => ur.UserRoles).ThenInclude(r => r.Role);

                        var user = users.FirstOrDefault(x => x.Email.Equals(User));
                        var histories = await _userManager.IsInRoleAsync(user, "Company") ? new List<ApplicationHistory>() : app.Histories.ToList();
                        histories.ForEach(h =>
                        {

                            var t = users.FirstOrDefault(x => x.Id.Equals(h.TriggeredBy));
                            var r = users.FirstOrDefault(x => x.Id.Equals(h.TargetedTo));
                            h.TriggeredBy = t.Email;
                            h.TriggeredByRole = t.UserRoles.FirstOrDefault().Role.Name;
                            h.TargetedTo = r.Email;
                            h.TargetRole = r.UserRoles.FirstOrDefault().Role.Name;
                        });
                        var schedules = app.Appointment.ToList();
                        schedules.ForEach(a =>
                        {
                            var s = users.FirstOrDefault(x => x.Id.Equals(a.ScheduledBy));
                            var ap = users.FirstOrDefault(x => x.Id.Equals(a.ApprovedBy));
                            a.ScheduledBy = s?.Email;
                            a.ApprovedBy = ap?.Email;
                        });
                        var sch = schedules.Select(s => new
                        {
                            s.ApprovedBy,
                            SupervisorReject = !s.IsApproved && !string.IsNullOrEmpty(s.ApprovalMessage),
                            s.ScheduledBy,
                            s.IsApproved,
                            s.ApprovalMessage,
                            InspectionDate = s.AppointmentDate.ToString("MMM dd, yyyy HH:mm:ss"),
                            s.ClientMessage,
                            s.ContactName,
                            s.IsAccepted,
                            CompanyReject = !s.IsAccepted && !string.IsNullOrEmpty(s.ClientMessage),
                            s.ScheduleMessage,
                            s.ScheduleType,
                            ExpiryDate = s.ExpiryDate.ToString("MMM dd, yyyy HH:mm:ss")
                        });

                        var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Utils.NOA));

                        var appDocs = await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId == app.Id && x.ApplicationTypeId == appType.Id);
                        

                        var paymentStatus = "Payment pending";
                        if (app.Payments.FirstOrDefault()?.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted)) is true)
                        {
                            paymentStatus = "Payment confirmed";
                        }
                        else if (app.Payments.FirstOrDefault()?.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentRejected)) is true)
                        {
                            paymentStatus = "Payment rejected";
                        }

                        var rrr = app.Payments.FirstOrDefault()?.RRR;
                        var surveyorId = (await _unitOfWork.ApplicationSurveyor
                            .FirstOrDefaultAsync(x => x.ApplicationId == id))?.NominatedSurveyorId;
                        var jetty = await _unitOfWork.Jetty.FirstOrDefaultAsync(x => x.Id.Equals(app.Jetty));
                        _response = new ApiResponse
                        {
                            Message = "Application detail found",
                            StatusCode = HttpStatusCode.OK,
                            Success = true,
                            Data = new
                            {
                                app.Id,
                                app.Status,
                                app.Reference,
                                CompanyName = app.User.Company.Name,
                                app.User.Email,
                                CompanyAddress = app.User.Company.Address,
                                //State = app.Facility.LGA.State.Name,
                                //LGA = app.Facility.LGA.Name,
                                AppType = app.ApplicationType.Name,
                                CreatedDate = app.CreatedDate.ToString("MMM dd, yyyy HH:mm:ss"),
                                SubmittedDate = app.SubmittedDate != null ? app.SubmittedDate.Value.ToString("MMM dd, yyyy HH:mm:ss") : null,
                                PaymnetStatus = paymentStatus,
                                RRR = rrr,
                                TotalAmount = string.Format("{0:N}", app.Payments.Sum(x => x.Amount)),
                                PaymentDescription = app.Payments.FirstOrDefault()?.Description,
                                PaymnetDate = app.Payments.FirstOrDefault()?.TransactionDate.ToString("MMM dd, yyyy HH:mm:ss"),
                                CurrentDesk = _userManager.Users.FirstOrDefault(x => x.Id.Equals(app.CurrentDeskId))?.Email,
                                AppHistories = histories.OrderByDescending(x => x.Id),
                                Schedules = sch,
                                Documents = appDocs,
                                app.MarketerName,
                                app.MotherVessel,
                                Jetty = jetty?.Name,
                                app.LoadingPort,
                                ApplicationDepots = appDepot,
                                NominatedSurveyor = (await _unitOfWork.NominatedSurveyor.Find(c => c.Id == surveyorId)).FirstOrDefault(),
                                Vessel = new
                                {
                                    app.Facility.Name,
                                    VesselType = app.Facility.VesselType.Name,
                                    app.Facility.Capacity,
                                    app.Facility.DeadWeight,
                                    app.Facility.IMONumber,
                                    app.Facility.Flag,
                                    app.Facility.CallSIgn,
                                    app.Facility.Operator,
                                    app.LoadingPort,
                                    Tanks = app.Facility.Tanks.Select(t => new
                                    {
                                        t.Name,
                                        t.Capacity,
                                        Product = t.Product.Name
                                    }),
                                    FacilitySources = app.Facility.FacilitySources.Select(f => new
                                    {
                                        f.Name,
                                        f.LicenseNumber,
                                        f.Address,
                                        State = f.LGA.State.Name,
                                        LGA = f.LGA.Name
                                    })
                                }
                            }
                        };
                    }
                    else
                        _response = new ApiResponse
                        {
                            Message = "No Application was found",
                            StatusCode = HttpStatusCode.NotFound,
                        };
                }
                catch (Exception ex)
                {
                    _response = new ApiResponse
                    {
                        Message = ex.Message,
                        StatusCode = HttpStatusCode.InternalServerError,
                    };
                }
            }
            else
                _response = new ApiResponse
                {
                    Message = "ApplicationID invalid",
                    StatusCode = HttpStatusCode.NotFound,
                };
            return _response;
        }

        public async Task<ApiResponse> Process(int id, string act, string comment = null)
        {

            if (id > 0)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(User);
                    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    _response = new ApiResponse();
                    if (app != null && user != null)
                    {
                        var flow = await _flow.AppWorkFlow(id, act, comment);
                        if (flow.Item1)
                        {
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.Success = true;
                            _response.Message = "Application has been pushed";
                            _response.Data = flow.Item2;
                        }
                        else
                        {

                            _response.Message = "Application cannot be pushed";
                        }
                    }
                }
                catch (Exception ex)
                {
                    _response = new ApiResponse
                    {
                        Message = ex.Message,
                        StatusCode = HttpStatusCode.InternalServerError,
                    };
                }
            }
            else
                _response = new ApiResponse
                {
                    Message = "ApplicationID invalid",
                    StatusCode = HttpStatusCode.NotFound,
                };

            return _response;
        }

        public async Task<ApiResponse> ApplicationReport(ApplicationReportViewModel model)
        {
            var apps = await _unitOfWork.vAppVessel.Find(a => a.SubmittedDate >= model.Min && a.SubmittedDate <= model.Max);

            if (!string.IsNullOrEmpty(model.Status))
                apps = apps.Where(b => b.Status.ToLower().Equals(model.Status.ToLower())).ToList();

            if (model.ApplicationTypeId != null && model.ApplicationTypeId > 0)
                apps = apps.Where(b => b.ApplicationTypeId == model.ApplicationTypeId).ToList();

            if (apps != null && apps.Count() > 0)
            {
                _response = new ApiResponse
                {
                    Message = "Application Report Found",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = apps.Select(a => new
                    {
                        a.Reference,
                        a.CompanyName,
                        a.AppTypeName,
                        a.VesselName,
                        a.NoOfTanks,
                        a.Capacity,
                        a.CreatedDate,
                        a.Status,
                        a.IsDeleted,

                    })
                };

                return _response;
            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "Application Report Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }

            return _response;
        }

        public async Task<ApiResponse> AllApplicationsByDepot(int depotId)
        {
            var appDepot = await _unitOfWork.ApplicationDepot.Find(x => x.DepotId.Equals(depotId));
            var applications = await _unitOfWork.Application.Find(x => appDepot.Any(a => a.AppId == x.Id && x.Status == Enum.GetName(AppStatus.Completed)) == true);
            if (applications != null)
            {
                var filteredapps = applications.Where(x => x.IsDeleted == false);
                _response = new ApiResponse
                {
                    Message = "Applications fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = applications.OrderByDescending(c => c.CreatedDate).ToList()
                };
            }



            return _response;
        }

        public async Task<ApiResponse> AllApplicationsByJetty(int jettyId)
        {
            var appDepot = await _unitOfWork.Jetty.Find(x => x.Id.Equals(jettyId));
            var applications = await _unitOfWork.Application.Find(x => appDepot.Any(a => a.Id == x.Jetty && x.Status == Enum.GetName(AppStatus.Completed)) == true);
            if (applications != null)
            {
                var filteredapps = applications.Where(x => x.IsDeleted == false);
                _response = new ApiResponse
                {
                    Message = "Applications fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = applications.OrderByDescending(c => c.CreatedDate).ToList()
                };
            }



            return _response;
        }

        public async Task<ApiResponse> AllApplicationsInDepotByUserID()
        {
            var user = await _userManager.FindByEmailAsync(User);

            if (user is null)
            {
                _response = new ApiResponse
                {
                    Message = "No User was found",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
                return _response;
            }
            var depots = (await _unitOfWork.PlantOfficer.Find(c => c.OfficerID.Equals(user.Id) && !c.IsDeleted)).Select(c => c.PlantID);
            if (!depots.Any())
            {
                _response = new ApiResponse
                {
                    Message = "User not assigned to a depot",
                    StatusCode = HttpStatusCode.OK,
                    Success = false
                };
                return _response;
            }

            var appDepots = await _unitOfWork.ApplicationDepot.Find(c => depots.Contains(c.DepotId) && !string.IsNullOrEmpty(c.DischargeId), "Application.Facility.VesselType,Application.User.Company,Application.AppJetty");
            //var apps = appDepots.Select(x => x.Application);
            IEnumerable<CoQ>? alldepotcoqs = await _unitOfWork.CoQ.Find(c => appDepots.Any(ad => ad.AppId.Equals(c.AppId) && ad.DepotId.Equals(c.PlantId)));
            var coq = (alldepotcoqs.Where(c => appDepots.Any(ad => ad.AppId.Equals(c.AppId) && ad.DepotId.Equals(c.PlantId)))).Select(i => new {i.AppId, i.PlantId}).ToList();
            var apps = appDepots.Select(a => a.Application);
            if(coq.Count > 0)
            {
                appDepots = appDepots.Where(a => !coq.Any(x => x.PlantId.Equals(a.DepotId) && x.AppId.Equals(a.AppId)));
                apps = appDepots.GroupBy(x => x.AppId).Select(a => a.FirstOrDefault().Application);
            }
            var rejectedCoQs = (await _unitOfWork.CoQ.Find(x => x.CurrentDeskId.Equals(user.Id) && x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Rejected)), "Application.User.Company,Application.Facility.VesselType,Application.AppJetty")).Select(n => new
            {
                n.Id,
                CompanyName = n.Application.User.Company.Name,
                n.Application.VesselName,
                VesselType = n.Application.Facility.VesselType.Name,
                CompanyEmail = n.Application.User.Email,
                n.Reference,
                Jetty = n.Application.AppJetty.Name,
                n.Status,
                n.Application.HasCleared,
                CreatedDate = n.DateCreated.ToString("yyyy-MM-dd hh:mm:ss"),
                SubmittewdDate = n.SubmittedDate.Value.ToString("yyyy-MM-dd hh:mm:ss"),
            }); ;

            //if (rejectedApps.Count() > 0)
            //    apps = apps.Concat(rejectedApps.Select(a => a.Application));
            var data = apps.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).OrderByDescending(x => x.SubmittedDate).Select(n => new
            {
                n.Id,
                CompanyName = n.User.Company.Name,
                n.VesselName,
                VesselType = n.Facility.VesselType.Name,
                CompanyEmail = n.User.Email,
                n.Reference,
                Jetty = n.AppJetty.Name,
                n.Status,
                n.HasCleared,
                CreatedDate = n.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss"),
            });

            _response = new ApiResponse
            {
                Message = "Applications fetched successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = new
                {
                    clearedNOAs = data,
                    rejectedCoQs
                }
            };

            return _response;
        }

        public async Task<ApiResponse> AllApplicationsInJettyByUserID()
        {
            var user = await _userManager.FindByEmailAsync(User);

            if (user is null)
            {
                _response = new ApiResponse
                {
                    Message = "No User was found",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
                return _response;
            }
            var jettys = (await _unitOfWork.JettyOfficer.Find(c => c.OfficerID == user.Id,"Jetty")).Select(c => c.JettyId);
            if (!jettys.Any())
                return new ApiResponse
                {
                    Message = "User not assigned to a Jetty",
                    StatusCode = HttpStatusCode.OK,
                    Success = false
                };

            var apps = await _unitOfWork.Application.Find(c => jettys.Contains(c.Jetty) && c.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Completed)) && !c.HasCleared, "User.Company,Facility.VesselType,AppJetty");
            var data = apps.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).OrderByDescending(x => x.SubmittedDate).Select(n => new
            {
                n.Id,
                CompanyName = n.User.Company.Name,
                n.VesselName,
                VesselType = n.Facility.VesselType.Name,
                CompanyEmail = n.User.Email,
                n.Reference,
                Jetty = n.AppJetty.Name,
                n.Status,
                n.HasCleared,
                CreatedDate = n.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss"),

            });

            _response = new ApiResponse
            {
                Message = "Applications fetched successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = data
            };

            return _response;
        }

        public async Task<ApiResponse> GetAppVesselInfo(int AppId, int DepotId)
        {
            try
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(AppId));

                if (app == null)
                    throw new Exception("Application does not exist!");

                var appDepot = await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.AppId == AppId && x.DepotId == DepotId);

                if (appDepot == null)
                    throw new Exception("The Select depot does not exist for this application!");

                var depot = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id.Equals(appDepot.DepotId));
                var product = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id.Equals(appDepot.ProductId));

                if (product == null)
                    throw new Exception("Product does not exist");
                if (depot == null)
                    throw new Exception("Depot does not exist");

                //Check if COQ exists
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.AppId == app.Id && x.PlantId == depot.Id);

                return new ApiResponse
                {
                    Data = new
                    {
                        MarketerName = depot.Company,
                        //DepotCapacity = depot.,
                        ProductName = product.Name,
                        Volume = appDepot.Volume,
                        VesselName = app.VesselName,
                        MotherVessel = app.MotherVessel,
                        Jetty = app.Jetty,
                        ETA = app,
                        RecievingTerminal = depot.Name,
                        COQExist = coq == null ? false : true,
                        coqId = coq?.Id,
                    },
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

        public async Task<ApiResponse> IMONumberVerification(string imoNumber)
        {
            var verifyIMO = await _unitOfWork.Facility.FirstOrDefaultAsync(x => x.IMONumber.Equals(imoNumber));
            if (verifyIMO != null)
            {
                _response = new ApiResponse
                {
                    Message = "Verified",
                    Data = new
                    {
                        Id = verifyIMO.Id,
                        IMONumber = verifyIMO.IMONumber,
                        Name = verifyIMO.Name
                    },
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };

                return _response;

            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            }

            return _response;
        }

        public async Task<ApiResponse> EditIMONoByName(string name, string newIMO)
        {
            var vessel = await _unitOfWork.Facility.FirstOrDefaultAsync(x => x.Name.Equals(name));
            if (vessel == null)
            {
                _response = new ApiResponse
                {
                    Message = "No Vessel was found with this Name",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
            else
            {
                var check = await _unitOfWork.Facility.FirstOrDefaultAsync(_ => _.IMONumber.Equals(newIMO));
                if (check == null)
                {

                    vessel.IMONumber = newIMO;
                    await _unitOfWork.Facility.Update(vessel);
                    _unitOfWork.Save();

                    _response = new ApiResponse
                    {
                        Message = "Updated Successfully",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
                else
                {
                    _response = new ApiResponse
                    {
                        Message = "There is an Existing Vessel with the suggested IMO number",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false,
                    };
                }
            }

            return _response;

        }


        public async Task<ApiResponse> GetAllVessels()
        {
            var vessel = await _unitOfWork.Facility.GetAll();
            if (vessel != null)
            {
                _response = new ApiResponse
                {
                    Message = "Vessels was found",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = vessel.Select(x => new
                    {
                        VesselName = x.Name,
                        ImoNumber = x.IMONumber,
                        VesselId = x.Id,
                    }).ToList()
                };
            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "No Vessel was found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }


            return _response;
        }
        //public string GenerateDischargeID(string product)
        //{
        //    Random random = new Random();
        //    string _token = random.Next(100001, 999999).ToString();

        //    return $"{product.ToUpper()}/{_token}";
        //}
        //public async Task<bool> PostDischargeId(int id,string  userId)
        //{
        //    var appDepots = await _context.ApplicationDepots.Where(a => a.AppId == id).Include(x => x.Product).ToListAsync();
        //    foreach (var appDepot in appDepots)
        //    {
        //        appDepot.DischargeId = GenerateDischargeID(appDepot.Product.Name);
        //    }
            
        //    _context.ApplicationDepots.UpdateRange(appDepots);
        //    var s = await _context.SaveChangesAsync();
        //    if(s > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        public IEnumerable<int> GetLongProcessingApps()
        {
            return _unitOfWork.Application.Query(c => c.DeskMovementDate.HasValue
            && c.DeskMovementDate.Value.AddMinutes(-15) >= DateTime.Now).Select(c => c.Id).ToList();
        }

        private ViewApplicationsByFieldOfficerDTO GetApplications(int Id)
        {
            var result = new ViewApplicationsByFieldOfficerDTO();

            var apps = _unitOfWork.ApplicationDepot.Query()
                        .Include(x => x.Application)
                        .Include(x => x.Depot)
                        .FirstOrDefault(x => x.AppId == Id );
            var companyDetails = _userManager.FindByIdAsync(apps.Application.UserId).Result;
            
            var vessel = _unitOfWork.Facility.Query().FirstOrDefault(x => x.Id == apps.Application.FacilityId);
            var vType = _unitOfWork.VesselType.Query().FirstOrDefault(x => x.Id == vessel.VesselTypeId);

            var jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == apps.Application.Jetty)?.Name;

            var app = new ViewApplicationsByFieldOfficerDTO
            {
                ApplicationTypeId = apps?.Application.ApplicationTypeId ?? 0,
                CreatedDate = apps.Application.CreatedDate,
                CurrentDeskId = apps.Application.CurrentDeskId,
                DeportStateId = apps.Application.DeportStateId,
                DeskMovementDate = apps.Application.DeskMovementDate,
                ETA = apps.Application.ETA,
                FacilityId = apps.Application.FacilityId,
                FADApproved = apps.Application.FADApproved,
                Jetty = jetty,
                LoadingPort = apps.Application.LoadingPort,
                FADStaffId = apps.Application.FADStaffId,
                FlowId = apps.Application.FlowId,
                IsDeleted = apps.Application.IsDeleted,
                MarketerName = apps.Application.MarketerName,
                ModifiedDate = apps.Application.ModifiedDate,
                MotherVessel = apps.Application.MotherVessel,
                Status = apps.Application.Status,
                Reference = apps.Application.Reference,
                VesselName = apps.Application.VesselName,
                SubmittedDate = apps.Application.SubmittedDate,
                SurveyorId = apps.Application.SurveyorId,
                UserId = apps.Application.UserId,
                CompanyId = vessel.CompanyId,
                IMONumber = vessel.IMONumber,
                VesselType = vessel?.VesselType.Name,
                IsLicensed = vessel.IsLicensed,
                Id = vessel.Id,
                CallSIgn = vessel.CallSIgn,
                Capacity = vessel.Capacity,
                DeadWeight = vessel.DeadWeight,
                ElpsId = vessel.ElpsId,
                Flag = vessel.Flag,
                Name = vessel.Name,
                Operator = vessel.Operator,
                PlaceOfBuild = vessel.PlaceOfBuild,
                VesselTypeId = vessel.VesselTypeId,
                YearOfBuild = vessel.YearOfBuild,
                DischargeId = apps.DischargeId,
                HasCleared = apps.Application.HasCleared,
                DepotId = apps.DepotId
            };

            return app;

        }

    }
}
