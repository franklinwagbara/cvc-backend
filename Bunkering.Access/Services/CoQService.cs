using AutoMapper;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bunkering.Access.Services
{
    public class CoQService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpCxtAccessor;
        private ApiResponse _apiReponse;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private string LoginUserEmail = string.Empty;

        public CoQService(IUnitOfWork unitOfWork, IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpCxtAccessor = httpCxtAccessor;
            _apiReponse = new ApiResponse();
            _userManager = userManager;
            _mapper = mapper;
            LoginUserEmail = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task<ApiResponse> CreateCoQ(CoQViewModel Model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LoginUserEmail);

                if (user == null)
                    throw new Exception("Can not find user with Email: " + LoginUserEmail);

                if (user.UserRoles.FirstOrDefault().Role.Name != RoleConstants.Field_Officer)
                    throw new Exception("Only Field Officers can create CoQ.");

                var coq = _mapper.Map<CoQ>(Model);
                var result_coq = await _unitOfWork.CoQ.Add(coq);
                await _unitOfWork.SaveChangesAsync(user.Id);

                return new ApiResponse
                {
                    Data = result_coq,
                    Message = "Successfull",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                return _apiReponse = new ApiResponse
                {
                    Data = null,
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}\n",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
