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
    public class RoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreateRole(RoleViewModel model)
        {
            var addRole = await _unitOfWork.Role.FirstOrDefaultAsync(d => d.Name.ToLower() == model.Name.ToLower());
            if (addRole != null)
            {
                _response = new ApiResponse
                {
                    Message = "Role Already Exist",
                    StatusCode = HttpStatusCode.Found,
                    Success = true
                };

                return _response;

            }
            var role = new Role
            {
                Name = model.Name,
                Description = model.Description,
            };

            await _unitOfWork.Role.Add(role);
            await _unitOfWork.SaveChangesAsync("");
            model.Id = role.Id;

            _response = new ApiResponse
            {
                Data = model,
                Message = "Role Created",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };
            return _response;
        }
        public async Task<ApiResponse> EditRole(RoleViewModel model)
        {
            var edit = await _unitOfWork.Role.FirstOrDefaultAsync(r => r.Id == model.Id);
            if(edit != null)
            {
                model.Name = edit.Name;
                model.Description = edit.Description;
                await _unitOfWork.Role.Update(edit);
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
                    Message = "Role not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }
            return _response;
        }
        public async Task<ApiResponse> AllRoles (RoleViewModel model)
        {
            var allRoles = await _unitOfWork.Role.GetAll();

            _response = new ApiResponse
            {
                Message = "Successful",
                Data = allRoles,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };
            return _response;
        }
       
       
    }
}
