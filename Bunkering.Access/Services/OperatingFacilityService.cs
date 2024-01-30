using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class OperatingFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public OperatingFacilityService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, ApiResponse response)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
        }

        public async Task<ApiResponse> CreateOperatingFacility(OpearatingFacilityViewModel model)
        {

            try
            {
                var opFacilityCheck = await _unitOfWork.OperatingFacility.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (opFacilityCheck != null)
                {
                    _response = new ApiResponse
                    {
                        Data = model,
                        Message = "Operating Plant Already Exists",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false,
                    };

                    return _response;

                }


                var opFacility = new OperatingFacility
                {
                    CompanyId = model.CompanyId,
                    Name = Enum.GetName(typeof(NameType), 0),
                };


                await _unitOfWork.OperatingFacility.Add(opFacility);
                await _unitOfWork.SaveChangesAsync("");
                model.Id = opFacility.Id;

                _response = new ApiResponse
                {
                    Data = model,
                    Message = "Operating Plant Created",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                _response = new ApiResponse { Message = ex.Message };
            }
            return _response;

        }
        public async Task<ApiResponse> EditOperatingFacility(OpearatingFacilityViewModel model)
        {

            var editOpFacility = await _unitOfWork.OperatingFacility.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (editOpFacility != null)
            {
                editOpFacility.Name = Enum.GetName(typeof(NameType), 0);

                await _unitOfWork.OperatingFacility.Update(editOpFacility);
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
                    Message = "Operating Facility Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }
            return _response;
        }
        public async Task<ApiResponse> AllOperatingFacilities()
        {
            var allOpFacility = await _unitOfWork.OperatingFacility.GetAll();

            _response = new ApiResponse
            {
                Data = allOpFacility,
                Message = "Success",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };
            return _response;
        }
    }
}
