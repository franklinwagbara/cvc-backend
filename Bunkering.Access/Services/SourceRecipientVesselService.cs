using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
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
    public class SourceRecipientVesselService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;

        private string User;
        public SourceRecipientVesselService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            //_response = response;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _userManager = userManager;
        }


        public async Task<ApiResponse> GetAllSourceRecipientMapping()
        {
            var mappings = (await _unitOfWork.SourceRecipientVessel.GetAll("SourceVessel,DestinationVessel")).ToList();
            //var staffs = await _userManager.Users.Where(x => x.IsDeleted != true).ToListAsync();
            var filteredMappings = mappings.Where(x => x.IsDeleted == false).Select(d => new SourceDestinationVesselViewModel
            {
                SourceVesselId = d.SourceVesselId,
                DestinationVesselId = d.DestinationVesselId,
                SourceVesselName = d.SourceVessel?.Name,
                DestinationVesselName = d.DestinationVessel?.Name,
            }).ToList();

            return new ApiResponse
            {
                Message = "All Mappings found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredMappings
            };
        }

        public async Task<ApiResponse> GetSourceRecipientVesselByID(int id)
        {
            SourceRecipientVessel? mapping = await _unitOfWork.SourceRecipientVessel.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResponse
            {
                Message = "All Mapping found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = mapping
            };
        }

        public async Task<ApiResponse> CreateSourceRecipientVesselMapping(SourceDestinationVesselViewModel model)
        {
            try
            {
                var SourceVesselExists = await _unitOfWork.Facility.FirstOrDefaultAsync(c => c.Id == model.SourceVesselId) is not null;
                var DestinationVesselExists = await _unitOfWork.Facility.FirstOrDefaultAsync(c => c.Id == model.DestinationVesselId) is not null;


                if (!SourceVesselExists)
                {
                    _response = new ApiResponse
                    {
                        Message = "Source Vessel not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };

                }

                else if (!DestinationVesselExists)
                {
                    _response = new ApiResponse
                    {
                        Message = "Destination Vessel not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };

                }

                else
                {
                    var existingMap = await _unitOfWork.SourceRecipientVessel.FirstOrDefaultAsync(a => a.SourceVesselId == model.SourceVesselId && a.DestinationVesselId == model.DestinationVesselId);
                    if (existingMap == null)

                    {
                        var map = new SourceRecipientVessel
                        {
                            SourceVesselId = model.SourceVesselId,
                            DestinationVesselId = model.DestinationVesselId

                        };
                        await _unitOfWork.SourceRecipientVessel.Add(map);
                        await _unitOfWork.SaveChangesAsync("");

                        _response = new ApiResponse
                        {
                            Message = "Source Recipient mapping was added successfully.",
                            StatusCode = HttpStatusCode.OK,
                            Success = true,
                        };

                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Message = "Recipient is Already Assigned To This Source Vessel",
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

        public async Task<ApiResponse> EditSourceDestinationMapping(int id, SourceDestinationVesselViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatMapping = await _unitOfWork.SourceRecipientVessel.FirstOrDefaultAsync(x => x.Id == id);
                try
                {
                    if (updatMapping != null)
                    {
                        updatMapping.SourceVesselId = model.SourceVesselId;
                        updatMapping.DestinationVesselId = model.DestinationVesselId;
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
                var deactiveMapping = await _unitOfWork.SourceRecipientVessel.FirstOrDefaultAsync(a => a.Id == id);
                if (deactiveMapping != null)
                {
                    if (!deactiveMapping.IsDeleted)
                    {
                        deactiveMapping.IsDeleted = true;
                        await _unitOfWork.SourceRecipientVessel.Update(deactiveMapping);
                        await _unitOfWork.SaveChangesAsync(user.Id);

                        _response = new ApiResponse
                        {
                            Data = deactiveMapping,
                            Message = "Mapping has been deleted",
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
