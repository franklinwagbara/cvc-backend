using Bunkering.Access.DAL;
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
    public class VesselService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public VesselService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        /*
         public async Task<ApiResponse> EditIMONoByName(string name, string newIMO)
        {
            var vessel = await _unitOfWork.Facility.FirstOrDefaultAsync(x => x.Name.Equals(name));
            if (vessel == null)
            {
                _response = new ApiResponse
                {
                    Message = "No Vessel was found with this Name",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            else
            {
                var check = await _unitOfWork.Facility.FirstOrDefaultAsync(_ => _.IMONumber.Equals(newIMO));
                if (check == null)
                {
                    
                    vessel.IMONumber = newIMO;
                    await _unitOfWork.Facility.Update(vessel);
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
                        Message = "There is an Existing Vessel with the suggested IMO number",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
            }

            return _response;

        }*/
    }
}
