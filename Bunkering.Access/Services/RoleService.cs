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
        private readonly RoleManager<ApplicationRole> _role;

        public RoleService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, RoleManager<ApplicationRole> role, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
            _userManager = userManager;
            _role = role;
        }

        public async Task<ApiResponse> CreateRole(RoleViewModel model)
        {
            var foundRole = await _role.Roles.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(model.Name.ToLower())); 
            if (foundRole != null)
            {
                _response = new ApiResponse
                {
                    Message = "Role Already Exist",
                    StatusCode = HttpStatusCode.Found,
                    Success = true
                };

                return _response;

            }

            var newRole = new ApplicationRole
            {
                Name = model.Name,
                Description = model.Description,
            };

            var result = await _role.CreateAsync(newRole);
            await _unitOfWork.SaveChangesAsync("");

            _response = new ApiResponse
            {
                Data = newRole,
                Message = "Role Created",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };
            return _response;
        }
        public async Task<ApiResponse> EditRole(RoleViewModel model)
        {
            ApplicationRole? foundRole = await _role.Roles.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(model.Name.ToLower()));

            if (foundRole != null)
            {
                foundRole.Description = model.Description;
                foundRole.Name = model.Name;

                await _role.UpdateAsync(foundRole);
                await _unitOfWork.SaveChangesAsync("");

                _response = new ApiResponse
                {
                    Data = foundRole,
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
        public async Task<ApiResponse> AllRoles ()
        {
            var allRoles = await _role.Roles.ToListAsync();

            _response = new ApiResponse
            {
                Message = "Successful",
                Data = allRoles,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };
            return _response;
        }


        public async Task<ApiResponse> DeleteRole(string Id)
        {
            ApplicationRole? foundRole = await _role.Roles.FirstOrDefaultAsync(x => x.Id.ToLower().Equals(Id));

            if (foundRole != null)
            {
                await _role.DeleteAsync(foundRole);
                return new ApiResponse
                {
                    Message = "Deletion successfull.",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            else
                return new ApiResponse
                {
                    Message = "Role does not exist!",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
        }
    }
}
