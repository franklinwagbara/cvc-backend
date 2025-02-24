﻿using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;
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
            var mappings = await _unitOfWork.vPlantFieldOfficer.Find(x => x.IsDeleted == false);
            //var staffs = await _userManager.Users.Where(x => x.UserRoles.Any(u => u.Role.Name == RoleConstants.COMPANY) != true).ToListAsync();
            var filteredMappings = mappings.Select(d => new DepotFieldOfficerViewModel
            {
                PlantFieldOfficerID = d.ID,
                DepotID = d.PlantID,
                UserID = d.OfficerID,
                DepotName = d.DepotName,
                OfficerName = $"{d.FirstName} {d.LastName}"
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
            PlantFieldOfficer? mapping = await _unitOfWork.PlantOfficer.FirstOrDefaultAsync(x => x.ID == id);
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
                var user = await _userManager.FindByEmailAsync(User);
                var userExists = await _userManager.Users.AnyAsync(c => c.Id == newDepotOfficer.UserID);
                var depotExists = await _unitOfWork.Plant.FirstOrDefaultAsync(c => c.Id ==  newDepotOfficer.DepotID) is not null;


                if (!depotExists)
                    _response = new ApiResponse
                    {
                        Message = "Facility not found",
                        StatusCode = HttpStatusCode.NotFound,
                    };
                else if(!userExists)
                    _response = new ApiResponse
                    {
                        Message = "Officer not found",
                        StatusCode = HttpStatusCode.NotFound,
                    };
                else
                {

                    var depotOfficerExist = await _unitOfWork.PlantOfficer.FirstOrDefaultAsync(j => j.PlantID.Equals(newDepotOfficer.DepotID) && j.OfficerID.Equals(newDepotOfficer.UserID));

                    if (depotOfficerExist != null && depotOfficerExist.IsDeleted)
                    {
                        depotOfficerExist.IsDeleted = false;

                        await _unitOfWork.PlantOfficer.Update(depotOfficerExist);
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }
                    else if(depotOfficerExist is null)
                    {
                        var map = new PlantFieldOfficer
                        {
                            PlantID = newDepotOfficer.DepotID,
                            OfficerID = newDepotOfficer.UserID
                        };
                        await _unitOfWork.PlantOfficer.Add(map);
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }
                    _response = new ApiResponse
                    {
                        Message = "Depot Officer mapping was added successfully.",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
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

        public async Task<ApiResponse> EditDepotOfficerMapping(int id, DepotFieldOfficerViewModel depot)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatMapping = await _unitOfWork.PlantOfficer.FirstOrDefaultAsync(x => x.ID == depot.PlantFieldOfficerID);
                try
                {
                    if (updatMapping != null)
                    {
                        updatMapping.PlantID = depot.DepotID;
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
                var deactiveMapping = await _unitOfWork.PlantOfficer.FirstOrDefaultAsync(a => a.ID == id);
                if (deactiveMapping != null)
                {
                    if (!deactiveMapping.IsDeleted)
                    {
                        deactiveMapping.IsDeleted = true;
                        await _unitOfWork.PlantOfficer.Update(deactiveMapping);
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
            catch (Exception ex)
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
