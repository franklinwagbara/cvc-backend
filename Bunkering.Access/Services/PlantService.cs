using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Bunkering.Access.Services
{
    public class PlantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private ApiResponse _response = new ApiResponse();
        private readonly UserManager<ApplicationUser> _userManager;
        private string User;
        private readonly HttpClient _httpClient;

        public PlantService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            //_response = response;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _userManager = userManager;
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse> GetAllPlants()
        {
            var plants = await _unitOfWork.Plant.GetAll();
            var filteredPlants = plants.Where(x => x.IsDeleted == false);
            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredPlants
            };
        }

        public async Task<ApiResponse> EditPlant(PlantDTO plant)
        {
            var user = await _userManager.FindByEmailAsync(User);
            var updatePlant = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id == plant.Id);
            if (updatePlant == null)
            {
                _response = new ApiResponse
                {
                    Message = "Plant not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
                return _response;
            }
            else
            {
                updatePlant.Name = plant.Name;
                updatePlant.Company = plant.Company;
                //updatePlant.PlantType = plant.PlantType;
                updatePlant.ElpsPlantId = plant.FacilityElpsId;
                updatePlant.CompanyElpsId = plant.CompanyElpsId;
                updatePlant.Email = plant.Email;
                updatePlant.State = plant.State;
                updatePlant.IsDeleted = false;

                var success = await _unitOfWork.SaveChangesAsync(user!.Id) > 0;
                _response = new ApiResponse
                {
                    Message = success ? "Plant was Edited successfully." : "Unable to edit Plant, please try again.",
                    StatusCode = success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                    Success = success
                };

            }
            return _response;
        }

        public async Task<ApiResponse> EditPlantTanks(PlantTankDTO tank)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatePlant = await _unitOfWork.PlantTank.FirstOrDefaultAsync(x => x.Id == tank.Id);
                if (updatePlant == null)
                {
                    _response = new ApiResponse
                    {
                        Message = "Tank not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                    return _response;
                }
                else
                {
                    updatePlant.Product = tank.Product;
                    updatePlant.TankName = tank.TankName;
                    updatePlant.Capacity = tank.Capacity;
                    updatePlant.IsDeleted = false;

                    var success = await _unitOfWork.SaveChangesAsync(user!.Id) > 0;
                    _response = new ApiResponse
                    {
                        Message = success ? "The Tank was Edited successfully." : "Unable to edit The Tank, please try again.",
                        StatusCode = success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                        Success = success
                    };

                }                

            }
            catch (Exception)
            {

                _response = new ApiResponse
                {
                    Message = "You need to LogIn to Edit a Plant",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }
            return _response;

        }

        public async Task<ApiResponse> CreatePlant(PlantDTO plant)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);

                var facility = new Plant
                {
                    Name = plant.Name,
                    //PlantType = plant.PlantType,
                    State = plant.State,
                    Company = plant.Company,
                    Email = plant.Email  
                };
                await _unitOfWork.Plant.Add(facility);
                await _unitOfWork.SaveChangesAsync(user.Id);

                int plantId = facility.Id;

                var TankList = new List<PlantTank>();
                foreach (var item in plant.Tanks)
                {
                    var plantTanks = new PlantTank
                    {
                        PlantId = plantId,
                        Capacity = item.Capacity,
                        Position = item.Position,
                        Product = item.Product,
                        TankName = item.TankName
                    };
                    TankList.Add(plantTanks);
                }
                await _unitOfWork.PlantTank.AddRange(TankList);
                await _unitOfWork.SaveChangesAsync(user.Id);

                _response = new ApiResponse
                {
                    Message = "Plant was added successfully.",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _response = new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }

            return _response;
        }

        public async Task<ApiResponse> GetPlantById(int id)
        {
            Plant? mapping = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResponse
            {
                Message = "All Mapping found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = mapping
            };
        }

        public async Task<ApiResponse> DeletePlant(int id)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                if (user is null)
                {
                    _response = new ApiResponse
                    {
                        Message = "You need to LogIn to Delete a Plant",
                        StatusCode = HttpStatusCode.Forbidden,
                        Success = true
                    };
                }
                var deactivePlant = await _unitOfWork.Plant.FirstOrDefaultAsync(a => a.Id == id);
                if (deactivePlant != null)
                {
                    if (!deactivePlant.IsDeleted)
                    {
                        deactivePlant.IsDeleted = true;
                        await _unitOfWork.Plant.Update(deactivePlant);
                        await _unitOfWork.SaveChangesAsync(user.Id);

                        _response = new ApiResponse
                        {
                            Data = deactivePlant,
                            Message = "Plant has been deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Data = deactivePlant,
                            Message = "Mapping is already deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }

                }

            }
            catch (Exception)
            {
                _response = new ApiResponse
                {
                    Message = "You need to LogIn to Delete a Plant",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }

            return _response;
        }

        public async Task<ApiResponse> DeletePlantTank(int id)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                if (user is null)
                {
                    _response = new ApiResponse
                    {
                        Message = "You need to LogIn to Delete a Tank",
                        StatusCode = HttpStatusCode.Forbidden,
                        Success = true
                    };
                }
                var deactivePlantTank = await _unitOfWork.PlantTank.FirstOrDefaultAsync(a => a.Id == id);
                if (deactivePlantTank != null)
                {
                    if (!deactivePlantTank.IsDeleted)
                    {
                        deactivePlantTank.IsDeleted = true;
                        await _unitOfWork.PlantTank.Update(deactivePlantTank);
                        await _unitOfWork.SaveChangesAsync(user.Id);

                        _response = new ApiResponse
                        {
                            Data = deactivePlantTank,
                            Message = "Tank has been deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }
                    else
                    {
                        _response = new ApiResponse
                        {
                            Data = deactivePlantTank,
                            Message = "Mapping is already deleted",
                            StatusCode = HttpStatusCode.OK,
                            Success = true
                        };
                    }

                }

            }
            catch (Exception)
            {
                _response = new ApiResponse
                {
                    Message = "You need to LogIn to Delete a Tank",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Success = true
                };
            }

            return _response;
        }

        public async Task<ApiResponse> GetDepotsList()
        {
            var apiUrl = "https://depotonline.nmdpra.gov.ng/Application/DepotFacilityReports"
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                Plant depotList = JsonConvert.DeserializeObject<Plant>(jsonResponse);

                _response = new ApiResponse
                {
                    Message = "You need to LogIn to Delete a Tank",
                    StatusCode = HttpStatusCode.Forbidden,
                    Success = true,
                    Data = depotList

                };

            }
            catch (Exception e)
            {
                _response = new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false                  

                };
            }
            return _response;

        }
    }
}
