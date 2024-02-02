using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Exceptions;
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
    public class MeterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public MeterService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            
        }

        public async Task<ApiResponse> AddMeter(MeterViewModel model)
        {
            var addMeter = await _unitOfWork.Meter.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (addMeter != null)
            {
                _response = new ApiResponse
                {
                    Message = "Meter already exist",
                    StatusCode = HttpStatusCode.Found,
                    Success = true
                };
            }

            var meter = new Meter
            {
                Name = model.Name,
                PlantId = model.PlantId,
            };

            await _unitOfWork.Meter.Add(meter);
            await _unitOfWork.SaveChangesAsync("");

            model.Id = meter.Id;
            model.PlantId = meter.PlantId;

            _response = new ApiResponse
            {
                Data = model,
                Message = "Meter Created",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> UpdateMeter(MeterViewModel model)
        {
            var updateMeter = await _unitOfWork.Meter.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
            if (updateMeter != null)
            {
                updateMeter.Name = model.Name;
                updateMeter.PlantId = model.PlantId;
               
                await _unitOfWork.Meter.Update(updateMeter);
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
                    Message = "Meter not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }

            return _response;
        }

        public async Task<ApiResponse> DeleteMeter(int Id)
        {
            var delMeter = await _unitOfWork.Meter.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (delMeter != null)
            {
                _response = new ApiResponse
                {
                    Message = "Meter has already been deleted",
                    StatusCode = HttpStatusCode.Conflict,
                    Success = false,
                };

            }

            delMeter.DeletedAt = DateTime.Now;
            await _unitOfWork.Meter.Update(delMeter);
            _unitOfWork.Save();


            _response = new ApiResponse
            {
                Message = "Meter has been deleted",
                Data = delMeter,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> AllMeters()
        {
            var allMeters = await _unitOfWork.Meter.GetAll();
            allMeters = allMeters.Where(x => x.DeletedAt == null);

            _response = new ApiResponse
            {
                Data = allMeters.ToList(),
                Message ="Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> MeterById(int id)
        {
            var meterById = await _unitOfWork.Meter.FirstOrDefaultAsync(x =>x.Id.Equals(id));
            if (meterById != null)
            {
                return new ApiResponse
                {
                    Data = meterById,
                    Message = "Successful",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };

            }

            return new ApiResponse
            {
                Message = "Meter Not Found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false,
            };
        }
    }
}
