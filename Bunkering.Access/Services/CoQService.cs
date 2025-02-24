﻿using AutoMapper;
using Azure;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bunkering.Access.Services
{
    public class CoQService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpCxtAccessor;
        private ApiResponse _apiReponse;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private string LoginUserEmail = string.Empty;
        private readonly IElps _elps;
        private readonly AppSetting _setting;
        private readonly WorkFlowService _flow;
        private readonly ApplicationContext _context;
        private readonly MessageService _messageService;


        public CoQService(IUnitOfWork unitOfWork, IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, 
            IOptions<AppSetting> setting, IMapper mapper, IElps elps, WorkFlowService flow, ApplicationContext context, MessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _httpCxtAccessor = httpCxtAccessor;
            _apiReponse = new ApiResponse();
            _userManager = userManager;
            _mapper = mapper;
            _apiReponse = new ApiResponse();
            LoginUserEmail = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _elps = elps;
            _setting = setting.Value;
            _flow = flow;
            _context = context;
            _messageService = messageService;
        }

        //public async Task<ApiResponse> CreateCoQ(CreateCoQViewModel Model)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(LoginUserEmail);

        //        if (user == null)
        //            throw new Exception("Cannot find user with Email: " + LoginUserEmail);

        //        //if (user.UserRoles.FirstOrDefault().Role.Name != RoleConstants.Field_Officer)
        //        //   throw new Exception("Only Field Officers can create CoQ.");
                
        //        var foundCOQ = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.AppId.Equals(Model.AppId) && x.DepotId.Equals(Model.DepotId));
        //        CoQ? result_coq = null;
        //        if(foundCOQ == null)
        //        {
        //            var coq = _mapper.Map<CoQ>(Model);
        //            coq.CreatedBy = LoginUserEmail;
        //            coq.DateCreated = DateTime.UtcNow.AddHours(1);
        //            coq.CurrentDeskId = user.Id;
        //            coq.Status = Enum.GetName(typeof(AppStatus), AppStatus.Initiated);
        //            result_coq = await _unitOfWork.CoQ.Add(coq);
        //        }
        //        else
        //        {
        //            foundCOQ.DateOfSTAfterDischarge = Model.DateOfSTAfterDischarge;
        //            foundCOQ.DateOfVesselArrival = Model.DateOfVesselArrival;
        //            foundCOQ.DateOfVesselUllage = Model.DateOfVesselUllage;
        //            foundCOQ.DepotPrice = Model.DepotPrice;
        //            foundCOQ.GOV = Model.GOV;
        //            foundCOQ.GSV = Model.GSV;
        //            foundCOQ.MT_VAC = Model.MT_VAC;
        //            foundCOQ.MT_AIR = Model.MT_AIR;
        //            foundCOQ.CurrentDeskId = user.Id;
                    
        //            result_coq = await _unitOfWork.CoQ.Update(foundCOQ);
        //        }

        //        await _unitOfWork.SaveChangesAsync(user.Id);

        //        return new ApiResponse
        //        {
        //            Data = result_coq,
        //            Message = "Successfull",
        //            StatusCode = System.Net.HttpStatusCode.OK
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        return _apiReponse = new ApiResponse
        //        {
        //            Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}\n",
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //    }
        //}

        public async Task<ApiResponse> GetCoQsByAppId(int appId)
        {
            try
            {
                var foundCOQ = await _unitOfWork.CoQ.Find(x => x.AppId == appId);
                return new ApiResponse
                {
                    Data = foundCOQ,
                    Message = "Successful",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return _apiReponse = new ApiResponse
                {
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}\n",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> GetCoQCertsByAppId(int id)
        {
            try
            {
                var foundCOQ = await _unitOfWork.COQCertificate.Find(x => x.COQ.AppId.Equals(id), "COQ.Application.User.Company,COQ.Application.Facility.VesselType,COQ.Plant");
                //return new ApiResponse
                //{
                //    Data = foundCOQ,
                //    Message = "Successful",
                //    StatusCode = HttpStatusCode.OK,
                //    Success = true
                //};
                int count = foundCOQ.Count();
                return new ApiResponse
                {
                    //using if statements here ?: to check conditions for the permit
                    Message = count > 0 ? "Success, Permit Found" : "Permit Not Found",
                    StatusCode = count > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    Success = count > 0 ? true : false,
                    Data = count > 0 ? foundCOQ.Select(x => new
                    {
                        x.Id,
                        x.COQ.AppId,
                        x.COQId,
                        DepotName = $"{x.COQ.Plant.Name}({x.COQ.Plant.State})",
                        CompanyName = x.COQ.Application.User.Company.Name,
                        LicenseNo = x.CertifcateNo,
                        IssuedDate = x.IssuedDate.ToString("MMM dd, yyyy HH:mm:ss"),
                        ExpiryDate = x.ExpireDate.ToString("MMM dd, yyyy HH:mm:ss"),
                        x.COQ.Application.User.Email,
                        VesselTypeType = x.COQ.Application.Facility.VesselType.Name,
                        VesselName = x.COQ.Application.Facility.Name,
                    }).OrderByDescending(d => d.IssuedDate) : new { }
                };
            }
            catch (Exception e)
            {
                return _apiReponse = new ApiResponse
                {
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}\n",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> GetCoQsByDepotId(int depotId)
        {
            try
            {
                var foundCOQ = await _unitOfWork.CoQ.Find(x => x.PlantId == depotId);
                return new ApiResponse
                {
                    Data = foundCOQ,
                    Message = "Successful",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                return _apiReponse = new ApiResponse
                {
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}\n",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> DocumentUpload(int id)
        {

            if (id > 0)
            {
                var docList = new List<SubmittedDocument>();
                var app = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id == id, "Application.User,Application.Facility.VesselType,Application.ApplicationType");
                if (app != null)
                {
                    var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(c => c.Name == Utils.COQ);
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
                    var factypedocs = await _unitOfWork.FacilityTypeDocuments
                        .Find(x => x.ApplicationTypeId
                        .Equals(appType.Id) && x.VesselTypeId.Equals(app.Application.Facility.VesselTypeId));
                    if (factypedocs != null && factypedocs.Count() > 0)
                    {
                        var compdocs = _elps.GetCompanyDocuments(app.Application.User.ElpsId, "company")
                            .Stringify().Parse<List<Document>>();
                        var facdocs = _elps.GetCompanyDocuments(app.Application.Facility.ElpsId, "facility")
                            .Stringify().Parse<List<FacilityDocument>>();
                        var appdocs = await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId == app.AppId);

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
                    _apiReponse = new ApiResponse
                    {
                        Message = "Facility Type Documents fetched",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                        Data = new
                        {
                            Docs = docList,
                            ApiData = new
                            {
                                CompanyElpsId = app.Application?.User.ElpsId,
                                FacilityElpsId = app.Application?.Facility.ElpsId,
                                ApiEmail = _setting.AppEmail,
                                ApiHash = $"{_setting.AppEmail}{_setting.AppId}".GenerateSha512()
                            }
                        }
                    };
                }
                else
                    _apiReponse = new ApiResponse
                    {
                        Message = "ApplicationID invalid",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
            }
            else
                _apiReponse = new ApiResponse
                {
                    Message = "ApplicationID invalid",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return _apiReponse;
        }

        public async Task<object?> GetCoqRequiredDocuments(Application appp)
        {
            var docList = new List<SubmittedDocument>();
            var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(c => c.Name == Utils.COQ);
            if (appType == null || appp == null)
            {
                return null;
            }
            var factypedocs = await _unitOfWork.FacilityTypeDocuments
                .Find(x => x.ApplicationTypeId
                .Equals(appType.Id) && x.VesselTypeId.Equals(appp.Facility.VesselTypeId));
            if (factypedocs != null && factypedocs.Count() > 0)
            {
                var compdocs = _elps.GetCompanyDocuments(appp.User.ElpsId, "company")
                    .Stringify().Parse<List<Document>>();
                var facdocs = _elps.GetCompanyDocuments(appp.Facility.ElpsId, "facility")
                    .Stringify().Parse<List<FacilityDocument>>();
                var appdocs = await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId == appp.Id);

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
                                    ApplicationId = appp.Id,
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
                                    ApplicationId = appp.Id,
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
            return new ApiResponse
            {
                Message = "Facility Type Documents fetched",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = new
                {
                    Docs = docList,
                    ApiData = new
                    {
                        CompanyElpsId = appp.User.ElpsId,
                        FacilityElpsId = appp.Facility.ElpsId,
                        ApiEmail = _setting.AppEmail,
                        ApiHash = $"{_setting.AppEmail}{_setting.AppId}".GenerateSha512()
                    }
                }
            };
        }

        public async Task<object?> GetCoqRequiredDocuments(Plant appp)
        {

            var docList = new List<SubmittedDocument>();
            var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(c => c.Name == Utils.COQ);
            if (appType == null || appp == null)
            {
                return null;
            }
            var factypedocs = await _unitOfWork.FacilityTypeDocuments
                .Find(x => x.ApplicationTypeId
                .Equals(appType.Id));
            if (factypedocs != null && factypedocs.Count() > 0)
            {
                var compdocs = _elps.GetCompanyDocuments((int)appp.CompanyElpsId, "company")
                    .Stringify().Parse<List<Document>>();
                var facdocs = _elps.GetCompanyDocuments((int)appp.CompanyElpsId, "facility")
                    .Stringify().Parse<List<FacilityDocument>>();
                var appdocs = await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId == appp.Id);

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
                                    ApplicationId = appp.Id,
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
                                    ApplicationId = appp.Id,
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
            return new ApiResponse
            {
                Message = "Facility Type Documents fetched",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = new
                {
                    Docs = docList,
                    ApiData = new
                    {
                        CompanyElpsId = appp.CompanyElpsId,
                        FacilityElpsId = appp.ElpsPlantId,
                        ApiEmail = _setting.AppEmail,
                        ApiHash = $"{_setting.AppEmail}{_setting.AppId}".GenerateSha512()
                    }
                }
            };
        }

        public async Task<ApiResponse> AddDocuments(int id)
        {
            if (id > 0)
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "Facility.VesselType");
                var user = await _userManager.FindByEmailAsync(LoginUserEmail);
                if (app != null)
                {
                    var facTypeDocs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.ApplicationTypeId.Equals(app.ApplicationTypeId) && x.VesselTypeId.Equals(app.Facility.VesselTypeId));

                    if (facTypeDocs.Any())
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
                        _apiReponse = new ApiResponse
                        {
                            Message = submit.Item2,
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _apiReponse = new ApiResponse
                        {
                            Message = submit.Item2,
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Success = false
                        };
                    }
                }
                else
                    _apiReponse = new ApiResponse
                    {
                        Message = "ApplicationID invalid",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
            }
            else
                _apiReponse = new ApiResponse
                {
                    Message = "Application not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            return _apiReponse;
        }

        public async Task<ApiResponse> Submit(int Id)
        {
            try
            {
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id.Equals(Id)) ?? throw new Exception($"COQ with id={Id} does not exist.");
                var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception($"User with email={LoginUserEmail} does not exist.");

                var result = await _flow.CoqWorkFlow(Id, Enum.GetName(typeof(AppActions), AppActions.Submit), "Application Submitted");

                return new ApiResponse
                {
                    Message = result.Item2,
                    StatusCode = result.Item1? HttpStatusCode.OK: HttpStatusCode.InternalServerError,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = true
                };
            }
        }

        public async Task<ApiResponse> Process(int id, string act, string comment)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception($"User with the email={LoginUserEmail} was not found.");
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new Exception($"COQ with the ID={id} could not be found.");

                var result = await _flow.CoqWorkFlow(id, act, comment);

                if(result.Item1)
                    return new ApiResponse
                    {
                        Data = result.Item1,
                        Message = result.Item2,
                        Success = true,
                        StatusCode = HttpStatusCode.OK
                    };
                else throw new Exception("COQ Application could not be pushed.");
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        public async Task<ApiResponse> GetDebitNote(int id)
        {
            var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(c => c.Id == id);
            
            if (payment is not null)
            {
                double capacity = 0;
                var dateOfSTAfterDischarge = new DateTime();
                string facName = string.Empty; string state = string.Empty; string? product = string.Empty;
                string marketer = string.Empty; string motherVessel = string.Empty; string daughterVessel = string.Empty; 
                string supplier = string.Empty;
                double price = 0;

                var coqRef = await _unitOfWork.CoQReference.FirstOrDefaultAsync(c => c.Id.Equals(payment.COQId));
                if (coqRef.PlantCoQId == null)
                {
                    var depotCoq = await _unitOfWork.CoQ.FirstOrDefaultAsync(d => d.Id.Equals(coqRef.DepotCoQId), "Application.User.Company,Application.Facility");

                    var appDepot = await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.AppId.Equals(payment.ApplicationId) && x.DepotId.Equals(depotCoq.PlantId), "Depot,Product");
                    //var depot = await _unitOfWork.Plant.FirstOrDefaultAsync(d => d.Id.Equals(depotCoq.PlantId));

                    capacity = depotCoq.MT_VAC == 0 ? depotCoq.GSV : depotCoq.MT_VAC;
                    state = appDepot.Depot.State;
                    facName = appDepot.Depot.Name;
                    price = depotCoq.DepotPrice;
                    product = appDepot.Product.Name;
                    marketer = depotCoq.Application.User.Company.Name;
                    motherVessel = depotCoq.Application.MotherVessel;
                    daughterVessel = depotCoq.Application.Facility.Name;
                }
                else
                {
                    var ppCoq = await _unitOfWork.ProcessingPlantCoQ.FirstOrDefaultAsync(p => p.ProcessingPlantCOQId.Equals(coqRef.PlantCoQId), "Plant");
                    state = ppCoq.Plant.State;
                    price = ppCoq.Price;
                    marketer = ppCoq.Plant.Company;
                    facName = ppCoq.Plant.Name;
                }

                var result = new DebitNoteDTO(
                dateOfSTAfterDischarge,
                payment.TransactionDate.AddDays(21),
                marketer,
                facName,
                price,
                payment.Amount,
                capacity,
                price,
                state,
                $"NMDPRA/{state.Substring(0, 2).ToUpper()}/{payment.Id}",
                motherVessel,
                daughterVessel,
                "", product);
                return new ApiResponse
                {
                    Message = $"Debit note fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = result
                };
            }
            else
            {
                return new ApiResponse
                {
                    Message = "CoQ Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
        }

        //public async Task<ApiResponse> GetDebitNote(int id)
        //{
        //   var coq = await _unitOfWork.Payment.FirstOrDefaultAsync(c => c.Id == id, "PLant");

        //    if (coq is not null)
        //    {
        //        var app = await _unitOfWork.Application.FirstOrDefaultAsync(a => a.Id == coq.Id);
        //        if (app is null)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Application Not Found",
        //                StatusCode = HttpStatusCode.NotFound,
        //                Success = false
        //            };
        //        }
        //        //var price = coq.MT_VAC * coq.DepotPrice;
        //        //var result = new DebitNoteDTO(
        //        //coq.DateOfSTAfterDischarge,
        //        //coq.DateOfSTAfterDischarge.AddDays(21),
        //        //app.MarketerName,
        //        //coq.Depot!.Name,
        //        //price,
        //        //coq.DepotPrice * 0.01m,
        //        //coq.Depot!.Capacity,
        //        //price / coq.Depot!.Capacity
        //        //);
        //        return new ApiResponse
        //        {
        //            Message = $"Debit note fetched successfully",
        //            StatusCode = HttpStatusCode.OK,
        //            Success = true,
        //            //Data = result
        //        };
        //    }
        //    else
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "CoQ Not Found",
        //            StatusCode = HttpStatusCode.NotFound,
        //            Success = false
        //        };
        //    }
        //}

        //public async Task<ApiResponse> AddCoqTank(COQCrudeTankDTO model) 
        //{
        //    var user = await _userManager.FindByEmailAsync(LoginUserEmail);
        //    try
        //    {
        //        var tank = await _unitOfWork.CoQTank.FirstOrDefaultAsync(x => x.CoQId == model.CoQId && x.TankName.ToLower().Equals(model.TankName.ToLower()) && x.TankMeasurement.Any(m => m.MeasurementTypeId.Equals(model.MeasurementTypeId)));
        //        if(tank == null)
        //        {
        //            var data = _mapper.Map<TankMeasurement>(model);
        //            await _unitOfWork.CoQTank.Add(new COQTank
        //            {
        //                CoQId = model.CoQId,
        //                TankName = model.TankName,
        //                TankMeasurement = new List<TankMeasurement> { data }
        //            });
        //            await _unitOfWork.SaveChangesAsync(user.Id);

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return _apiReponse;
        //}

        public async Task<ApiResponse> EditCOQForGas(int id, CreateGasProductCoQDto model)
        {
            var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");
            var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name == Utils.COQ) ?? throw new Exception("Application type of COQ is not configured yet, please contact support");

            var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if(coq != null)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    #region Create Coq
                    coq.DateOfSTAfterDischarge = model.DateOfSTAfterDischarge;
                    coq.DateOfVesselArrival = model.DateOfVesselArrival;
                    coq.DateOfVesselUllage = model.DateOfVesselUllage;
                    coq.DepotPrice = model.DepotPrice;
                    coq.ProductId = model.ProductId;
                    coq.ArrivalShipFigure = model.ArrivalShipFigure;
                    coq.QuauntityReflectedOnBill = model.QuauntityReflectedOnBill;
                    coq.DischargeShipFigure = model.DischargeShipFigure;
                    coq.Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing);
                    coq.NameConsignee = model.NameConsignee;

                    _context.CoQs.Update(coq);
                    _context.SaveChanges();
                    #endregion

                    //remove existing tank readings
                    var coqTanks = await _unitOfWork.CoQTank.Find(x => x.CoQId.Equals(id), "TankMeasurement");
                    if(coqTanks != null)
                    {
                        await _unitOfWork.CoQTank.RemoveRange(coqTanks);
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }

                    #region Create COQ Tank
                    var coqTankList = new List<COQTank>();

                    foreach (var before in model.TankBeforeReadings)
                    {
                        var newCoqTank = new COQTank
                        {
                            CoQId = coq.Id,
                            TankId = before.TankId
                        };

                        var after = model.TankAfterReadings.FirstOrDefault(x => x.TankId == before.TankId);

                        if (after != null && before.coQGasTankDTO != null)
                        {
                            var b = before.coQGasTankDTO;
                            var a = after.coQGasTankDTO;

                            var newBTankM = _mapper.Map<TankMeasurement>(b);
                            newBTankM.MeasurementType = ReadingType.Before;

                            var newATankM = _mapper.Map<TankMeasurement>(a);
                            newATankM.MeasurementType = ReadingType.After;

                            var newTankMeasurement = new List<TankMeasurement>
                        {
                            newBTankM, newATankM
                        };

                            newCoqTank.TankMeasurement = newTankMeasurement;

                            coqTankList.Add(newCoqTank);
                        }
                    }

                    _context.COQTanks.AddRange(coqTankList);
                    #endregion

                    var totalBeforeWeightAir = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.TotalGasWeightAir);
                    var totalAfterWeightAir = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.TotalGasWeightAir);

                    var totalBeforeWeightVac = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.TotalGasWeightVAC);
                    var totalAfterWeightVac = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.TotalGasWeightVAC);

                    coq.MT_VAC = totalAfterWeightVac - totalBeforeWeightVac;
                    coq.MT_AIR = (double)(totalAfterWeightAir - totalBeforeWeightAir);

                    _context.CoQs.Update(coq);

                    #region Document Submission

                    var sDocumentList = new List<SubmittedDocument>();

                    //remove existing documents
                    var exisitngDocs = await _unitOfWork.SubmittedDocument.Find(s => s.Id.Equals(coq.Id));
                    if (exisitngDocs != null)
                    {
                        await _unitOfWork.SubmittedDocument.RemoveRange(exisitngDocs);
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }

                    model.SubmitDocuments.ForEach(x =>
                    {
                        var newSDoc = new SubmittedDocument
                        {
                            DocId = x.DocId,
                            FileId = x.FileId,
                            DocName = x.DocName,
                            DocSource = x.DocSource,
                            DocType = x.DocType,
                            ApplicationId = coq.Id,
                            ApplicationTypeId = appType.Id,
                        };

                        sDocumentList.Add(newSDoc);
                    });

                    _context.SubmittedDocuments.AddRange(sDocumentList);
                    #endregion

                    _context.SaveChanges();

                    var submit = await _flow.CoqWorkFlow(coq.Id, Enum.GetName(typeof(AppActions), AppActions.Resubmit), "COQ REsubmitted", user.Id);
                    if (submit.Item1)
                    {
                        var message = new Message
                        {
                            COQId = coq.Id,
                            Subject = $"COQ with reference {coq.Reference} Resubmitted",
                            Content = $"COQ with reference {coq.Reference} has been resubmitted to your desk for further processing",
                            UserId = user.Id,
                            Date = DateTime.Now.AddHours(1),
                        };

                        _context.Messages.Add(message);
                        _context.SaveChanges();
                        //_messageService.CreateMessageAsync(message);

                        transaction.Commit();

                        return new ApiResponse
                        {
                            Message = submit.Item2,
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        transaction.Rollback();
                        return new ApiResponse
                        {
                            Message = submit.Item2,
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Success = false
                        };

                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new ApiResponse
                    {
                        Message = $"An error occur, COQ not created: {ex.Message}",
                        Success = false,
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }
            }
            return new ApiResponse
            {
                Message = $"Coq not found",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public async Task<ApiResponse> CreateCOQForGas(CreateGasProductCoQDto model)
        {
            var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");

            var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name == Utils.COQ) ?? throw new Exception("Application type of COQ is not configured yet, please contact support");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                #region Create Coq
                var coq = new CoQ
                {
                    AppId = model.NoaAppId,
                    PlantId = model.PlantId,
                    Reference = Utils.GenerateCoQRefrenceCode(),
                   // PlantId = model.PlantId,
                    DateOfSTAfterDischarge = model.DateOfSTAfterDischarge,
                    DateOfVesselArrival = model.DateOfVesselArrival,
                    DateOfVesselUllage = model.DateOfVesselUllage,
                    DepotPrice = model.DepotPrice,
                    ProductId = model.ProductId,
                    ArrivalShipFigure = model.ArrivalShipFigure,
                    QuauntityReflectedOnBill = model.QuauntityReflectedOnBill,
                    DischargeShipFigure = model.DischargeShipFigure,
                    CreatedBy = user.Id,
                    Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing),
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    NameConsignee = model.NameConsignee,
                    //SubmittedDate = DateTime.UtcNow.AddHours(1),
                    
                };

                _context.CoQs.Add(coq);
                _context.SaveChanges();
                #endregion

                #region Create COQ Tank
                var coqTankList = new List<COQTank>();

                foreach (var before in model.TankBeforeReadings)
                {
                    var newCoqTank = new COQTank
                    {
                        CoQId = coq.Id,
                        TankId = before.TankId
                    };

                    var after = model.TankAfterReadings.FirstOrDefault(x => x.TankId == before.TankId);

                    if (after != null && before.coQGasTankDTO != null)
                    {
                        var b = before.coQGasTankDTO;
                        var a = after.coQGasTankDTO;

                        var newBTankM = _mapper.Map<TankMeasurement>(b);
                        newBTankM.MeasurementType = ReadingType.Before;

                        var newATankM = _mapper.Map<TankMeasurement>(a);
                        newATankM.MeasurementType = ReadingType.After;

                        var newTankMeasurement = new List<TankMeasurement>
                        {
                            newBTankM, newATankM
                        };

                        newCoqTank.TankMeasurement = newTankMeasurement;

                        coqTankList.Add(newCoqTank);
                    }
                }

                _context.COQTanks.AddRange(coqTankList);
                #endregion


                var totalBeforeWeightAir = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.TotalGasWeightAir);
                var totalAfterWeightAir = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.TotalGasWeightAir);

                var totalBeforeWeightVac = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.TotalGasWeightVAC);
                var totalAfterWeightVac = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.TotalGasWeightVAC);

                coq.MT_VAC = totalAfterWeightVac - totalBeforeWeightVac;
                coq.MT_AIR = (double)(totalAfterWeightAir - totalBeforeWeightAir);

                _context.CoQs.Update(coq);

                #region Document Submission

                //SubmitDocumentDto sDoc = model.SubmitDocuments.FirstOrDefault();
                //var sDocument = _mapper.Map<SubmittedDocument>(sDoc);

                var sDocumentList = new List<SubmittedDocument>();

                model.SubmitDocuments.ForEach(x =>
                {
                    var newSDoc = new SubmittedDocument
                    {
                        DocId = x.DocId,
                        FileId = x.FileId,
                        DocName = x.DocName,
                        DocSource = x.DocSource,
                        DocType = x.DocType,
                        ApplicationId = coq.Id,
                        ApplicationTypeId = appType.Id,
                    };

                    sDocumentList.Add(newSDoc);
                });

                _context.SubmittedDocuments.AddRange(sDocumentList);
                #endregion

                _context.SaveChanges();               

                var submit = await _flow.CoqWorkFlow(coq.Id, Enum.GetName(typeof(AppActions), AppActions.Submit), "COQ Submitted", user.Id);
                if (submit.Item1)
                {
                    var message = new Message
                    {
                        COQId = coq.Id,
                        Subject = $"COQ with reference {coq.Reference} Submitted",
                        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                        UserId = user.Id,
                        Date = DateTime.Now.AddHours(1),
                    };

                    _context.Messages.Add(message);
                    _context.SaveChanges();
                    //_messageService.CreateMessageAsync(message);

                    transaction.Commit();

                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    transaction.Rollback();
                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Success = false
                    };

                }
                
            }
            catch (Exception ex)
            { 
                transaction.Rollback();

                return new ApiResponse
                {
                    Message = $"An error occur, COQ not created: {ex.Message}",
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> EditCOQForLiquid(int id, CreateCoQLiquidDto model)
        {
            var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");
            var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name == Utils.COQ) ?? throw new Exception("Application type of COQ is not configured yet, please contact support");
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id == id);
                if(coq != null)
                {
                    coq.DateOfSTAfterDischarge = model.DateOfSTAfterDischarge;
                    coq.DateOfVesselArrival = model.DateOfVesselArrival;
                    coq.DateOfVesselUllage = model.DateOfVesselUllage;
                    coq.DepotPrice = model.DepotPrice;
                    coq.ProductId = model.ProductId;
                    coq.Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing);
                    coq.DateModified = DateTime.UtcNow.AddHours(1);
                    _context.CoQs.Update(coq);

                    var coqTanks = await _unitOfWork.CoQTank.Find(t => t.CoQId.Equals(id), "TankMeasurement");

                    if(coqTanks != null )
                    {
                        await _unitOfWork.CoQTank.RemoveRange(coqTanks);
                        _context.SaveChanges();

                        #region Create COQ Tank
                        var coqTankList = new List<COQTank>();

                        foreach (var before in model.TankBeforeReadings)
                        {
                            var newCoqTank = new COQTank
                            {
                                CoQId = coq.Id,
                                TankId = before.TankId
                            };

                            var after = model.TankAfterReadings.FirstOrDefault(x => x.TankId == before.TankId);

                            if (after != null && before.coQTankDTO != null)
                            {
                                var b = before.coQTankDTO;
                                var a = after.coQTankDTO;

                                //var newBTankM = _mapper.Map<TankMeasurement>(b);
                                var newBTankM = new TankMeasurement
                                {
                                    DIP = b.DIP,
                                    WaterDIP = b.DIP,
                                    TOV = b.TOV,
                                    WaterVolume = b.WaterVolume,
                                    FloatRoofCorr = b.FloatRoofCorr,
                                    GOV = b.GOV,
                                    Tempearture = b.Temperature,
                                    Density = b.Density,
                                    VCF = b.VCF,
                                };
                                newBTankM.MeasurementType = ReadingType.Before;

                                //var newATankM = _mapper.Map<TankMeasurement>(a);
                                var newATankM = new TankMeasurement
                                {
                                    DIP = a.DIP,
                                    WaterDIP = a.DIP,
                                    TOV = a.TOV,
                                    WaterVolume = a.WaterVolume,
                                    FloatRoofCorr = a.FloatRoofCorr,
                                    GOV = a.GOV,
                                    Tempearture = a.Temperature,
                                    Density = a.Density,
                                    VCF = a.VCF,
                                };
                                newATankM.MeasurementType = ReadingType.After;

                                var newTankMeasurement = new List<TankMeasurement>
                                {
                                    newBTankM, newATankM
                                };

                                newCoqTank.TankMeasurement = newTankMeasurement;

                                coqTankList.Add(newCoqTank);
                            }
                        }

                        _context.COQTanks.AddRange(coqTankList);
                        #endregion

                        coq.GSV = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.GSV) - coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.GSV);

                        coq.GOV = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.GOV) - coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.GOV);

                        coq.MT_VAC = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.MTVAC) - coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.MTVAC);

                        _context.CoQs.Update(coq);

                        #region Document Submission

                        //SubmitDocumentDto sDoc = model.SubmitDocuments.FirstOrDefault();
                        //var sDocument = _mapper.Map<SubmittedDocument>(sDoc);

                        var sDocumentList = new List<SubmittedDocument>();

                        //remove existing documents
                        var exisitngDocs = await _unitOfWork.SubmittedDocument.Find(s => s.Id.Equals(coq.Id));
                        if(exisitngDocs != null)
                        {
                            await _unitOfWork.SubmittedDocument.RemoveRange(exisitngDocs);
                            await _unitOfWork.SaveChangesAsync(user.Id);
                        }

                        model.SubmitDocuments.ForEach(x =>
                        {
                            var newSDoc = new SubmittedDocument
                            {
                                DocId = x.DocId,
                                FileId = x.FileId,
                                DocName = x.DocName,
                                DocSource = x.DocSource,
                                DocType = x.DocType,
                                ApplicationId = coq.Id,
                                ApplicationTypeId = appType.Id
                            };

                            sDocumentList.Add(newSDoc);
                        });

                        _context.SubmittedDocuments.AddRange(sDocumentList);
                        #endregion

                        _context.SaveChanges();

                        var submit = await _flow.CoqWorkFlow(coq.Id, Enum.GetName(typeof(AppActions), AppActions.Resubmit), "COQ Submitted", user.Id);
                        if (submit.Item1)
                        {
                            var message = new Message
                            {
                                COQId = coq.Id,
                                Subject = $"COQ with reference {coq.Reference} re-submitted",
                                Content = $"COQ with reference {coq.Reference} has been re-submitted to your desk for further processing",
                                UserId = user.Id,
                                Date = DateTime.UtcNow.AddHours(1),
                            };

                            _context.Messages.Add(message);
                            _context.SaveChanges();

                            transaction.Commit();

                            return new ApiResponse
                            {
                                Message = submit.Item2,
                                StatusCode = HttpStatusCode.OK,
                                Success = true
                            };
                        }
                        else
                        {
                            transaction.Rollback();
                            return new ApiResponse
                            {
                                Message = submit.Item2,
                                StatusCode = HttpStatusCode.NotAcceptable,
                                Success = false
                            };
                        }
                    }
                }
                return new ApiResponse
                {
                    Message = $"Invalid COQ",
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new ApiResponse
                {
                    Message = $"An error occur, COQ not created: {ex.Message}",
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> CreateCOQForLiquid(CreateCoQLiquidDto model)
        {
            var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");

            var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name == Utils.COQ) ?? throw new Exception("Application type of COQ is not configured yet, please contact support");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                #region Create Coq
                var coq = new CoQ
                {
                    AppId = model.NoaAppId,
                    PlantId = model.PlantId,
                    ProductId = model.ProductId,
                    Reference = Utils.GenerateCoQRefrenceCode(),
                    DateOfSTAfterDischarge = model.DateOfSTAfterDischarge,
                    DateOfVesselArrival = model.DateOfVesselArrival,
                    DateOfVesselUllage = model.DateOfVesselUllage,
                    DepotPrice = model.DepotPrice,
                    CreatedBy = user.Id,
                    Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing),
                    DateCreated = DateTime.UtcNow.AddHours(1),
                };

                _context.CoQs.Add(coq);
                _context.SaveChanges();
                #endregion

                #region Create COQ Tank
                var coqTankList = new List<COQTank>();

                foreach (var before in model.TankBeforeReadings)
                {
                    var newCoqTank = new COQTank
                    {
                        CoQId = coq.Id,
                        TankId = before.TankId
                    };

                    var after = model.TankAfterReadings.FirstOrDefault(x => x.TankId == before.TankId);

                    if (after != null && before.coQTankDTO != null)
                    {
                        var b = before.coQTankDTO;
                        var a = after.coQTankDTO;

                        //var newBTankM = _mapper.Map<TankMeasurement>(b);
                        var newBTankM = new TankMeasurement
                        {
                            DIP = b.DIP,
                            WaterDIP = b.DIP,
                            TOV = b.TOV,
                            WaterVolume = b.WaterVolume,
                            FloatRoofCorr = b.FloatRoofCorr,
                            GOV = b.GOV,
                            Tempearture = b.Temperature,
                            Density = b.Density,
                            VCF = b.VCF,
                        };
                        newBTankM.MeasurementType = ReadingType.Before;

                        //var newATankM = _mapper.Map<TankMeasurement>(a);
                        var newATankM = new TankMeasurement
                        {
                            DIP = a.DIP,
                            WaterDIP = a.DIP,
                            TOV = a.TOV,
                            WaterVolume = a.WaterVolume,
                            FloatRoofCorr = a.FloatRoofCorr,
                            GOV = a.GOV,
                            Tempearture = a.Temperature,
                            Density = a.Density,
                            VCF = a.VCF,
                        };
                        newATankM.MeasurementType = ReadingType.After;

                        var newTankMeasurement = new List<TankMeasurement>
                        {
                            newBTankM, newATankM
                        };

                        newCoqTank.TankMeasurement = newTankMeasurement;

                        coqTankList.Add(newCoqTank);
                    }
                }

                _context.COQTanks.AddRange(coqTankList);
                #endregion

                coq.GSV = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.GSV) - coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.GSV);

                coq.GOV = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.GOV) - coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.GOV);

                coq.MT_VAC = coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.After).Sum(t => t.MTVAC) - coqTankList.SelectMany(x => x.TankMeasurement).Where(x => x.MeasurementType == ReadingType.Before).Sum(t => t.MTVAC);

                _context.CoQs.Update(coq);

                #region Document Submission

                //SubmitDocumentDto sDoc = model.SubmitDocuments.FirstOrDefault();
                //var sDocument = _mapper.Map<SubmittedDocument>(sDoc);

                var sDocumentList = new List<SubmittedDocument>();

                model.SubmitDocuments.ForEach(x =>
                {
                    var newSDoc = new SubmittedDocument
                    {
                        DocId = x.DocId,
                        FileId = x.FileId,
                        DocName = x.DocName,
                        DocSource = x.DocSource,
                        DocType = x.DocType,
                        ApplicationId = coq.Id,
                        ApplicationTypeId = appType.Id,
                    };

                    sDocumentList.Add(newSDoc);
                });

                _context.SubmittedDocuments.AddRange(sDocumentList);
                #endregion

                _context.SaveChanges();

                var submit = await _flow.CoqWorkFlow(coq.Id, Enum.GetName(typeof(AppActions), AppActions.Submit), "COQ Submitted", user.Id);
                if (submit.Item1)
                {
                    var message = new Message
                    {
                        COQId = coq.Id,
                        Subject = $"COQ with reference {coq.Reference} Submitted",
                        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                        UserId = user.Id,
                        Date = DateTime.UtcNow.AddHours(1),
                    };

                    _context.Messages.Add(message);
                    _context.SaveChanges();

                    transaction.Commit();

                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    transaction.Rollback();
                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Success = false
                    };

                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new ApiResponse
                {
                    Message = $"An error occur, COQ not created: {ex.Message}",
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> GetCoqCreateRequirementsAsync(int depotId, int? appId)
        {
            if (appId is not null) // Depot CoQ
            {
                var appDepot = await _context.ApplicationDepots.Include(c => c.Product)
                    .Include(c => c.Application.Facility)
                    .Include(c => c.Application.User)
                    .Include(c => c.Depot)
                    .FirstOrDefaultAsync(c => c.AppId == appId && c.DepotId == depotId);
                if (appDepot?.Depot is null)
                {
                    return new()
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Depot not found in the application"
                    };
                }
                var plant = await _context.Plants
                    .Include(p => p.Tanks)
                    .FirstOrDefaultAsync(t => t.Id == appDepot.DepotId);
                if (plant is null)
                {
                    return new()
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Depot not found in the application"
                    };
                }

                var tanks = plant.Tanks.Select(t => new TankDTO()
                {
                    Id = t.PlantTankId,
                    Name = t.TankName
                }).ToList();

                var docData = (await GetCoqRequiredDocuments(appDepot.Application) as dynamic)?.Data;



                var data = new CreateCoqGetDTO
                {
                    ProductName = appDepot.Product.Name,
                    ProductType = appDepot.Product.ProductType,
                    Tanks = tanks,
                    RequiredDocuments = docData.Docs as List<SubmittedDocument>,
                    ApiData = docData.ApiData
                };
                return new()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = data,
                    Message = "fetched successfully"
                };
            }
            else // Proceccing Plant Coq
            {
                var plant = await _context.Plants
                    .Include(p => p.Tanks)
                    .FirstOrDefaultAsync(t => t.Id == depotId);
                if (plant is null)
                {
                    return new()
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "processing plant not found"
                    };
                }

                var tanks = plant.Tanks.Select(t => new TankDTO()
                {
                    Id = t.PlantTankId,
                    Name = t.TankName
                }).ToList();

                var docData = (await GetCoqRequiredDocuments(plant) as dynamic)?.Data;
                var data = new CreateCoqGetDTO
                {

                    Tanks = tanks,
                    RequiredDocuments = docData.Docs as List<SubmittedDocument>,
                    ApiData = docData.ApiData
                };
                return new()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = data,
                    Message = "fetched successfully"
                };
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(c => c.Id == id, "Application.Facility,Application.User");
                var gastankList = new List<GasTankReadingsPerCoQ>();
                var liqtankList = new List<LiquidTankReadingsPerCoQ>();
                var product = (await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.AppId.Equals(coq.AppId) && x.DepotId.Equals(coq.PlantId), "Product")).Product;

                if (coq is null)
                    return new() { StatusCode = HttpStatusCode.NotFound, Success = false };

                var appHistories = await _unitOfWork.COQHistory.Find(h => h.COQId.Equals(coq.Id));
                if(appHistories.Count() > 0)
                {
                    var users = _userManager.Users.Include(c => c.Company).Include(ur => ur.UserRoles).ThenInclude(r => r.Role);
                    appHistories.OrderByDescending(x => x.Id).ToList().ForEach(h =>
                    {

                        var t = users.FirstOrDefault(x => x.Id.Equals(h.TriggeredBy));
                        var r = users.FirstOrDefault(x => x.Id.Equals(h.TargetedTo));
                        h.TriggeredBy = t.Email;
                        h.TriggeredByRole = t.UserRoles.FirstOrDefault().Role.Name;
                        h.TargetedTo = r.Email;
                        h.TargetRole = r.UserRoles.FirstOrDefault().Role.Name;
                    });
                }

                if (product != null)
                {
                    switch (product.ProductType.ToLower())
                    {
                        case "gas":
                            gastankList = await _context.COQTanks
                                                .Include(c => c.TankMeasurement).Where(c => c.CoQId == coq.Id)
                                                .Select(c => new GasTankReadingsPerCoQ
                                                {
                                                    TankId = c.TankId,
                                                    Id = c.Id,
                                                    TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == c.TankId).TankName,
                                                    CoQId = c.CoQId,
                                                    TankMeasurement = c.TankMeasurement.Select(m => new CreateCoQGasTankDTO
                                                    {
                                                        LiquidDensityVac = m.LiquidDensityVac,
                                                        MolecularWeight = m.MolecularWeight,
                                                        ObservedLiquidVolume = m.ObservedLiquidVolume,
                                                        ObservedSounding = m.ObservedSounding,
                                                        LiquidTemperature = (double)m.Tempearture,
                                                        ShrinkageFactorLiquid = m.ShrinkageFactorLiquid,
                                                        ShrinkageFactorVapour = m.ShrinkageFactorVapour,
                                                        TapeCorrection = m.TapeCorrection,
                                                        VapourFactor = m.VapourFactor,
                                                        VapourPressure = m.VapourPressure,
                                                        VapourTemperature = m.VapourTemperature,
                                                        TankVolume = m.TankVolume,
                                                        VCF = m.VCF,
                                                        MeasurementType = m.MeasurementType
                                                    }).ToList()
                                                }).ToListAsync();
                            break;
                        default:
                            liqtankList = await _context.COQTanks
                                                 .Include(c => c.TankMeasurement).Where(c => c.CoQId == coq.Id)
                                                 .Select(c => new LiquidTankReadingsPerCoQ
                                                 {
                                                     TankId = c.TankId,
                                                     Id = c.Id,
                                                     CoQId = c.CoQId,
                                                     TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == c.TankId).TankName,
                                                     TankMeasurement = c.TankMeasurement.Select(m => new CreateCoQLiquidTankDto
                                                     {
                                                         Density = m.Density,
                                                         DIP = m.DIP,
                                                         FloatRoofCorr = m.FloatRoofCorr,
                                                         GOV = m.GOV,
                                                         MeasurementType = m.MeasurementType,
                                                         Temperature = m.Tempearture,
                                                         TOV = m.TOV,
                                                         VCF = m.VCF,
                                                         WaterDIP = m.WaterDIP,
                                                         WaterVolume = m.WaterVolume
                                                     }).ToList()
                                                 })
                                                 .ToListAsync();
                            break;
                    }
                }
                var coqData = new CoQsDataDTO()
                {
                    Id = coq.Id,
                    PlantId = coq.PlantId,
                    Vessel = new Vessel(),
                    DateOfSTAfterDischarge = coq.DateOfSTAfterDischarge,
                    DateOfVesselArrival = coq.DateOfVesselArrival,
                    DateOfVesselUllage = coq.DateOfVesselUllage,
                    ArrivalShipFigure = coq.ArrivalShipFigure,
                    DepotPrice = coq.DepotPrice,
                    MT_AIR = coq.MT_AIR,
                    MT_VAC = coq.MT_VAC,
                    GOV = coq.GOV,
                    GSV = coq.GSV,
                    Status = coq.Status,
                    AppId = coq.AppId,
                    Reference = coq.Reference,
                    NameConsignee = coq.NameConsignee,
                    DischargeShipFigure = coq.DischargeShipFigure,
                    QuauntityReflectedOnBill = coq.QuauntityReflectedOnBill
                };
                if (coq.AppId != null || coq.Reference != null)
                {
                    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(coq.AppId) || x.Reference == coq.Reference, "Facility.VesselType");
                    var jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == app.Jetty)?.Name;
                    if (app != null)
                    {
                        coqData.Reference = app.Reference;
                        coqData.MarketerName = app?.MarketerName ?? string.Empty;
                        coqData.MotherVessel = app.MotherVessel;
                        coqData.Jetty = jetty;
                        coqData.LoadingPort = app.LoadingPort;
                        coqData.Vessel.Name = app.Facility.Name;
                        coqData.Vessel.VesselType = app.Facility?.VesselType?.Name ?? string.Empty;
                        coqData.NominatedSurveyor = (await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(c => c.Id == app.SurveyorId)).Name;
                        coqData.AppId = app.Id;
                        coqData.ApplicationTypeId = app.ApplicationTypeId;
                        coqData.ApplicationType = Enum.GetName(typeof(AppTypes), AppTypes.COQ);
                    }
                }
                coqData.ProductType = product.ProductType;
                coqData.CurrentDesk = _userManager.Users.FirstOrDefault(u => u.Id.Equals(coq.CurrentDeskId)).Email;
                coqData.Plant = _context.Plants.FirstOrDefault(p => p.Id.Equals(coq.PlantId)).Name;
                var docData = (await GetCoqRequiredDocuments(coq.Application) as dynamic)?.Data;

                if (product.ProductType != null && product.ProductType.ToLower().Equals("gas"))
                    return new()
                    {
                        Success = true,
                        StatusCode = HttpStatusCode.OK,
                        Data = new
                        {
                            coq = coqData,
                            tankList = gastankList,
                            docs = docData.Docs as List<SubmittedDocument>,
                            docData.ApiData,
                            appHistories
                        }
                    };
                else
                    return new()
                    {
                        Success = true,
                        StatusCode = HttpStatusCode.OK,
                        Data = new
                        {
                            coq = coqData,
                            tankList = liqtankList,
                            docs = docData.Docs as List<SubmittedDocument>,
                            docData.ApiData,
                            appHistories
                        }
                    };
            }
            catch (Exception ex)
            {
                return new()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse> GetApprovedCoQsByFieldOfficer()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");
                var FieldOfficer = _httpCxtAccessor.HttpContext.User.IsInRole(RoleConstants.Field_Officer);
                if (FieldOfficer == true)
                {
                    var depotsList = GetDepotsListforUSer(user.Id);

                    List<CoQDTO> Coqs = new List<CoQDTO>();
                    foreach (var item in depotsList)
                    {
                        var coqPerDepot = GetCoqApproved(item.PlantID);
                        if (coqPerDepot is not null)
                        {
                            Coqs.Add(coqPerDepot);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    _apiReponse = new ApiResponse
                    {
                        Success = true,
                        StatusCode = HttpStatusCode.OK,
                        Data = Coqs,
                    };
                }
                else
                {
                    _apiReponse = new ApiResponse
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.MethodNotAllowed,
                        Message = "Sorry only a Field Officer is allowed",
                        Data = null
                        
                    };
                }
               

                return _apiReponse;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResponse> GetAllCoQsByFieldOfficer()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");
                var FieldOfficer = _httpCxtAccessor.HttpContext.User.IsInRole(RoleConstants.Field_Officer);
                if (FieldOfficer == true)
                {
                    var depotsList = GetDepotsListforUSer(user.Id);

                    List<CoQDTO> Coqs = new List<CoQDTO>();
                    foreach (var item in depotsList)
                    {
                        var coqPerDepot = GetAllCoQs(item.PlantID);
                        if (coqPerDepot is not null)
                        {
                            Coqs.Add(coqPerDepot);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    _apiReponse = new ApiResponse
                    {
                        Success = true,
                        StatusCode = HttpStatusCode.OK,
                        Data = Coqs,
                    };
                }
                else
                {
                    _apiReponse = new ApiResponse
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.MethodNotAllowed,
                        Message = "Sorry only a Field Officer is allowed",
                        Data = null

                    };
                }


                return _apiReponse;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResponse> GetAllCoQCertificates()
        {
            try
            {
                var certList = await _unitOfWork.COQCertificate.GetAll("COQ.Application.User.Company,COQ.Application.Facility.VesselType,COQ,COQ.Plant");
                int count = certList.Count();
                _apiReponse = new ApiResponse
                {
                    Message = count > 0 ? "Success, Permit Found" : "Permit Not Found",
                    StatusCode = count > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    Success = count > 0 ? true : false,
                    Data = new
                    {
                        DepotCOQList = certList.OrderByDescending(d => d.IssuedDate).Select(x => new
                        {
                            x.Id,
                            x.COQId,
                            x.COQ.AppId,
                            DepotName = $"{x.COQ.Plant.Name}({x.COQ.Plant.State})",
                            CompanyName = x.COQ.Application.User.Company.Name,
                            LicenseNo = x.CertifcateNo,
                            IssuedDate = x.IssuedDate.ToString("MMM dd, yyyy HH:mm:ss"),
                            ExpiryDate = x.ExpireDate.ToString("MMM dd, yyyy HH:mm:ss"),
                            x.COQ.Application.User.Email,
                            VesselTypeType = x.COQ.Application.Facility.VesselType.Name,
                            VesselName = x.COQ.Application.Facility.Name,
                        }),
                        PPCoQList = (string)null
                    }
                };
            }
            catch (Exception e)
            {
                _apiReponse = new ApiResponse { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = e.Message };                
            }

            return _apiReponse;
        }
       
        private List<PlantFieldOfficer> GetDepotsListforUSer(string Id)
        {
            var plist =  _context.PlantFieldOfficers.Where(x => x.OfficerID.ToString() == Id).ToList();
            return plist;
        }

        public async Task<ApiResponse> ViewCoQCertificate(int coqId)
        {
            try
            {
                var dataforView = await GetCoQCertificate(coqId);
                if (dataforView == null)
                {
                    _apiReponse = new ApiResponse
                    {
                        Message = "Unable to complete request",
                        StatusCode = HttpStatusCode.NoContent,
                        Data = null,
                        Success = false
                    };
                    return _apiReponse;
                }
                _apiReponse= new ApiResponse { Success = true, StatusCode = HttpStatusCode.OK,Data = dataforView, Message = "Data returned"};

                return _apiReponse;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResponse> ViewCoQGasCertificate(int coqId)
        {
            try
            {
                var dataforView = GetCOQGasCertficate(coqId);
                if (dataforView == null)
                {
                    _apiReponse = new ApiResponse
                    {
                        Message = "Unable to complete request",
                        StatusCode = HttpStatusCode.NoContent,
                        Data = null,
                        Success = false
                    };
                    return _apiReponse;
                }
                _apiReponse = new ApiResponse { Success = true, StatusCode = HttpStatusCode.OK, Data = dataforView, Message = "Data returned" };

                return _apiReponse;

            }
            catch (Exception)
            {

                throw;
            }    
        }

        private CoQDTO GetCoqApproved(int Id)
        {
            var plist = _context.CoQs.FirstOrDefault(x => x.PlantId == Id && x.Status == "Approved");
            if (plist == null)
            {
                return new CoQDTO();
            }
            CoQDTO cd = new CoQDTO
            {
                NoaAppId = plist.AppId,
                PlantId = plist.PlantId,
                ArrivalShipFigure = plist.ArrivalShipFigure,
                DateOfSTAfterDischarge = plist.DateOfSTAfterDischarge,
                DateOfVesselArrival = plist.DateOfVesselArrival,
                DateOfVesselUllage = plist.DateOfVesselUllage,
                DepotPrice = plist.DepotPrice,
                DischargeShipFigure = plist.DischargeShipFigure,
                NameConsignee = plist.NameConsignee,
                QuauntityReflectedOnBill = plist.QuauntityReflectedOnBill
               


            };
            return cd;
        }

        private CoQDTO GetAllCoQs(int Id)
        {
            var plist = _context.CoQs.FirstOrDefault(x => x.PlantId == Id);
            if (plist == null)
            {
                return new CoQDTO();
            }
            CoQDTO cd = new CoQDTO
            {
                NoaAppId = plist.AppId,
                PlantId = plist.PlantId,
                ArrivalShipFigure = plist.ArrivalShipFigure,
                DateOfSTAfterDischarge = plist.DateOfSTAfterDischarge,
                DateOfVesselArrival = plist.DateOfVesselArrival,
                DateOfVesselUllage = plist.DateOfVesselUllage,
                DepotPrice = plist.DepotPrice,
                DischargeShipFigure = plist.DischargeShipFigure,
                NameConsignee = plist.NameConsignee,
                QuauntityReflectedOnBill = plist.QuauntityReflectedOnBill

            };
            return cd ;
        }

        private async Task<dynamic> GetCoQCertificate(int coqId)
        {
            try
            {
                var cqs = _context.COQCertificates.Include(x => x.COQ.Plant).Include(n => n.COQ.Application).FirstOrDefault(x => x.COQId == coqId);
                if(cqs != null)
                {
                    var product = (await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.AppId.Equals(cqs.COQ.AppId) && x.DepotId.Equals(cqs.COQ.PlantId), "Product")).Product;
                    var tnks = _context.PlantTanks.Where(x => x.PlantId == cqs.COQ.Plant.Id).ToList();
                    var coQTanks = _context.COQTanks.Include(t => t.TankMeasurement)
                        .Where(c => c.CoQId == coqId).ToList();

                    var jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == cqs.COQ.Application.Jetty)?.Name;
                    var fieldofficer = _userManager.FindByIdAsync(cqs.COQ.CreatedBy).Result;
                    //var coqHistory = _unitOfWork.COQHistory.Find(x => x.TargetRole)

                    if (product.ProductType.Equals(Enum.GetName(typeof(ProductTypes), ProductTypes.Gas)))
                    {
                        var dat = new COQGASCertificateDTO
                        {
                            CompanyName = cqs.COQ.Plant.Company,
                            DateOfVesselArrival = cqs.COQ.DateOfVesselArrival,
                            DateOfVessselUllage = cqs.COQ.DateOfVesselUllage,
                            Jetty = jetty,
                            MotherVessel = cqs.COQ.Application?.MotherVessel ?? string.Empty,
                            ReceivingTerminal = cqs.COQ.Plant.Name,
                            VesselName = cqs.COQ.Application?.VesselName ?? string.Empty,
                            Cosignee = cqs.COQ.NameConsignee ?? string.Empty,
                            DepotPrice = cqs.COQ.DepotPrice,
                            GOV = cqs.COQ.GOV,
                            GSV = cqs.COQ.GSV,
                            MTVAC = cqs.COQ.MT_VAC,
                            DateAfterDischarge = cqs.COQ.DateOfSTAfterDischarge,
                            QuantityReflectedOnBill = cqs.COQ.QuauntityReflectedOnBill,
                            ArrivalShipFigure = cqs.COQ.ArrivalShipFigure,
                            DischargeShipFigure = cqs.COQ.DischargeShipFigure,
                            FieldOfficerName = $"{fieldofficer.FirstName} {fieldofficer.LastJobDate}",
                            FieldOfficerSignature = $"{fieldofficer.Signature}"
                        };
                        dat = GetGasCOQCalculationList(tnks, coQTanks, dat);
                        dat.Product = product.Name;
                        dat.ProductType = product.ProductType;
                        return dat;
                    }
                    else
                    {
                        var dat = new COQNonGasCertificateDTO
                        {
                            CompanyName = cqs.COQ.Plant.Company,
                            DateOfVesselArrival = cqs.COQ.DateOfVesselArrival,
                            DateOfVessselUllage = cqs.COQ.DateOfVesselUllage,
                            Jetty = jetty,
                            MotherVessel = cqs.COQ.Application?.MotherVessel ?? string.Empty,
                            ReceivingTerminal = cqs.COQ.Plant.Name,
                            VesselName = cqs.COQ.Application?.VesselName ?? string.Empty,
                            Cosignee = cqs.COQ.NameConsignee ?? string.Empty,
                            DepotPrice = cqs.COQ.DepotPrice,
                            DateAfterDischarge = cqs.COQ.DateOfSTAfterDischarge,
                            FieldOfficerName = $"{fieldofficer.FirstName} {fieldofficer.LastJobDate}",
                            FieldOfficerSignature = $"{fieldofficer.Signature}",
                        };
                        dat = GetNonGasCOQCalculationList(tnks, coQTanks, dat);
                        dat.Product = product.Name;
                        dat.ProductType = product.ProductType;

                        return dat;
                    }
                }
                
            }
            catch (Exception e)
            {

            }
            return null;
        }

        private COQGASCertificateDTO GetGasCOQCalculationList(List<PlantTank> tnks, List<COQTank> coQTanks, COQGASCertificateDTO dat)
        {
            var tankList = new List<CoQTanksDTO>();
            foreach (var item in coQTanks)
            {
                var tr = new CoQTanksDTO
                {
                    AfterTankMeasurement = item.TankMeasurement.Where(t => t.MeasurementType == ReadingType.After)
                    .Select(tt => new CoQTankAfterReading
                    {
                        TankId = item.TankId,
                        coQCertTank = new CoqCertTank
                        {
                            Density = tt.Density,
                            DIP = tt.DIP,
                            MeasurementType = tt.MeasurementType,
                            FloatRoofCorr = tt.FloatRoofCorr,
                            GOV = tt.GOV,
                            //GSV = tt.GSV,
                            MTVAC = tt.MTVAC,
                            Tempearture = tt.Tempearture,
                            TOV = tt.TOV,
                            Vcf = tt.VCF,
                            WaterDIP = tt.WaterDIP,
                            WaterVolume = tt.WaterVolume,
                            LiquidDensityVac = tt.LiquidDensityVac,
                            MolecularWeight = tt.MolecularWeight,
                            ShrinkageFactorVapour = tt.ShrinkageFactorVapour,
                            LiquidTemperature = (double)tt.LiquidTemperature,
                            VapourTemperature = (double)tt.VapourTemperature,
                            ObservedLiquidVolume = tt.ObservedLiquidVolume,
                            ObservedSounding = tt.ObservedSounding,
                            ShrinkageFactorLiquid = tt.ShrinkageFactorLiquid,
                            TankVolume = tt.TankVolume,
                            TapeCorrection = tt.TapeCorrection,
                            VapourFactor = tt.VapourFactor,
                            VapourPressure = tt.VapourPressure
                        }
                    }
                    ).ToList(),
                    BeforeTankMeasurements = item.TankMeasurement.Where(t => t.MeasurementType == ReadingType.Before)
                    .Select(tt => new CoQTankBeforeReading
                    {
                        TankId = item.TankId,
                        coQCertTank = new CoqCertTank
                        {
                            Density = tt.Density,
                            DIP = tt.DIP,
                            MeasurementType = tt.MeasurementType,
                            FloatRoofCorr = tt.FloatRoofCorr,
                            GOV = tt.GOV,
                            //GSV = tt.GSV,
                            MTVAC = tt.MTVAC,
                            Tempearture = tt.Tempearture,
                            TOV = tt.TOV,
                            Vcf = tt.VCF,
                            WaterDIP = tt.WaterDIP,
                            WaterVolume = tt.WaterVolume,
                            LiquidDensityVac = tt.LiquidDensityVac,
                            //LiquidTemperature = tt.LiquidTemperature
                            MolecularWeight = tt.MolecularWeight,
                            ShrinkageFactorVapour = tt.ShrinkageFactorVapour,
                            LiquidTemperature = (double)tt.LiquidTemperature,
                            VapourTemperature = (double)tt.VapourTemperature,
                            ObservedLiquidVolume = tt.ObservedLiquidVolume,
                            ObservedSounding = tt.ObservedSounding,
                            ShrinkageFactorLiquid = tt.ShrinkageFactorLiquid,
                            TankVolume = tt.TankVolume,
                            TapeCorrection = tt.TapeCorrection,
                            VapourFactor = tt.VapourFactor,
                            VapourPressure = tt.VapourPressure
                        }
                    }
                    ).ToList(),
                    TankName = tnks.FirstOrDefault(t => t.PlantTankId == item.TankId).TankName
                };
                tankList.Add(tr);
            }

            dat.tanks = tankList;
            dat.TotalBeforeWeightAir = tankList.SelectMany(x => x.BeforeTankMeasurements).Sum(t => t.coQCertTank.TotalGasWeightAir);
            dat.TotalAfterWeightAir = tankList.SelectMany(x => x.AfterTankMeasurement).Sum(t => t.coQCertTank.TotalGasWeightAir);
            dat.TotalBeforeWeightVac = tankList.SelectMany(x => x.BeforeTankMeasurements).Sum(t => t.coQCertTank.TotalGasWeightVAC);
            dat.TotalAfterWeightVac = tankList.SelectMany(x => x.AfterTankMeasurement).Sum(t => t.coQCertTank.TotalGasWeightVAC);

            return dat;
        }

        private COQNonGasCertificateDTO GetNonGasCOQCalculationList(List<PlantTank> tnks, List<COQTank> coQTanks, COQNonGasCertificateDTO dat)
        {
            var tankList = new List<CoQNonGasTanksDTO>();
            foreach (var item in coQTanks)
            {
                //var  tanks = new List<CoQTanksDTO>

                var tr = new CoQNonGasTanksDTO
                {
                    AfterTankMeasurement = item.TankMeasurement.Where(t => t.MeasurementType == ReadingType.After)
                    .Select(tt => new COQTankNonGasAfter
                    {
                        TankId = item.TankId,
                        CoQCertTankNonGas = new CoQCertTankNonGas
                        {
                            Density = tt.Density,
                            DIP = tt.DIP,
                            MeasurementType = tt.MeasurementType,
                            FloatRoofCorr = tt.FloatRoofCorr,
                            GOV = tt.GOV,
                            MTVAC = tt.MTVAC,
                            Tempearture = tt.Tempearture,
                            TOV = tt.TOV,
                            Vcf = tt.VCF,
                            WaterDIP = tt.WaterDIP,
                            WaterVolume = tt.WaterVolume
                        }
                    }
                    ).ToList(),
                    BeforeTankMeasurements = item.TankMeasurement.Where(t => t.MeasurementType == ReadingType.Before)
                    .Select(tt => new COQTankNonGasBefore
                    {
                        TankId = item.TankId,
                        CoQCertTankNonGas = new CoQCertTankNonGas
                        {
                            Density = tt.Density,
                            DIP = tt.DIP,
                            MeasurementType = tt.MeasurementType,
                            FloatRoofCorr = tt.FloatRoofCorr,
                            GOV = tt.GOV,
                            MTVAC = tt.MTVAC,
                            Tempearture = tt.Tempearture,
                            TOV = tt.TOV,
                            Vcf = tt.VCF,
                            WaterDIP = tt.WaterDIP,
                            WaterVolume = tt.WaterVolume
                        }
                    }
                    ).ToList(),
                    TankName = tnks.FirstOrDefault(t => t.PlantTankId == item.TankId).TankName
                };
                tankList.Add(tr);
            }
            dat.tanks = tankList;
            dat.GSV = tankList.SelectMany(x => x.AfterTankMeasurement).Sum(t => t.CoQCertTankNonGas.GSV) - tankList.SelectMany(x => x.BeforeTankMeasurements).Sum(t => t.CoQCertTankNonGas.GSV);

            dat.GOV = tankList.SelectMany(x => x.AfterTankMeasurement).Sum(t => t.CoQCertTankNonGas.GOV) - tankList.SelectMany(x => x.BeforeTankMeasurements).Sum(t => t.CoQCertTankNonGas.GOV);

            dat.MTVAC = tankList.SelectMany(x => x.AfterTankMeasurement).Sum(t => t.CoQCertTankNonGas.MTVAC) - tankList.SelectMany(x => x.BeforeTankMeasurements).Sum(t => t.CoQCertTankNonGas.MTVAC);
            return dat;
        }

        private COQGasCertficateDTO GetCOQGasCertficate(int coqId)
        {
            try
            {
                var cqs = _context.COQCertificates.Include(x => x.COQ.Plant).Include(n => n.COQ.Application).FirstOrDefault(x => x.Id == coqId);
                var tnks = _context.PlantTanks.Where(x => x.PlantId == cqs.COQ.Plant.Id).ToList();
                var coQTanks = _context.COQTanks.Include(t => t.TankMeasurement)
                    .Where(c => c.CoQId == coqId).ToList();

                string jetty = null;
                if(cqs.COQ.Application?.Jetty > 0)
                {
                    jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == cqs.COQ.Application.Jetty)?.Name;
                }
                var dat = new COQGasCertficateDTO
                {
                    CompanyName = cqs.COQ.Plant.Company,
                    DateOfVesselArrival = cqs.COQ.DateOfVesselArrival,
                    ShoreDate = cqs.COQ.DateOfVesselUllage,
                    Jetty = jetty,
                    Consignee = cqs.COQ.NameConsignee,
                    Product = tnks.FirstOrDefault().Product,
                    ReceivingTerminal = cqs.COQ.Plant.Name,
                    VesselName = cqs.COQ.Application?.VesselName ?? string.Empty,
                    QuantityReflectedOnBill = cqs.COQ.QuauntityReflectedOnBill,
                    ArrivalShipFigure = cqs.COQ.ArrivalShipFigure,
                    DischargeShipFigure = cqs.COQ.DischargeShipFigure
                    
                };
                var tankList = new List<COQGasTankDTO>();

                foreach (var item in coQTanks)
                {
                    //var  tanks = new List<CoQTanksDTO>

                    var tr = new COQGasTankDTO
                    {
                        AfterMeasurements = item.TankMeasurement.Select(
                                             tt => new COQGastTankAfter
                                             {
                                                 TankId = item.TankId,
                                                 coQGasTank = new CoQGasTank
                                                 {
                                                     LiquidDensityVac = tt.LiquidDensityVac,
                                                     //LiquidTemperature = tt.LiquidTemperature
                                                     MolecularWeight = tt.MolecularWeight,
                                                     MeasurementType = tt.MeasurementType,
                                                     ObservedLiquidVolume = tt.ObservedLiquidVolume,
                                                     ObservedSounding = tt.ObservedSounding,
                                                     ShrinkageFactorLiquid = tt.ShrinkageFactorLiquid,
                                                     TankVolume = tt.TankVolume,
                                                     TapeCorrection = tt.TapeCorrection,
                                                     VapourFactor = tt.VapourFactor,
                                                     VapourPressure = tt.VapourPressure,
                                                     Vcf = tt.VCF
                                                     
                                                 }
                                             }
                                          ).Where(t => t.coQGasTank.MeasurementType == ReadingType.After).ToList(),
                        BeforeMeasuremnts = item.TankMeasurement.Select(
                                             tt => new COQGastTankBefore
                                             {
                                                 TankId = item.TankId,
                                                 coQGasTank = new CoQGasTank
                                                 {
                                                     LiquidDensityVac = tt.LiquidDensityVac,
                                                     //LiquidTemperature = tt.LiquidTemperature
                                                     MolecularWeight = tt.MolecularWeight,
                                                     MeasurementType = tt.MeasurementType,
                                                     ObservedLiquidVolume = tt.ObservedLiquidVolume,
                                                     ObservedSounding = tt.ObservedSounding,
                                                     ShrinkageFactorLiquid = tt.ShrinkageFactorLiquid,
                                                     TankVolume = tt.TankVolume,
                                                     TapeCorrection = tt.TapeCorrection,
                                                     VapourFactor = tt.VapourFactor,
                                                     VapourPressure = tt.VapourPressure,
                                                     Vcf = tt.VCF

                                                 }

                                             }
                                          ).Where(t => t.coQGasTank.MeasurementType == ReadingType.Before).ToList(),
                        TankName = _context.PlantTanks.FirstOrDefault(t => t.PlantTankId == item.TankId).TankName
                    };
                    tankList.Add(tr);
                }

                dat.tanks = tankList;
                dat.TotalBeforeWeightAir = tankList.SelectMany(x => x.BeforeMeasuremnts).Sum(t => t.coQGasTank.TotalGasWeightAir);
                dat.TotalAfterWeightAir = tankList.SelectMany(x => x.AfterMeasurements).Sum(t => t.coQGasTank.TotalGasWeightAir);
                dat.TotalBeforeWeightVac = tankList.SelectMany(x => x.BeforeMeasuremnts).Sum(t => t.coQGasTank.TotalGasWeightVAC);
                dat.TotalAfterWeightVac = tankList.SelectMany(x => x.AfterMeasurements).Sum(t => t.coQGasTank.TotalGasWeightVAC);
                return dat;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
