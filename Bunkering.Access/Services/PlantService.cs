using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Access.Query;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
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
        private readonly IElps _elps;
        private readonly PlantQueries _plantQueries;
        private readonly ApplicationContext context;

        public PlantService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, IElps elps)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            //_response = response;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _userManager = userManager;
            _httpClient = new HttpClient();
            _elps = elps;
            
           
        }

        public async Task<ApiResponse> GetAllPlants()
        {
            var plants = GetAllPlantswithTanks();
            var filteredPlants = plants.Where(x => x.IsDeleted == false );
            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredPlants
            };
        }

        public async Task<ApiResponse> GetAllPlantsByCompany()
        {
            var user = await _userManager.FindByEmailAsync(User);
            if (user == null)
            {
                _response = new ApiResponse
                {
                    Message = "You need to LogIn to get Plants",
                    StatusCode = HttpStatusCode.MethodNotAllowed,
                    Success = false
                };
                return _response;
            }
            var plants = GetPlantsByCompanywithTanks(user.Email);
            //var plants = await _unitOfWork.Plant.GetAll();
            var filteredPlants = plants.Where(x => x.IsDeleted == false && x.Email == user.Email);
            return new ApiResponse
            {
                Message = "All Plants for Company",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredPlants
            };
        }

        public async Task<ApiResponse> GetAllPlantTanksByPlantId(int id)
        {
            var tanks = await _unitOfWork.PlantTank.Find(x => x.PlantId == id);
            var filteredPlants = tanks.Where(x => x.IsDeleted == false);
            return new ApiResponse
            {
                Message = "All Fees found",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = filteredPlants
            };
        }

        public async Task<ApiResponse> EditPlant(int Id, PlantDTO plant)
        {
            var user = await _userManager.FindByEmailAsync(User);          
            

            var updatePlant = await _unitOfWork.Plant.FirstOrDefaultAsync(x => x.Id == Id);
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
                updatePlant.PlantType = plant.PlantType;
                updatePlant.ElpsPlantId = plant.PlantElpsId;
                updatePlant.CompanyElpsId = user.ElpsId;
                updatePlant.Email = user.Email;
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

        public async Task<ApiResponse> EditPlantTanks(int Id, PlantTankDTO tank)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                var updatePlant = await _unitOfWork.PlantTank.FirstOrDefaultAsync(x => x.PlantTankId == Id);
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
                if (user == null)
                {
                    _response = new ApiResponse
                    {
                        Message = "You need to LogIn to get Create Plants",
                        StatusCode = HttpStatusCode.MethodNotAllowed,
                        Success = false
                    };
                    return _response;
                }
                var companyDetails = _elps.GetCompanyDetailByEmail(user.Email);

                var comName = companyDetails["name"];
                var allExistingPlants = GetAllPlantsNamesInCompany(user.Email);
                bool exist = allExistingPlants.Any(x => x.Name == plant.Name);
                if (exist)
                {
                    _response = new ApiResponse
                    {
                        Message = "A Plant that belongs to this company already has this Name",
                        StatusCode = HttpStatusCode.MethodNotAllowed,
                        Success = false
                    };
                    return _response;
                }
                var facility = new Plant
                {
                    Name = plant.Name,
                    PlantType = 2,
                    State = plant.State,
                    Company = comName,
                    Email = user.Email,
                    ElpsPlantId = plant.PlantElpsId,
                    CompanyElpsId = user.ElpsId,
                    IsDeleted = false,
                };
                await _unitOfWork.Plant.Add(facility);
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

        public async Task<ApiResponse> CreatePlantTank(PlantTankDTO plant, int id)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User);
                if (user == null)
                {
                    _response = new ApiResponse
                    {
                        Message = "Please Log In to create a Tank",
                        StatusCode = HttpStatusCode.Forbidden,
                        Success = false
                    };
                    return _response;
                }
                var getPlant = await _unitOfWork.Plant.FirstOrDefaultAsync( x => x.Id == id);
                var existingTankNames = GetAllTanksNamesInPlant(id);
                if (getPlant.IsDeleted == true)
                {
                    _response = new ApiResponse
                    {
                        Message = "This Plant has been Deleted.",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                    return _response;
                }
                bool exist = existingTankNames.Any(x => x.TankName == plant.TankName);
                if (exist)
                {
                    _response = new ApiResponse
                    {
                        Message = "This Tank Name exists in this Plant already.",
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Success = false
                    };
                    return _response;
                }
                var tank = new PlantTank
                {
                   PlantId = id,
                   Capacity = plant.Capacity,
                   Position = plant.Position,
                   Product = plant.Product,
                   TankName = plant.TankName
                   
                };
                await _unitOfWork.PlantTank.Add(tank);
                await _unitOfWork.SaveChangesAsync(user.Id);              

                _response = new ApiResponse
                {
                    Message = "Tank was added to the Plant successfully.",
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
                var deactivePlantTank = await _unitOfWork.PlantTank.FirstOrDefaultAsync(a => a.PlantTankId == id);
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
            var apiUrl = "https://depotonline.nmdpra.gov.ng/Application/DepotFacilityReports";
            try
            {
                var depotsInDb = await _unitOfWork.Plant.GetAll();
                List<Plant>  depotsExist = depotsInDb.ToList();
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);
                
                if (data.Any())
                {
                    var depotList = new List<Plant>();

                    foreach (var item in data)
                    {
                        
                        var plant = new Plant
                        {
                            Name = item.depotName,
                            ElpsPlantId = item.depotElpsId,
                            Email = item.email,
                            State = item.state,
                            Company = item.company,
                            PlantType = 2,
                            CompanyElpsId = item.companyElpsId,
                        };
                       
                        if (item.tanks != null)
                        {
                            var tankList = new List<PlantTank>();
                            foreach (var tank in item.tanks)
                            {
                                var pTank = new PlantTank
                                {
                                    TankName = tank.tankName,
                                    Product = tank.product,
                                    Capacity = tank.capacity,
                                    Position = tank.position
                                };

                                tankList.Add(pTank);
                            }
                            plant.Tanks = tankList;
                        }
                        bool plantExist = depotsExist.Any(x => x.CompanyElpsId == plant.CompanyElpsId);
                        if (!plantExist)
                        {
                            depotList.Add(plant);
                        }
                        else
                        {
                            continue;
                        }
                        
                    }
                    
                    var distinctDepots = depotList.Distinct().ToList();
                    await _unitOfWork.Plant.AddRange(distinctDepots);
                    await _unitOfWork.SaveChangesAsync("");

                }
                _response = new ApiResponse
                {
                    Message = "Successfully Pulled data from Depot project into Plant Table",
                    StatusCode = HttpStatusCode.OK,
                    Success = true

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

        private List<Plant> GetPlantsByCompanywithTanks(string email)
        {
           
            var plist = _unitOfWork.Plant.Query()
                .Select(x => new Plant
                {
                    Name = x.Name,
                    Id = x.Id,
                    Company = x.Company,
                    ElpsPlantId = x.ElpsPlantId,
                    CompanyElpsId = x.ElpsPlantId,
                    Email = x.Email,
                    PlantType = x.PlantType,
                    State = x.State,
                    IsDeleted = x.IsDeleted,
                    Tanks = x.Tanks.Where(u => !u.IsDeleted).ToList()
                })
                .Where(x => x.Email == email).ToList();
            return plist;
        }

        private List<Plant> GetAllPlantswithTanks()
        {

            var plist = _unitOfWork.Plant.Query()
                        .Select(x => new Plant
                        {
                            Name = x.Name,
                            Id = x.Id,
                            Company = x.Company,
                            ElpsPlantId = x.ElpsPlantId,
                            CompanyElpsId = x.ElpsPlantId,
                            Email = x.Email,
                            PlantType = x.PlantType,
                            State = x.State,
                            IsDeleted = x.IsDeleted,
                            Tanks = x.Tanks.Where(u => !u.IsDeleted).ToList()                            
                        })                        
                        .ToList();
            return plist;
        }

        private List<Plant> GetAllPlantsNamesInCompany(string email)
        {

            var plist = _unitOfWork.Plant.Query()
                        .Where(x => x.Email == email)
                        .Select(x => new Plant
                        {
                            Name = x.Name                           
                        })                        
                        .ToList();
            return plist;
        }

        private List<PlantTank> GetAllTanksNamesInPlant(int id)
        {

            var plist = _unitOfWork.PlantTank.Query()
                        .Where(x => x.PlantId == id)
                        .Select(x => new PlantTank
                        {
                            TankName = x.TankName
                        })                        
                        .ToList();
            return plist;
        }
    }
}
