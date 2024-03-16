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
    public class JettyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public JettyService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreateJetty(JettyViewModel model)
        {
            try
            {
                var checkjetty = await _unitOfWork.Jetty.FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());
                if (checkjetty != null)
                {
                    _response = new ApiResponse
                    {
                        Message = "Jetty Already Exist",
                        StatusCode = HttpStatusCode.Found,
                        Success = true
                    };

                    return _response;

                }

                var jetty = new Jetty
                {
                    Name = model.Name,
                    StateId = model.StateId,
                    Location = model.Location
                };

                await _unitOfWork.Jetty.Add(jetty);
                await _unitOfWork.SaveChangesAsync("");
                model.Id = jetty.Id;

                _response = new ApiResponse
                {
                    Data = model,
                    Message = "Jetty Created",
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
        public async Task<ApiResponse> EditJetty(JettyViewModel model)
        {
            var editJetty = await _unitOfWork.Jetty.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (editJetty != null)
            {
                editJetty.Name = model.Name;
                editJetty.StateId = model.StateId;
                editJetty.Location = model.Location;

                await _unitOfWork.Jetty.Update(editJetty);
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
                    Message = "Jetty not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }
            return _response;
        }
        public async Task<ApiResponse> DeleteJetty(int id)
        {
            var jetty = await _unitOfWork.Jetty.FirstOrDefaultAsync(j => j.Id.Equals(id));
            if (jetty != null)
            {
                if (!jetty.IsDeleted)
                {
                    jetty.IsDeleted = true;
                    jetty.DeletedAt = DateTime.Now;
                    jetty.DeletedBy = _contextAccessor.HttpContext.User.Identity?.Name ?? string.Empty;
                    await _unitOfWork.Jetty.Update(jetty);
                    _unitOfWork.Save();

                    _response = new ApiResponse
                    {

                        Message = "Jetty has been deleted",
                        Data = jetty,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
                else
                {
                    _response = new ApiResponse
                    {

                        Message = "Jetty Already deleted",
                        Data = jetty,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
            }
            else
            {
                _response = new ApiResponse
                {

                    Message = "Jetty Doesnt Exist",
                    Data = jetty,
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            return _response;
        }
        public async Task<ApiResponse> AllJetty()
        {
            var allJetty = await _unitOfWork.Jetty.GetAll("State");
            var list = allJetty.Where(x => x.IsDeleted == false).GroupBy(s => s.State).Select(x => new
            {
                GroupName = x.Key.Name,
                Jetties = x.Select(y => new { y.Id, Name = $"{y.Name} ({y.Location})"})
            }).ToList();

            _response = new ApiResponse
            {
                Message = "Successful",
                Data = list,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

    }
}
