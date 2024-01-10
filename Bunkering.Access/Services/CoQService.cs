using AutoMapper;
using Azure;
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
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
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


        public CoQService(IUnitOfWork unitOfWork, IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, IOptions<AppSetting> setting, IMapper mapper, IElps elps, WorkFlowService flow, ApplicationContext context)
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

        public async Task<ApiResponse> GetCoQsByDepotId(int depotId)
        {
            try
            {
                var foundCOQ = await _unitOfWork.CoQ.Find(x => x.DepotId == depotId);
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
                        Message = "COQ Application has been pushed",
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
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(c => c.Id == id, "Depot");


            if (coq is not null)
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(a => a.Id == coq.AppId);
                if (app is null)
                {
                    return new ApiResponse
                    {
                        Message = "Application Not Found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                }
                //var price = coq.MT_VAC * coq.DepotPrice;
                //var result = new DebitNoteDTO(
                //coq.DateOfSTAfterDischarge,
                //coq.DateOfSTAfterDischarge.AddDays(21),
                //app.MarketerName,
                //coq.Depot!.Name,
                //price,
                //coq.DepotPrice * 0.01m,
                //coq.Depot!.Capacity,
                //price / coq.Depot!.Capacity
                //);
                return new ApiResponse
                {
                    Message = $"Debit note fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    //Data = result
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
                    DepotId = model.PlantId,
                    DateOfSTAfterDischarge = model.DateOfSTAfterDischarge,
                    DateOfVesselArrival = model.DateOfVesselArrival,
                    DateOfVesselUllage = model.DateOfVesselUllage,
                    DepotPrice = model.DepotPrice,
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

            //return new ApiResponse
            //{
            //    Message = "COQ created successfully",
            //    Success = true,
            //    StatusCode = HttpStatusCode.OK
            //};
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
                    Reference = Utils.GenerateCoQRefrenceCode(),
                    DepotId = model.PlantId,
                    DateOfSTAfterDischarge = model.DateOfSTAfterDischarge,
                    DateOfVesselArrival = model.DateOfVesselArrival,
                    DateOfVesselUllage = model.DateOfVesselUllage,
                    DepotPrice = model.DepotPrice,
                    CreatedBy = user.Id,
                    Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing),
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    //NameConsignee = model.NameConsignee,
                    //SubmittedDate = DateTime.UtcNow.AddHours(1),
                };

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

            //return new ApiResponse
            //{
            //    Message = "COQ created successfully",
            //    Success = true,
            //    StatusCode = HttpStatusCode.OK
            //};
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
            var coq = await _context.CoQs
                .Include(c => c.Application!.Facility.VesselType)
                .Include(c => c.Application!.Payments)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (coq is null)
            {
                return new() { StatusCode = HttpStatusCode.NotFound, Success = false };
            }
            var tanks = await _context.COQTanks
                .Include(c => c.TankMeasurement).Where(c => c.CoQId == coq.Id)
                .Select(c => new
                {
                    c.TankId,
                    c.Id,
                    c.TankMeasurement,
                    c.CoQId,
                    TankName = _context.Tanks.Where(x => x.Id == c.TankId).Select(x => x.Name).FirstOrDefault()
                })
                .ToListAsync();
            var docs = await _context.SubmittedDocuments.FirstOrDefaultAsync(c => c.ApplicationId == coq.Id);
            var dictionary = new Dictionary<string, object?>();
            var props = coq?.GetType()?.GetProperties();
            if (props?.Any() is true)
            {
                foreach (var property in props)
                {
                    dictionary.Add(property.Name, property.GetValue(coq));
                }
                dictionary.Add("Application.Vessel", coq.Application.Facility);
                dictionary.Remove("Application.Facility");
            }
            var app = coq.Application;
            if (dictionary.ContainsKey("Application"))
            {
                dictionary["Application"] = new
                {
                    app.Id,
                    app.Status,
                    app.Reference,
                    CreatedDate = app.CreatedDate.ToString("MMM dd, yyyy HH:mm:ss"),
                    SubmittedDate = app.SubmittedDate != null ? app.SubmittedDate.Value.ToString("MMM dd, yyyy HH:mm:ss") : null,
                    TotalAmount = string.Format("{0:N}", app.Payments.Sum(x => x.Amount)),
                    PaymentDescription = app.Payments.FirstOrDefault()?.Description,
                    PaymnetDate = app.Payments.FirstOrDefault()?.TransactionDate.ToString("MMM dd, yyyy HH:mm:ss"),
                    CurrentDesk = _userManager.Users.FirstOrDefault(x => x.Id.Equals(app.CurrentDeskId))?.Email,
                    app.MarketerName,
                    app.MotherVessel,
                    app.Jetty,
                    app.LoadingPort,
                    NominatedSurveyor = (await _unitOfWork.NominatedSurveyor.Find(c => c.Id == app.SurveyorId)).FirstOrDefault(),
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
                    }
                };

            }
            return new()
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new
                {
                    coq = dictionary,
                    tanks,
                    docs
                }
            };
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

        public async Task<ApiResponse> GetAllCoQCertificates()
        {
            try
            {
                var certList = await _unitOfWork.COQCertificate.GetAll();

                _apiReponse = new ApiResponse { Success = true, StatusCode = HttpStatusCode.OK, Data = certList };
            }
            catch (Exception e)
            {

                _apiReponse = new ApiResponse { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = e.Message };
                
            }

            return _apiReponse;
        }

       // public async Task<>

        private List<PlantFieldOfficer> GetDepotsListforUSer(string Id)
        {
            var plist =  _context.PlantFieldOfficers.Where(x => x.OfficerID.ToString() == Id).ToList();
            return plist;
        }

        private CoQDTO GetCoqApproved(int Id)
        {
            var plist = _context.CoQs.FirstOrDefault(x => x.DepotId== Id && x.Status == "Approved");
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

    }
}
