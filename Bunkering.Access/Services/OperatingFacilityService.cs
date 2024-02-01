using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Exceptions;
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

        public OperatingFacilityService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;

        }

        public async Task<ApiResponse> CreateOperatingFacility(OpearatingFacilityViewModel model)
        {

            try
            {
                var opFacilityCheck = await _unitOfWork.OperatingFacility.FirstOrDefaultAsync(x => x.CompanyEmail == model.CompanyEmail);
                
                if (opFacilityCheck != null)
                {
                    opFacilityCheck.Name = model.Name;
                    await _unitOfWork.OperatingFacility.Update(opFacilityCheck);
                    await _unitOfWork.SaveChangesAsync("");

                    return new ApiResponse
                    {
                        Data = opFacilityCheck,
                        Message = "Operating Facility was Created",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }

                var opFacility = new OperatingFacility
                {
                    CompanyEmail = model.CompanyEmail,
                    Name = Enum.GetName(typeof(NameType), model.Name),
                };


                await _unitOfWork.OperatingFacility.Add(opFacility);
                await _unitOfWork.SaveChangesAsync("");
     

                return new ApiResponse
                {
                    Data = model,
                    Message = "Operating Plant Created",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                if(ex is ConflictException)
                    return new ApiResponse { Message = ex.Message, StatusCode = HttpStatusCode.Conflict };
                else return new ApiResponse { Message = ex.Message };
            }
        }
        public async Task<ApiResponse> EditOperatingFacility(OpearatingFacilityViewModel model)
        {
            try
            {
                var editOpFacility = await _unitOfWork.OperatingFacility.FirstOrDefaultAsync(x => x.CompanyEmail == model.CompanyEmail);
                if (editOpFacility == null)
                    throw new NotFoundException("Operating facility not found");
                
                
                    editOpFacility.CompanyEmail = model.CompanyEmail;
                    editOpFacility.Name = Enum.GetName(typeof(NameType), model.Name);

                    await _unitOfWork.OperatingFacility.Update(editOpFacility);
                    _unitOfWork.Save();

                    return new ApiResponse
                    {
                        Message = "Updated Successfully",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };

                
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                    return new ApiResponse { Message = ex.Message, StatusCode = HttpStatusCode.NotFound };
                else return new ApiResponse { Message = ex.Message };
            }




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

        public async Task<ApiResponse> GetOperationFacility(string Email)
        {
            try
            {
                var res = await _unitOfWork.OperatingFacility.FirstOrDefaultAsync(x => x.CompanyEmail== Email);
                return new ApiResponse
                {
                    Data = res,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                };
            }
        }
    }
}
