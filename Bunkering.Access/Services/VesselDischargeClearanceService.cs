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
                var create = await _unitOfWork.VesselDischargeClearance
                    .FirstOrDefaultAsync(x => x.AppId == model.AppId);

                if (create != null) {

                    _response = new ApiResponse
                    {
                        Message = "Vessel Discharge Clearance Already Exist",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                    return _response;
                }

                var app = await _unitOfWork.Application
                    .FirstOrDefaultAsync(x => x.Id == model.AppId);

                if (app == null)
                {
                    _response = new ApiResponse
                    {
                        Message = "NOA dosen't exist",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                    return _response;
                }

               
                var appDepot = await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.AppId == model.AppId && x.DepotId == x.DepotId);

                if (appDepot == null)
                {
                    _response = new ApiResponse
                    {
                        Message = "This Depot dosen't exist on this NOA ",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                    return _response;
                }



                if (create == null)
                {
                    var vesselDischargeClearance = new VesselDischargeClearance
                    {
                        AppId = model.AppId,
                        DischargeId = appDepot.DischargeId,
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
                        DepotId = model.DepotId,
                        IsAllowed   = model.IsAllowed,
                        FinalBoilingPoint = model.FinalBoilingPoint,
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
