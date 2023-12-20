using AutoMapper;
using Azure;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;

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


        public CoQService(IUnitOfWork unitOfWork, IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, IOptions<AppSetting> setting, IMapper mapper, IElps elps, WorkFlowService flow)
        {
            _unitOfWork = unitOfWork;
            _httpCxtAccessor = httpCxtAccessor;
            _userManager = userManager;
            _mapper = mapper;
            LoginUserEmail = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _elps = elps;
            _setting = setting.Value;
            _flow = flow;
        }

        public async Task<ApiResponse> CreateCoQ(CoQViewModel Model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LoginUserEmail);

                if (user == null)
                    throw new Exception("Can not find user with Email: " + LoginUserEmail);

                if (user.UserRoles.FirstOrDefault().Role.Name != RoleConstants.Field_Officer)
                    throw new Exception("Only Field Officers can create CoQ.");

                var coq = _mapper.Map<CoQ>(Model);
                var result_coq = await _unitOfWork.CoQ.Add(coq);
                await _unitOfWork.SaveChangesAsync(user.Id);

                return new ApiResponse
                {
                    Data = result_coq,
                    Message = "Successfull",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                return _apiReponse = new ApiResponse
                {
                    Data = null,
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}\n",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse> DocumentUpload(int id)
        {

            if (id > 0)
            {
                var docList = new List<SubmittedDocument>();
                var app = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id == id, "User,Facility.VesselType,ApplicationType");
                if (app != null)
                {
                    var factypedocs = await _unitOfWork.FacilityTypeDocuments
                        .Find(x => x.ApplicationTypeId
                        .Equals(app.Application.ApplicationTypeId) && x.VesselTypeId.Equals(app.Application.Facility.VesselTypeId));
                    if (factypedocs != null && factypedocs.Count() > 0)
                    {
                        var compdocs = _elps.GetCompanyDocuments(app.Application.User.ElpsId, "company")
                            .Stringify().Parse<List<Document>>();
                        var facdocs = _elps.GetCompanyDocuments(app.Application.Facility.ElpsId, "facility")
                            .Stringify().Parse<List<FacilityDocument>>();
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
                                            ApplicationId = id
                                        });
                                    }
                                    else
                                    {
                                        docList.Add(new SubmittedDocument
                                        {
                                            DocId = x.DocumentTypeId,
                                            DocName = x.Name,
                                            DocType = x.DocType,
                                        });
                                    }
                                }
                                else
                                    docList.Add(new SubmittedDocument
                                    {
                                        DocId = x.DocumentTypeId,
                                        DocName = x.Name,
                                        DocType = x.DocType,
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
                                            ApplicationId = id
                                        });
                                    }
                                    else
                                    {
                                        docList.Add(new SubmittedDocument
                                        {
                                            DocId = x.DocumentTypeId,
                                            DocName = x.Name,
                                            DocType = x.DocType,
                                        });
                                    }
                                }
                                else
                                    docList.Add(new SubmittedDocument
                                    {
                                        DocId = x.DocumentTypeId,
                                        DocName = x.Name,
                                        DocType = x.DocType,
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
    }
}
