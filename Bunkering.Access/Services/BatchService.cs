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
    public class BatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public BatchService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;

        }

        public async Task<ApiResponse> AddBatch(BatchViewModel model)
        {
            var addBatch = await _unitOfWork.Batch.FirstOrDefaultAsync(x => x.BatchId == model.BatchId);
            if (addBatch != null)
            {
                _response = new ApiResponse
                {
                    Message = "Batch already exist",
                    StatusCode = HttpStatusCode.Found,
                    Success = true
                };
            }

            var batch = new Batch
            {
                Name = model.Name,
            };

            await _unitOfWork.Batch.Add(batch);
            await _unitOfWork.SaveChangesAsync("");

            model.BatchId = batch.BatchId;

            _response = new ApiResponse
            {
                Data = model,
                Message = "Batch Created",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> UpdateBatch(BatchViewModel model)
        {
            var updateBatch = await _unitOfWork.Batch.FirstOrDefaultAsync(x => x.BatchId.Equals(model.BatchId));
            if (updateBatch != null)
            {
                updateBatch.Name = model.Name;

                await _unitOfWork.Batch.Update(updateBatch);
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
                    Message = "Batch not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }

            return _response;
        }

        public async Task<ApiResponse> DeleteBatch(int Id)
        {
            var delBatch = await _unitOfWork.Batch.FirstOrDefaultAsync(x => x.BatchId.Equals(Id));
            if (delBatch != null)
            {
                _response = new ApiResponse
                {
                    Message = "Batch has already been deleted",
                    StatusCode = HttpStatusCode.Conflict,
                    Success = false,
                };

            }

            delBatch.DeletedAt = DateTime.Now;
            await _unitOfWork.Batch.Update(delBatch);
            _unitOfWork.Save();


            _response = new ApiResponse
            {
                Message = "Batch has been deleted",
                Data = delBatch,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> AllBatches()
        {
            var allBatch = await _unitOfWork.Batch.GetAll();
            allBatch = allBatch.Where(x => x.DeletedAt == null).ToList();

            _response = new ApiResponse
            {
                Data = allBatch,
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> BatchById(int id)
        {
            var batchById = await _unitOfWork.Batch.FirstOrDefaultAsync(x => x.BatchId.Equals(id));
            if (batchById != null)
            {
                return new ApiResponse
                {
                    Data = batchById,
                    Message = "Successful",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };

            }

            return new ApiResponse
            {
                Message = "Batch Not Found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false,
            };
        }
    }
}
