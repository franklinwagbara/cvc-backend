using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class NominatedSurveyorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public NominatedSurveyorService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreateSurveyor(NominatedSurveyorViewModel model)
        {
            try
            {
                var addSurveyor = await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(n => n.Name == model.Name);
                if (addSurveyor == null)
                {
                    var surveyor = new NominatedSurveyor
                    {
                        Name = model.Name,
                    };

                    await _unitOfWork.NominatedSurveyor.Add(surveyor);
                    await _unitOfWork.SaveChangesAsync("");
                    model.Id = surveyor.Id;

                    _response = new ApiResponse
                    {
                        Data = model,
                        Message = "Nominated Surveyor Created",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
                else if (addSurveyor != null)
                {
                    _response = new ApiResponse
                    {
                        Message = "Nominated Surveyor Already Exist",
                        StatusCode = HttpStatusCode.Found,
                        Success = true
                    };

                }
                else
                {
                    _response = new ApiResponse
                    {
                        Message = "Unable to create Nominated Surveyor",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {
                _response = new ApiResponse { Message = ex.Message };
            }

            return _response;
        }
        public async Task<ApiResponse> EditSurveyor(NominatedSurveyorViewModel model)
        {
            var editSurveyor = await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (editSurveyor != null)
            {
                editSurveyor.Name = model.Name;

                await _unitOfWork.NominatedSurveyor.Update(editSurveyor);
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
                    Message = "Nominated Surveyor not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };

            }
            return _response;
        }
        public async Task<ApiResponse> DeleteSurveyor(int id)
        {
            var surveyor = await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (surveyor != null)
            {
                if (!surveyor.IsDeleted)
                {
                    surveyor.IsDeleted = true;
                    surveyor.DeletedAt = DateTime.Now;
                    surveyor.DeletedBy = _contextAccessor.HttpContext.User.Identity?.Name ?? string.Empty;
                    await _unitOfWork.NominatedSurveyor.Update(surveyor);
                    _unitOfWork.Save();

                    _response = new ApiResponse
                    {
                        Message = "Nominated Surveyor has been deleted",
                        Data = surveyor,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
                else
                {
                    _response = new ApiResponse
                    {
                        Message = "Nominated Surveyor already deleted",
                        Data = surveyor,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "Nominated Surveyor does not Exist",
                    Data = surveyor,
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };

            }
            return _response;
        }
        public async Task<ApiResponse> AllNominatedSurveyor()
        {
            var allSurveyor = await _unitOfWork.NominatedSurveyor.GetAll();
            allSurveyor = allSurveyor.Where(x => x.IsDeleted != true);

            _response = new ApiResponse
            {
                Message = "Successful",
                Data = allSurveyor,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }
    }


}
