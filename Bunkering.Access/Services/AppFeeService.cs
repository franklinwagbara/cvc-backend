using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Bunkering.Access.Services
{
    public class AppFeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        public AppFeeService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            //_response = response;
            _userManager = userManager;
        }

        public async Task<ApiResponse> GetAllFees()
        {
            var fees = await _unitOfWork.AppFee.GetAll();
            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = fees.Select(x => new
                {
                    x.Id,
                    x.SerciveCharge,
                    x.ApplicationTypeId,
                    x.NOAFee,
                    x.COQFee,
                    x.ProcessingFee
                })
            };
        }

        public async Task<ApiResponse> GetFeeByID(int id)
        {
            var fee = await _unitOfWork.AppFee.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = fee
            };
        }

        public async Task<ApiResponse> CreateFee(AppFee newFee)
        {
            try
            {
                var fee = new AppFee
                {
                    ProcessingFee = newFee.ProcessingFee,
                    ApplicationFee = newFee.ApplicationFee,
                    SerciveCharge = newFee.SerciveCharge,
                    NOAFee = newFee.NOAFee,
                    COQFee = newFee.COQFee,
                    ApplicationTypeId = newFee.ApplicationTypeId,
                    
                };
                await _unitOfWork.AppFee.Add(fee);
                await _unitOfWork.SaveChangesAsync("");
                _response = new ApiResponse
                {
                    Message = "Fee was added successfully.",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
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

        public async Task<ApiResponse> EditFee(AppFee newFee)
        {
            var updateFee = await _unitOfWork.AppFee.FirstOrDefaultAsync(x => x.Id == newFee.Id);
            try
            {
                if (updateFee != null)
                {
                    var Fee = new AppFee
                    {
                        SerciveCharge = newFee.SerciveCharge,
                        NOAFee = newFee.NOAFee,
                        COQFee = newFee.COQFee,
                        ApplicationFee = newFee.ApplicationFee,
                        ProcessingFee = newFee.ProcessingFee
                    };
                    await _unitOfWork.AppFee.Add(Fee);
                    await _unitOfWork.SaveChangesAsync("");
                    _response = new ApiResponse
                    {
                        Message = "Fee was Edited successfully.",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
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

        public async Task<ApiResponse> DeleteFee(int id)
        {
            var deactiveFee = await _unitOfWork.AppFee.FirstOrDefaultAsync(a => a.Id.Equals(id));
            if (deactiveFee != null)
            {
                if (!deactiveFee.IsDeleted)
                {
                    deactiveFee.IsDeleted = true;
                    await _unitOfWork.AppFee.Update(deactiveFee);

                    _response = new ApiResponse
                    {
                        Data = deactiveFee,
                        Message = "Fee has been deleted",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                _response = new ApiResponse
                {
                    Data = deactiveFee,
                    Message = "Fee is already deleted",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
            }

            return _response;
        }
    }
}
