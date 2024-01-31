﻿using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class ShipToShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private string User;

        public ShipToShipService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task<ApiResponse> AddRecord(TransferRecordDTO transferRecord)
        {
            try
            {
                if (transferRecord == null)
                {
                    var tr = new TransferRecord();
                    tr.IMONumber = transferRecord.IMONumber;
                    tr.LoadingPort = transferRecord.LoadingPort;
                    tr.TransferDate = transferRecord.TransferDate;
                    tr.MotherVessel = transferRecord.MotherVessel;
                    tr.VessellID = transferRecord.VessellID;
                    tr.TransferDetails = new List<TransferDetail>();
                    foreach (var item in transferRecord.Details)
                    {
                        tr.TransferDetails.Add(new TransferDetail()
                        {
                            CreatedDate = DateTime.UtcNow,
                            IMONumber = item.IMONumber,
                            LoadingPort = item.LoadingPort,
                            ProductId = item.ProductId,
                            TransferDetailId = item.TransferDetailId,
                            VessellID = item.VessellID,
                            VolumeToTransfer = item.VolumeToTransfer
                           
                        });

                    }

                    await _unitOfWork.TransferRecord.Add(tr);
                    await _unitOfWork.SaveChangesAsync(User);


                    _response = new ApiResponse
                    {
                        Message = "Successfully Added TransferRecord",
                        StatusCode = HttpStatusCode.OK,
                        Success = true

                    };
                }
               
            }
            catch (Exception)
            {

                throw;
            }
           

            return _response;
        }

        public async Task<ApiResponse> GetAllTransferRecords()
        {
            var records = await _unitOfWork.TransferRecord.GetAll();

            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = records
            };
        }

        //public async Task<ApiResponse> GetTransferRecordsByVesselId(int vesselId)
        //{
        //    // Retrieve transfer records associated with the given VesselId
        //    var transferRecords = _unitOfWork.TransferDetail.Query()
        //        .Where(td => td.OfftakeVesselId == vesselId)
        //        .Select(td => td.TransferRecord)
        //        .ToList();

        //    return new ApiResponse
        //    {
        //        Message = "All Fees found",
        //        StatusCode = HttpStatusCode.OK,
        //        Success = true,
        //        Data = transferRecords
        //    };
        //}

       
    }
}
