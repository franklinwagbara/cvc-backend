using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class ShipToShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IElps _elps;
        private string User;
        private readonly WorkFlowService _flow;
        private readonly AppSetting _setting;

        public ShipToShipService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager, IElps elps, WorkFlowService flow, IOptions<AppSetting> setting)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _elps = elps;
            _flow = flow;
            _setting = setting.Value;
        }

        public async Task<ApiResponse> AddRecord(DestinationVesselDTO transferRecord)
        {

            try
            {                
                if (transferRecord is not null)
                {
                    var tr = new TransferRecord();
                    tr.IMONumber = transferRecord.IMONumber;
                    tr.LoadingPort = transferRecord.LoadingPort;
                    tr.TransferDate = transferRecord.TransferDate;
                    tr.MotherVessel = transferRecord.MotherVessel;
                    tr.VesselName = transferRecord.VesselName;
                    tr.TotalVolume = transferRecord.TotalVolume;
                    tr.VesselTypeId = transferRecord.VesselTypeId;
                    tr.UserId = User;
                    tr.TransferDetails = new List<TransferDetail>();
                    foreach (var item in transferRecord.DestinationVessels)
                    {
                        tr.TransferDetails.Add(new TransferDetail()
                        {
                            CreatedDate = DateTime.UtcNow,
                            IMONumber = item.IMONumber,
                            ProductId = item.ProductId,
                            VesselName = item.VesselName,
                            OfftakeVolume = item.OfftakeVolume
                           
                        });

                    }

                    await _unitOfWork.TransferRecord.Add(tr);
                    await _unitOfWork.SaveChangesAsync(User);


                    _response = new ApiResponse
                    {
                        Message = "Successfully Added TransferRecord",
                        StatusCode = HttpStatusCode.OK,
                        Success = true

                    };
                }
               
            }
            catch (Exception)
            {

                throw;
            }
           

            return _response;
        }

        public async Task<ApiResponse> GetAllTransferRecordsByCompany()
        {
            var user = await _userManager.FindByEmailAsync(User);
           
            var records =  GetAllTransferByEmail(User);

            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = records
            };
        }

        public async Task<ApiResponse> GetAllTransferRecords()
        {
            var records = GetAllTransfer();

            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = records
            };
        }

        private List<TransferRecord> GetAllTransferByEmail(string email)
        {

            var plist = _unitOfWork.TransferRecord.Query().Where(x => x.UserId == email)
                        .Select(x => new TransferRecord
                        {
                            Id = x.Id,
                            LoadingPort = x.LoadingPort,
                            IMONumber = x.IMONumber,
                            MotherVessel = x.MotherVessel,
                            TotalVolume = x.TotalVolume,
                            TransferDate = x.TransferDate,
                            VesselName = x.VesselName,
                            VesselTypeId = x.VesselTypeId,
                            TransferDetails = x.TransferDetails.ToList()
                        })
                        .ToList();
            return plist;
        }

        private List<TransferRecord> GetAllTransfer()
        {

            var plist = _unitOfWork.TransferRecord.Query()
                        .Select(x => new TransferRecord
                        {
                            Id = x.Id,
                            LoadingPort = x.LoadingPort,
                            IMONumber = x.IMONumber,
                            MotherVessel = x.MotherVessel,
                            TotalVolume = x.TotalVolume,
                            TransferDate = x.TransferDate,
                            VesselName = x.VesselName,
                            VesselTypeId = x.VesselTypeId,
                            TransferDetails = x.TransferDetails.ToList()
                        })
                        .ToList();
            return plist;
        }

        //public async Task<ApiResponse> GetTransferRecordsByVesselId(int vesselId)
        //{
        //    // Retrieve transfer records associated with the given VesselId
        //    var transferRecords = _unitOfWork.TransferDetail.Query()
        //        .Where(td => td.OfftakeVesselId == vesselId)
        //        .Select(td => td.TransferRecord)
        //        .ToList();

        //    return new ApiResponse
        //    {
        //        Message = "All Fees found",
        //        StatusCode = HttpStatusCode.OK,
        //        Success = true,
        //        Data = transferRecords
        //    };
        //}

        //public async Task<ApiResponse> AddDocuments(int id)
        //{
        //    if (id > 0)
        //    {
        //        var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "Facility.VesselType");
        //        var user = await _userManager.FindByEmailAsync(User);
        //        if (app != null)
        //        {
        //            var facTypeDocs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.ApplicationTypeId.Equals(app.ApplicationTypeId) && x.VesselTypeId.Equals(app.Facility.VesselTypeId));

        //            if (facTypeDocs.Any())
        //            {
        //                var compdocs = (List<Document>)_elps.GetCompanyDocuments(user.ElpsId);
        //                var facdocs = (List<FacilityDocument>)_elps.GetCompanyDocuments(app.Facility.ElpsId, "facility");
        //                var docs = new List<SubmittedDocument>();

        //                foreach (var item in facTypeDocs.ToList())
        //                {
        //                    if (item.DocType.ToLower().Equals("company"))
        //                    {
        //                        var doc = compdocs.FirstOrDefault(x => int.Parse(x.document_type_id) == item.DocumentTypeId);
        //                        if (doc != null)
        //                            docs.Add(new SubmittedDocument
        //                            {
        //                                ApplicationId = app.Id,
        //                                DocId = item.DocumentTypeId,
        //                                DocName = item.Name,
        //                                DocSource = doc.source,
        //                                DocType = item.DocType,
        //                                FileId = doc.id,
        //                                ApplicationTypeId = app.ApplicationTypeId,
        //                            });
        //                    }
        //                    else
        //                    {
        //                        var doc = facdocs.FirstOrDefault(x => x.Document_Type_Id == item.DocumentTypeId);
        //                        if (doc != null)
        //                            docs.Add(new SubmittedDocument
        //                            {
        //                                ApplicationId = app.Id,
        //                                DocId = item.DocumentTypeId,
        //                                DocName = item.Name,
        //                                DocSource = doc.Source,
        //                                DocType = item.DocType,
        //                                FileId = doc.Id,
        //                                ApplicationTypeId = app.ApplicationTypeId,
        //                            });
        //                    }
        //                }

        //                if (docs.Count > 0)
        //                {
        //                    var appdocs = (await _unitOfWork.SubmittedDocument.Find(x => x.ApplicationId.Equals(app.Id))).ToList();
        //                    if (appdocs.Count() > 0)
        //                        await _unitOfWork.SubmittedDocument.RemoveRange(appdocs);

        //                    await _unitOfWork.SubmittedDocument.AddRange(docs);
        //                    await _unitOfWork.SaveChangesAsync(user.Id);
        //                }
        //            }

        //            var submit = app.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentRejected)) || app.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Rejected))
        //                ? await _flow.AppWorkFlow(id, Enum.GetName(typeof(AppActions), AppActions.Resubmit), "Application re-submitted")
        //                : await _flow.AppWorkFlow(id, Enum.GetName(typeof(AppActions), AppActions.Submit), "Application Submitted");
        //            if (submit.Item1)
        //            {
        //                return new ApiResponse
        //                {
        //                    Message = submit.Item2,
        //                    StatusCode = HttpStatusCode.OK,
        //                    Success = true
        //                };
        //            }
        //            else
        //            {
        //                return new ApiResponse
        //                {
        //                    Message = submit.Item2,
        //                    StatusCode = HttpStatusCode.NotAcceptable,
        //                    Success = false
        //                };
        //            }
        //        }
        //        else
        //            return new ApiResponse
        //            {
        //                Message = "ApplicationID invalid",
        //                StatusCode = HttpStatusCode.BadRequest,
        //                Success = false
        //            };
        //    }
        //    else
        //       return new ApiResponse
        //        {
        //            Message = "Application not found",
        //            StatusCode = HttpStatusCode.NotFound,
        //            Success = false
        //        };

        //}

        public async Task<ApiResponse> GetSTSDocuments(int id)
        {

            if (id > 0)
            {
                var docList = new List<SubmittedDocument>();
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == id, "User,Facility.VesselType,ApplicationType");
                if (app != null)
                {
                    var appType = await _unitOfWork.ApplicationType.FirstOrDefaultAsync(c => c.Name == Utils.STS);
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
                    var factypedocs = await _unitOfWork.FacilityTypeDocuments.Find(x => x.ApplicationTypeId.Equals(appType.Id));
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
                                CompanyElpsId = app.User.ElpsId,
                                FacilityElpsId = app.Facility.ElpsId,
                                ApiEmail = _setting.AppEmail,
                                ApiHash = $"{_setting.AppEmail}{_setting.AppId}".GenerateSha512()
                            }
                        }
                    };
                }
                else
                    return new ApiResponse
                    {
                        Message = "ApplicationID invalid",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
            }
            else
                return new ApiResponse
                {
                    Message = "ApplicationID invalid",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

        }
    }
}
