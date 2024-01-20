using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class JettyOfficerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private string User;
        public JettyOfficerService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task<ApiResponse> GetAllJettyOfficerMapping()
        {
            var mappings = await _unitOfWork.JettyOfficer.GetAll("Jetty");
            var staffs = await _userManager.Users.Where(x => x.UserRoles.Any(u => u.Role.Name == RoleConstants.COMPANY) != true).ToListAsync();
            var filteredMappings = mappings.Where(x => x.IsDeleted == false).Select(d => new JettyFieldOfficerViewModel
            {
                JettyID = d.ID,
                UserID = d.OfficerID,
                JettyName = d.Jetty.Name,
                OfficerName = staffs.Where(x => x.Id == d.OfficerID.ToString()).Select(u => u?.FirstName + ", " + u?.LastName).FirstOrDefault()
            }).ToList();

            return new ApiResponse
            {
                Message = "All Mappings found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredMappings
            };
        }

        public async Task<ApiResponse> GetJettyOfficerByID(int id)
        {
            JettyFieldOfficer? mapping = await _unitOfWork.JettyOfficer.FirstOrDefaultAsync(x => x.ID == id);
            return new ApiResponse
            {
                Message = "All Mapping found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = mapping
            };
        }

        public async Task<ApiResponse> CreateJettyOfficerMapping(JettyFieldOfficerViewModel newJettyOfficer)
        {
            try
            {
                var userExists = await _userManager.Users.AnyAsync(c => c.Id == newJettyOfficer.UserID.ToString());
                var jettyExists = await _unitOfWork.Plant.FirstOrDefaultAsync(c => c.Id == newJettyOfficer.JettyID) is not null;


                if (!jettyExists)
                {
                    _response = new ApiResponse
                    {
                        Message = "Jetty not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };

                }

                else if (!userExists)
                {
                    _response = new ApiResponse
                    {
                        Message = "Officer not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };

                }

                else
                {
                    var existingMap = await _unitOfWork.JettyOfficer.FirstOrDefaultAsync(a => a.OfficerID == newJettyOfficer.UserID && a.JettyId == newJettyOfficer.JettyID);
                    if (existingMap == null)

                    {
                        var map = new JettyFieldOfficer
                        {
                            JettyId = newJettyOfficer.JettyID,
                            OfficerID = newJettyOfficer.UserID

                        };
                        await _unitOfWork.JettyOfficer.Add(map);
                        await _unitOfWork.SaveChangesAsync("");

                        _response = new ApiResponse
                        {
                            Message = "Jetty Officer mapping was added successfully.",
                            StatusCode = HttpStatusCode.OK,
                            Success = true,
                        };

                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Message = "Officer is Already Assigned To This Jetty",
                            StatusCode = HttpStatusCode.Conflict,
                            Success = false,
                        };

                    }
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

        public async Task<ApiResponse> EditJettyOfficerMapping(int id, JettyFieldOfficerViewModel jetty)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatMapping = await _unitOfWork.JettyOfficer.FirstOrDefaultAsync(x => x.ID == id);
                try
                {
                    if (updatMapping != null)
                    {
                        updatMapping.JettyId = jetty.JettyID;
                        updatMapping.OfficerID = jetty.UserID;
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
                var deactiveMapping = await _unitOfWork.JettyOfficer.FirstOrDefaultAsync(a => a.ID == id);
                if (deactiveMapping != null)
                {
                    if (!deactiveMapping.IsDeleted)
                    {
                        deactiveMapping.IsDeleted = true;
                        await _unitOfWork.JettyOfficer.Update(deactiveMapping);
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
