using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Bunkering.Access.DAL;

namespace Bunkering.Access.Services
{
    public class AppStageDocService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private string User;
        ApiResponse _response;
        private readonly IElps _elps;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppStageDocService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IElps elps,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _elps = elps;
            _userManager = userManager;
        }

        public async Task<ApiResponse> GetAllElpsDocs()
        {
            var documents = _elps.GetDocumentTypes();
            documents.AddRange(_elps.GetDocumentTypes("facility"));

            if (documents.Any())
            {
                _response = new ApiResponse
                {
                    Message = "Elps Dccumnets successfully found",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = documents
                };
            }
            else
                _response = new ApiResponse
                {
                    Message = "No Dccumnet was found",
                    StatusCode = HttpStatusCode.OK,
                    Success = false
                };

            return _response;
        }
        public async Task<ApiResponse> GetAllFADDoc()
        {
            var docs = await _unitOfWork.FacilityTypeDocuments.GetAll("VesselType,ApplicationType");
            if (docs != null)
                _response = new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = docs.Select(d => new
                    {
                        d.Id,
                        d.Name,
                        d.IsFADDoc,
                        AppType = d.ApplicationType.Name,
                        d.DocType,
                        VesselType = d.VesselType.Name,
                        DocId = d.DocumentTypeId
                    })
                };
            else
                _response = new ApiResponse
                {
                    Message = "No documents found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return _response;
        }

        public async Task<ApiResponse> CreateFADDoc(AppStageDocsViewModel model)
        {
            try
            {
                var documents = _elps.GetDocumentTypes();
                documents.AddRange(_elps.GetDocumentTypes("facility"));

                var docs = documents.Where(x => model.DocumentTypeId.Any(y => y.Equals(x.Id))).Select(d => new FacilityTypeDocument
                {
                    ApplicationTypeId = model.ApplicationTypeId,
                    VesselTypeId = model.VesselTypeId,
                    DocumentTypeId = d.Id,
                    Name = d.Name,
                    DocType = d.Type,
                });

                if (docs.Any())
                {
                    var user = await _userManager.FindByEmailAsync(User);

                    docs = await _unitOfWork.FacilityTypeDocuments.AddRange(docs);
                    await _unitOfWork.SaveChangesAsync(user.Id);
                    //_unitOfWork.Save();

                    _response = new ApiResponse
                    {
                        Message = "Facility Type Dccuments added successfully",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                    _response = new ApiResponse
                    {
                        Message = "No Facility Type Dccuments to add",
                        StatusCode = HttpStatusCode.OK,
                        Success = false
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
            return _response;
        }

        public async Task<ApiResponse> UpdateFADDoc(int id)
        {
            try
            {
                var doc = await _unitOfWork.FacilityTypeDocuments.FirstOrDefaultAsync(x => x.DocumentTypeId.Equals(id));
                if (doc != null)
                {
                    doc.IsFADDoc = true;
                    await _unitOfWork.FacilityTypeDocuments.Update(doc);
                    _unitOfWork.Save();

                    _response = new ApiResponse
                    {
                        Message = "Update was successful",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                    _response = new ApiResponse
                    {
                        Message = "Document not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
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
            return _response;
        }

        public async Task<ApiResponse> DeleteFADDoc(int id)
        {
            try
            {
                var save = 0;
                var faddoc = await _unitOfWork.FacilityTypeDocuments.FirstOrDefaultAsync(d => d.Id.Equals(id));
                if (faddoc != null)
                {
                    await _unitOfWork.FacilityTypeDocuments.Remove(faddoc);
                    save = _unitOfWork.Save();

                    if (save > 0)
                    {
                        return new ApiResponse
                        {
                            Message = "Deleted Successful",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                }
                else
                {
                    _response = new ApiResponse
                    {
                        Data = faddoc,
                        Message = "Document Not Found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                }

            }

            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = ex.Data,
                    Success = false
                };
            }

            return _response;

            /* Message = "Error in operation",
			StatusCode = HttpStatusCode.BadRequest,
			Success = false
			*/




        }
    }
}
