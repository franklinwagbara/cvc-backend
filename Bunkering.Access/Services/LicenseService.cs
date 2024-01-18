using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class LicenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public LicenseService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApiResponse> LicenseReport(LicenseReportViewModel model)
        {
            var permitReport = await _unitOfWork.vFacilityPermit.Find(a => a.IssuedDate >= model.Min && a.IssuedDate <= model.Max);
            var permits = permitReport.OrderBy(a => new
            {
                a.Id,
                a.ApplicationId,
                a.VesselName,
                a.VesselType,
                a.PermitNo,
                a.IssuedDate,
                a.ExpireDate,
            }).ToList();

            if (permitReport != null)
            {
                _response = new ApiResponse
                {
                    Data = permits.ToList(),
                    Message = "Permit Report Found",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,

                };
                return _response;
            }
            else
                _response = new ApiResponse
                {
                    Message = "Permit Report Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            return _response;
        }
    }
}
