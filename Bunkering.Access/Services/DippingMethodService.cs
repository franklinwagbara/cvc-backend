using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class DippingMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private string User;
        public DippingMethodService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task<ApiResponse> GetDippingMethodByID(int id)
        {
            DippingMethod? mapping = await _unitOfWork.DippingMethod.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResponse
            {
                Message = "All Methods found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = mapping
            };
        }

        public async Task<ApiResponse> CreateDippingMethod(DippingMethod model)
        {
            try
            {
                var map = new DippingMethod
                {

                    Name = model.Name,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,

                };
                await _unitOfWork.DippingMethod.Add(map);
                await _unitOfWork.SaveChangesAsync("");

                _response = new ApiResponse
                {
                    Message = "Dipping Methhod was added successfully.",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
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

        public async Task<ApiResponse> EditDippingMethod(int id, DippingMethod model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatDipping = await _unitOfWork.DippingMethod.FirstOrDefaultAsync(x => x.Id == id);
                try
                {
                    if (updatDipping != null)
                    {
                        updatDipping.ModifiedDate = DateTime.Now;
                        updatDipping.Name = model.Name;
                        var success = await _unitOfWork.SaveChangesAsync(user!.Id) > 0;
                        _response = new ApiResponse
                        {
                            Message = success ? "Dipping Method was Edited successfully." : "Unable to edit Dipping Method, please try again.",
                            StatusCode = success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                            Success = success
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
            }
            catch (Exception)
            {
                _response = new ApiResponse
                {
                    Message = "You need to LogIn to Edit a Dipping Method",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }
            return _response;
        }

        public async Task<ApiResponse> DeleteDippingMethod(int id)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                if (user is null)
                {
                    _response = new ApiResponse
                    {
                        Message = "You need to LogIn to Delete a MeterType",
                        StatusCode = HttpStatusCode.Forbidden,
                        Success = true
                    };
                }
                var deactiveMethod = await _unitOfWork.DippingMethod.FirstOrDefaultAsync(a => a.Id == id);
                if (deactiveMethod != null)
                {
                    if (!deactiveMethod.IsDeleted)
                    {
                        deactiveMethod.IsDeleted = true;
                        await _unitOfWork.DippingMethod.Update(deactiveMethod);
                        await _unitOfWork.SaveChangesAsync(user.Id);

                        _response = new ApiResponse
                        {
                            Data = deactiveMethod,
                            Message = "Dipping Method has been deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Data = deactiveMethod,
                            Message = "Mapping is already deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }

                }

            }
            catch (Exception)
            {
                _response = new ApiResponse
                {
                    Message = "You need to LogIn to Delete a Mapping",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }

            return _response;
        }

        public async Task<ApiResponse> GetAllDippingMethods()
        {
            var types = await _unitOfWork.DippingMethod.GetAll();
            var filteredmethods = types.Where(x => x.IsDeleted == false);
            return new ApiResponse
            {
                Message = "All Dipping Methods found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredmethods
            };
        }
    }
}
