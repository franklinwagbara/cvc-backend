using Azure;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class VesselDischargeClearanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public VesselDischargeClearanceService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApiResponse> CreateVesselDischargeClearance(VesselDischargeCleareanceViewModel model)
        {
            try
            {
                var create = await _unitOfWork.VesselDischargeClearance.FirstOrDefaultAsync(x => x.VesselName == model.VesselName);
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == model.AppId);
                if (create == null)
                {
                    var vesselDischargeClearance = new VesselDischargeClearance
                    {
                        AppId = model.AppId,
                        DischargeId = model.DischargeId,
                        VesselName = model.VesselName,
                        VesselPort = model.VesselPort,
                        Product = model.Product,
                        Density = model.Density,
                        RON = model.RON,
                        FlashPoint = model.FlashPoint,
                        Color = model.Color,
                        Odour = model.Odour,
                        Oxygenate = model.Oxygenate,
                        Others = model.Others,
                        Comment = model.Comment,
                    };
                    
                    await _unitOfWork.VesselDischargeClearance.Add(vesselDischargeClearance);
                    await _unitOfWork.SaveChangesAsync("");
                    app.HasCleared = true;
                    await _unitOfWork.Application.Update(app);
                    _unitOfWork.Save();
                    
                    model.Id = vesselDischargeClearance.Id;

                    _response = new ApiResponse
                    {
                        Data = model,
                        Message = "Vessel Discharge Clearance Created",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };

                }
                else
                {
                    _response = new ApiResponse
                    {
                        Message = "Vessel Discharge Clearance Already Exist",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                    return _response;
                }

            }
            catch(Exception ex)
            {
                _response = new ApiResponse { Message = ex.Message };
            }
            return _response;
        }

        public async Task<ApiResponse> GetAllVesselDischargeClearance()
        {
            var aLLVesselDischargeClearance = await _unitOfWork.VesselDischargeClearance.GetAll();

            _response = new ApiResponse
            {
                Data = aLLVesselDischargeClearance,
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };

            return _response;
        }
    }
}
