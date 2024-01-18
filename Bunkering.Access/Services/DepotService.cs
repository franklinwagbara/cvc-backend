using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Bunkering.Access.Services
{
    public class DepotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public DepotService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreateDepot(DepotViewModel model)
        {
            var addDepot = await _unitOfWork.Depot.FirstOrDefaultAsync(d => d.Name.ToLower() == model.Name.ToLower());

            if (addDepot != null)
            {
                _response = new ApiResponse
                {
                    Message = "Depot Already Exist",
                    StatusCode = HttpStatusCode.Found,
                    Success = true
                };

                return _response;
            }

            var depot = new Depot
            {
                Name = model.Name,
                StateId = model.StateId,
                Capacity = model.Capacity,
            };

            await _unitOfWork.Depot.Add(depot);
            await _unitOfWork.SaveChangesAsync("");
            model.Id = depot.Id;

            _response = new ApiResponse
            {
                Data = model,
                Message = "Depot Created",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };


            return _response;

        }
        public async Task<ApiResponse> EditDepot(DepotViewModel model)
        {
            var depot = await _unitOfWork.Depot.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
            if (depot != null)
            {
                depot.Name = model.Name;
                depot.StateId = model.StateId;
                depot.Capacity = model.Capacity;
                await _unitOfWork.Depot.Update(depot);
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
                    Message = "Depot not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }

            return _response;
        }
        public async Task<ApiResponse> DeleteDepot(int Id)
        {
            var depot = await _unitOfWork.Depot.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (depot != null)
            {
                if (!depot.IsDeleted)
                {
                    depot.IsDeleted = true;
                    depot.DeletedAt = DateTime.Now;
                    depot.DeletedBy = _contextAccessor.HttpContext.User.Identity?.Name ?? string.Empty;
                    await _unitOfWork.Depot.Update(depot);
                    _unitOfWork.Save();

                    _response = new ApiResponse
                    {
                        Message = "Depot has been deleted",
                        Data = depot,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };

                }
                else
                {
                    _response = new ApiResponse
                    {

                        Message = "Depot has already been deleted",
                        Data = depot,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };

                }

            }
            else
            {
                _response = new ApiResponse
                {

                    Message = "Depot doesnt exist",
                    Data = depot,
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };

            }
            return _response;
        }
        public async Task<ApiResponse> GetAllDepot()
        {
            var allDepot = await _unitOfWork.Depot.GetAll("State");
            allDepot = allDepot.Where(x => x.DeletedAt == null);

            _response = new ApiResponse
            {
                Message = "Successful",
                Data = allDepot,
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }

        public async Task<ApiResponse> AllDepotByAppId(int AppId)
        {
            try
            {
                var user = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
                var coq = await _unitOfWork.CoQ.Query(c =>  c.AppId == AppId).Select(c => c.Id).ToListAsync();
                var depotOffices = (await _unitOfWork.PlantOfficer.Find(c => c.OfficerID.ToString() == user && c.IsDeleted != true)).ToList();

                if(!depotOffices.Any() )
                {
                    return new ApiResponse
                    {
                        Message = $"No depot assign to you at the moment.",
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var appDepots = (await _unitOfWork.ApplicationDepot.Find(x => x.AppId == AppId, "Depot")).ToList() ?? throw new Exception("No Depot was applied for this NOA application.");
                var depots = appDepots.Where(x => depotOffices
                .Exists(a => a.PlantID == x.DepotId)).Select(x => x.Depot)
                .ToList() ?? throw new Exception("Could find these depot(s)");

                depots = depots.SkipWhile(d => coq.Exists(c => c == d.Id)).ToList();
                return new ApiResponse
                {
                    Data = depots,
                    Message = "Successful.",
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = $"{e.Message} +++ {e.StackTrace} ~~~ {e.InnerException?.ToString()}",
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
