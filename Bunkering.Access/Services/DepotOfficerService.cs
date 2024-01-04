﻿using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Bunkering.Access.Services
{
    public class DepotOfficerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private string User;
        public DepotOfficerService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            //_response = response;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _userManager = userManager;
        }

        public async Task<ApiResponse> GetAllDepotOfficerMapping()
        {
            var mappings = await _unitOfWork.DepotOfficer.GetAll("Depot");
            var staffs = await _userManager.Users.Where(x => x.UserRoles.Any(u => u.Role.Name == RoleConstants.COMPANY) != true).ToListAsync();
            var filteredMappings = mappings.Where(x => x.IsDeleted == false).Select(d => new DepotFieldOfficerViewModel
            {
                DepotID = d.ID,
                UserID = d.OfficerID,
                DepotName = d.Depot.Name,
                OfficerName = staffs.Where(x => x.Id == d.OfficerID.ToString()).Select(u => u?.FirstName + ", " + u?.LastName ).FirstOrDefault()
            }).ToList(); 

            return new ApiResponse
            {
                Message = "All Mappings found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredMappings
            };
        }

        public async Task<ApiResponse> GetDepotOfficerByID(int id)
        {
            DepotFieldOfficer? mapping = await _unitOfWork.DepotOfficer.FirstOrDefaultAsync(x => x.ID == id);
            return new ApiResponse
            {
                Message = "All Mapping found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = mapping
            };
        }

        public async Task<ApiResponse> CreateDepotOfficerMapping(DepotFieldOfficerViewModel newDepotOfficer)
        {
            try
            {
                var depotExists = await _unitOfWork.Depot.FirstOrDefaultAsync(c => c.Id ==  newDepotOfficer.DepotID) is not null;
                if (!depotExists)
                {
                    _response = new ApiResponse
                    {
                        Message = "Depot not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                    return _response;
                }
                var userExists = await _userManager.Users.AnyAsync(c => c.Id == newDepotOfficer.UserID.ToString());
                if (!userExists)
                {
                    _response = new ApiResponse
                    {
                        Message = "Officer not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                    return _response;
                }
                var map = new DepotFieldOfficer
                {
                    DepotID = newDepotOfficer.DepotID,
                    OfficerID = newDepotOfficer.UserID

                };
                await _unitOfWork.DepotOfficer.Add(map);
                await _unitOfWork.SaveChangesAsync("");
                _response = new ApiResponse
                {
                    Message = "DepotOfficer mapping was added successfully.",
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

        public async Task<ApiResponse> EditDepotOfficerMapping(int id, DepotFieldOfficerViewModel depot)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatMapping = await _unitOfWork.DepotOfficer.FirstOrDefaultAsync(x => x.ID == id);
                try
                {
                    if (updatMapping != null)
                    {
                        updatMapping.DepotID = depot.DepotID;
                        updatMapping.OfficerID = depot.UserID;
                        var success = await _unitOfWork.SaveChangesAsync(user!.Id) > 0;
                        _response = new ApiResponse
                        {
                            Message = success ? "Mapping was Edited successfully." : "Unable to edit Mapping, please try again.",
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
                    Message = "You need to LogIn to Edit a Mapping",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }
            return _response;
        }

        public async Task<ApiResponse> DeleteMapping(int id)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                if (user is null)
                {
                    _response = new ApiResponse
                    {
                        Message = "You need to LogIn to Delete a Mapping",
                        StatusCode = HttpStatusCode.Forbidden,
                        Success = true
                    };
                }
                var deactiveMapping = await _unitOfWork.DepotOfficer.FirstOrDefaultAsync(a => a.ID == id);
                if (deactiveMapping != null)
                {
                    if (!deactiveMapping.IsDeleted)
                    {
                        deactiveMapping.IsDeleted = true;
                        await _unitOfWork.DepotOfficer.Update(deactiveMapping);
                        await _unitOfWork.SaveChangesAsync(user.Id);

                        _response = new ApiResponse
                        {
                            Data = deactiveMapping,
                            Message = "Fee has been deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Data = deactiveMapping,
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
    }
}
