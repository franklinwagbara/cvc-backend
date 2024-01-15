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
using System.Data;
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
                        ApplicationId = coq.Id,
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
                    ProductId = model.ProductId,
                    Reference = Utils.GenerateCoQRefrenceCode(),
                   // DepotId = model.PlantId,
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
                            Tempearture = b.Tempearture,
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
                            Tempearture = a.Tempearture,
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
                        ApplicationId = coq.Id,
                        Subject = $"COQ with reference {coq.Reference} Submitted",
                        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                        UserId = user.Id,
                        Date = DateTime.Now.AddHours(1),
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
            var coq = await _context.CoQs.FirstOrDefaultAsync(c => c.Id == id);
            var gastankList = new List<List<CreateCoQGasTankDTO>>();
            var liqtankList = new List<List<CreateCoQLiquidTankDto>>();
            var product = new Product();

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
                    TankName = _context.Tanks.FirstOrDefault(x => x.Id == c.TankId)
                })
                .ToListAsync();
            var docs = await _context.SubmittedDocuments.FirstOrDefaultAsync(c => c.ApplicationId == coq.Id);
            
            if(coq.ProductId != null)
            {
                product = await _unitOfWork.Product.FirstOrDefaultAsync(x =>x.Id.Equals(coq.ProductId));
                if(product != null)
                {
                    switch(product.ProductType.ToLower())
                    {
                        case "gas":
                            foreach(var item in tanks)
                            {
                                var reading = _mapper.Map<List<CreateCoQGasTankDTO>>(item.TankMeasurement);
                                reading.ForEach(x => x.TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == item.TankId).TankName);
                                gastankList.Add(reading);
                            }
                            break;
                        default:
                            foreach(var item in tanks)
                            {
                                var reading = _mapper.Map<List<CreateCoQLiquidTankDto>>(item.TankMeasurement);
                                reading.ForEach(x => x.TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == item.TankId).TankName);
                                liqtankList.Add(reading);
                            }
                            break;
                    }
                }
            }
            else
            {
                var app = await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.Id.Equals(coq.AppId), "Product");
                if (app != null)
                    product = app.Product;
            }
            var dictionary = coq.Stringify().Parse<Dictionary<string, object>>();
            var coqData = new CoQsDataDTO()
            {
                coq = new()
                {
                    Vessel = new()
                }
            };
            if(coq.AppId != null)
            {
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(coq.AppId), "Facility");
                if(app != null)
                {
                    
                    coqData.coq.MarketerName = app?.MarketerName ?? string.Empty;
                    coqData.coq.MotherVessel = app.MotherVessel;
                    coqData.coq.Jetty = app.Jetty;
                    coqData.coq.LoadingPort = app.LoadingPort;
                    coqData.coq.Vessel.Name = app.Facility.Name;
                    coqData.coq.Vessel.VesselType = app.Facility?.VesselType?.Name?? string.Empty;
                    coqData.coq.NominatedSurveyor = (await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(c => c.Id == app.SurveyorId)).Name;
                    //dictionary.Add("MarketerName", app.MarketerName);
                    //dictionary.Add("MotherVessel", app.MotherVessel);
                    //dictionary.Add("Jetty", app.Jetty);
                    //dictionary.Add("LoadingPort", app.LoadingPort);
                    //dictionary.Add("VesselName", app.Facility.Name);
                    //dictionary.Add("NominatedSurveyor", (await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(c => c.Id == app.SurveyorId)).Name);
                    
                }
            }
            coqData.coq.ProductType = product.ProductType;
            coqData.coq.CurrentDesk = _userManager.Users.FirstOrDefault(u => u.Id.Equals(coq.CurrentDeskId)).Email;
            coqData.coq.Plant = _context.Plants.FirstOrDefault(p => p.Id.Equals(coq.PlantId)).Name;
            //dictionary.Add("ProductType", product.ProductType);
            ////remove deskid and replace with name
            //dictionary.Remove("CurrentDeskId");
            //dictionary.Add("CurrentDesk", _userManager.Users.FirstOrDefault(u => u.Id.Equals(coq.CurrentDeskId)).Email);
            ////remove deskid and replace with name
            //dictionary.Remove("Plant");
            //dictionary.Add("Plant", _context.Plants.FirstOrDefault(p => p.Id.Equals(coq.PlantId)).Name);
            if (product.ProductType != null && product.ProductType.ToLower().Equals("gas"))
                 return new()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = new
                    {
                        coq = coqData,
                        tankList = gastankList,
                        docs
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
                var certList = await _unitOfWork.COQCertificate.GetAll();

                _apiReponse = new ApiResponse { Success = true, StatusCode = HttpStatusCode.OK, Data = certList };
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

        public async Task<ApiResponse> ViewCoQLiquidCertificate(int coqId)
        {
            try
            {
                //var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception("Unathorise, this action is restricted to only authorise users");

                var dataforView = GetLiquidCertificate(coqId);
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
            return cd;
        }

        private COQLiquidCertificateDTO GetLiquidCertificate(int coqId)
        {
            try
            {
                //var data = _context.COQCertificates.Include(c => c.COQ)
                //            .ThenInclude(ct => ct.Plant.Tanks);
                var cqs = _context.CoQs.Include(x => x.Plant).Include(n => n.Application).FirstOrDefault(x => x.Id == coqId);
                var tnks = _context.PlantTanks.Where(x => x.PlantId == cqs.Plant.Id).ToList();
                var coQTanks = _context.COQTanks.Include(t => t.TankMeasurement)
                    .Where(c => c.CoQId == coqId).ToList();

                var dat = new COQLiquidCertificateDTO
                {
                    CompanyName = cqs.Plant.Company,
                    DateOfVesselArrival = cqs.DateOfVesselArrival,
                    DateOfVessselUllage = cqs.DateOfVesselUllage,
                    Jetty = cqs.Application?.Jetty ?? string.Empty,
                    MotherVessel = cqs.Application?.MotherVessel ?? string.Empty,
                    Product = tnks.FirstOrDefault().Product,
                    ReceivingTerminal = cqs.Plant.Name,
                    VesselName = cqs.Application?.VesselName ?? string.Empty,
                };
                var tankList = new  List<CoQTanksDTO>();
                foreach (var item in coQTanks)
                {
                    //var  tanks = new List<CoQTanksDTO>

                    var tr = new CoQTanksDTO
                    {
                        AfterTankMeasurement = item.TankMeasurement.Select(
                                             tt => new CoQLiquidTankAfterReading
                                             {
                                                 TankId = item.TankId,
                                                 coQLiquidTank = new CoQLiquidTank
                                                 {
                                                     Density = tt.Density,
                                                     DIP = tt.DIP,
                                                     MeasurementType = tt.MeasurementType,
                                                     FloatRoofCorr = tt.FloatRoofCorr,
                                                     GOV = tt.GOV,
                                                     GSV = tt.GSV,
                                                     MTVAC = tt.MTVAC,
                                                     Tempearture = tt.Tempearture,
                                                     TOV = tt.TOV,
                                                     VCF = tt.VCF,
                                                     WaterDIP = tt.WaterDIP,
                                                     WaterVolume = tt.WaterVolume
                                                 }
                                             }
                                          ).Where(t => t.coQLiquidTank.MeasurementType == ReadingType.After).ToList(),
                        BeforeTankMeasurements = item.TankMeasurement.Select(
                                             tt => new CoQLiquidTankBeforeReading
                                             {
                                                 TankId = item.TankId,
                                                 coQLiquidTank = new CoQLiquidTank
                                                 {
                                                     Density = tt.Density,
                                                     DIP = tt.DIP,
                                                     MeasurementType = tt.MeasurementType,
                                                     FloatRoofCorr = tt.FloatRoofCorr,
                                                     GOV = tt.GOV,
                                                     GSV = tt.GSV,
                                                     MTVAC = tt.MTVAC,
                                                     Tempearture = tt.Tempearture,
                                                     TOV = tt.TOV,
                                                     VCF = tt.VCF,
                                                     WaterDIP = tt.WaterDIP,
                                                     WaterVolume = tt.WaterVolume
                                                 }
                                             }
                                          ).Where(t => t.coQLiquidTank.MeasurementType == ReadingType.Before).ToList(),
                        TankName = _context.PlantTanks.FirstOrDefault(t => t.PlantTankId == item.TankId).TankName
                    };
                    tankList.Add(tr);
                }
                   
                    

               dat.tanks = tankList;

                return dat;
            }
            catch (Exception e)
            {

                throw;
            }
           
        }

        private COQGasCertficateDTO GetCOQGasCertficate(int coqId)
        {
            try
            {
                var cqs = _context.CoQs.Include(x => x.Plant).Include(n => n.Application).FirstOrDefault(x => x.Id == coqId);
                var tnks = _context.PlantTanks.Where(x => x.PlantId == cqs.Plant.Id).ToList();
                var coQTanks = _context.COQTanks.Include(t => t.TankMeasurement)
                    .Where(c => c.CoQId == coqId).ToList();

                var dat = new COQGasCertficateDTO
                {
                    CompanyName = cqs.Plant.Company,
                    DateOfVesselArrival = cqs.DateOfVesselArrival,
                    ShoreDate = cqs.DateOfVesselUllage,
                    Jetty = cqs.Application?.Jetty ?? string.Empty,
                    Consignee = cqs.NameConsignee,
                    Product = tnks.FirstOrDefault().Product,
                    ReceivingTerminal = cqs.Plant.Name,
                    VesselName = cqs.Application?.VesselName ?? string.Empty,
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
