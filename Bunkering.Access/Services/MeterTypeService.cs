using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class MeterTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private string User;
        public MeterTypeService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task<ApiResponse> GetMeterTypeByID(int id)
        {
            MeterType? mapping = await _unitOfWork.MeterType.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResponse
            {
                Message = "All Mapping found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = mapping
            };
        }

        public async Task<ApiResponse> CreateMeterType(MeterType model)
        {
            try
            {
                var map = new MeterType
                {
                            
                    Name = model.Name,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,

                };
                await _unitOfWork.MeterType.Add(map);
                await _unitOfWork.SaveChangesAsync("");

                _response = new ApiResponse
                {
                    Message = "Jetty Officer mapping was added successfully.",
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

        public async Task<ApiResponse> EditMeterType(int id, MeterType model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatMeter = await _unitOfWork.MeterType.FirstOrDefaultAsync(x => x.Id == id);
                try
                {
                    if (updatMeter != null)
                    {
                        updatMeter.ModifiedDate = DateTime.Now;
                        updatMeter.Name = model.Name;
                        var success = await _unitOfWork.SaveChangesAsync(user!.Id) > 0;
                        _response = new ApiResponse
                        {
                            Message = success ? "MeterType was Edited successfully." : "Unable to edit MeterType, please try again.",
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
                    Message = "You need to LogIn to Edit a MeterType",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }
            return _response;
        }

        public async Task<ApiResponse> DeleteMeterType(int id)
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
                var deactiveMeterType = await _unitOfWork.MeterType.FirstOrDefaultAsync(a => a.Id == id);
                if (deactiveMeterType != null)
                {
                    if (!deactiveMeterType.IsDeleted)
                    {
                        deactiveMeterType.IsDeleted = true;
                        await _unitOfWork.MeterType.Update(deactiveMeterType);
                        await _unitOfWork.SaveChangesAsync(user.Id);

                        _response = new ApiResponse
                        {
                            Data = deactiveMeterType,
                            Message = "MeterType has been deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Data = deactiveMeterType,
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

        public async Task<ApiResponse> GetAllMeterTypes()
        {
            var types = await _unitOfWork.MeterType.GetAll();
            var filteredtypes = types.Where(x => x.IsDeleted == false);
            return new ApiResponse
            {
                Message = "All MeterTypes found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredtypes
            };
        }
    }
}
