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

       
    }
}
