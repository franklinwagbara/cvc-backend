﻿using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Bunkering.Access.Services
{
	public class CompanyService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly string User;
		private readonly UserManager<ApplicationUser> _userManager;
		ApiResponse _response;
		private readonly IElps _elps;
		private readonly WorkFlowService _flow;

		public CompanyService(
			IUnitOfWork unitOfWork,
			IHttpContextAccessor contextAccessor,
			IElps elps,
			UserManager<ApplicationUser> userManager,
			WorkFlowService flow)
		{
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
			_elps = elps;
			_userManager = userManager;
			_flow = flow;
		}


		//public async Task<ApiResponse> Dashboard()
		//{
		//    //ViewData["Message"] = TempData["Message"];
		//    var company = await _userManager.FindByEmailAsync(User);
		//    if(company != null)
		//        _response = new ApiResponse
		//        {
		//            Message = "Company not found",
		//            StatusCode = HttpStatusCode.OK,
		//            Success = false
		//        };

		//    if (!company.ProfileComplete)
		//        _response = new ApiResponse
		//        {
		//            Message = "Company profile update not completed",
		//            StatusCode = HttpStatusCode.OK,
		//            Success = false
		//        };
		//    else
		//    {
		//        var messages = (await _unitOfWork.Message.Find(x => x.UserId.Equals(company.Id))).ToList();
		//        var apps = (await _unitOfWork.Application.Find(x => x.UserId.Equals(company.Id), "User.Company")).ToList();
		//        var users = await _userManager.FindByEmailAsync(User);

		//        _response = new ApiResponse
		//        {
		//            Message = "Success",
		//            StatusCode = HttpStatusCode.OK,
		//            Success = true,
		//            Data = new
		//            {
		//                Messages = messages,
		//                Apps = apps.Count > 0 ? apps.Select(x => new
		//                {
		//                    CompanyEmail = x.User.Email,
		//                    CompanyName = x.User.Company.Name,
		//                    FacilityAddress = x.Facility,
		//                    FacilityType = x.Facility.FacilityType.Name,
		//                    State = x.Facility.LGA.State.Name,
		//                    x.Reference,
		//                    x.Status,
		//                    CreatedDate = x.CreatedDate.ToString("MMMM dd, yyyy HH:mm:ss")
		//                }) : null
		//            }
		//        };
		//    }
		//    return _response;
		//}

		public async Task<ApiResponse> GetProfile(string email = null)
		{
			try
			{
				var nations = new List<object>();
				var user = _userManager.Users.Include("Company").FirstOrDefault(x => x.Email == User);
				if (!string.IsNullOrEmpty(email))
					user = _userManager.Users.Include("Company").FirstOrDefault(x => x.UserName == email);

				var countries = await _unitOfWork.Country.GetAll();
				var company = _elps.GetCompanyDetailByEmail(user.Email).Stringify().Parse<CompanyModel>();
				var companyAdd = new RegisteredAddress();
				if (user.Company.AddressId > 0)
					companyAdd = _elps.GetCompanyRegAddressById(user.Company.AddressId.Value).Stringify().Parse<RegisteredAddress>();
				else
					companyAdd = _elps.GetCompanyRegAddress(user.ElpsId).FirstOrDefault();

				foreach (var country in countries)
				{
					if (country.Name == company.nationality)
					{
						nations.Add(new
						{
							Text = country.Name,
							Value = country.Id,
							Selected = true
						});
					}
					else
					{
						nations.Add(new
						{
							Text = country.Name,
							Value = country.Id,
							Selected = true
						});
					}
				}
				_response = new ApiResponse
				{
					Data = new
					{
						Company = company,
						RegisteredAddress = companyAdd,
						Nations = nations
					},
					Success = true,
					Message = "Company profile found",
					StatusCode = HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				_response = new ApiResponse
				{
					Success = false,
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError
				};
			}
			return _response;
		}

		public async Task<ApiResponse> UpdateProfile(CompanyInformation model, string oldemail = null)
		{
			var user = _userManager.Users.Include("Company").FirstOrDefault(x => x.Email == User);
			try
			{

				if (await _userManager.IsInRoleAsync(user, "Support") || await _userManager.IsInRoleAsync(user, "ICT")
					|| await _userManager.IsInRoleAsync(user, "SuperAdmin"))
				{
					var companyuser = _userManager.Users.Include("Company").FirstOrDefault(x => x.Email == oldemail);
					model.Company.id = companyuser.ElpsId;
					_elps.UpdateCompanyDetails(model.Company, model.Company.user_Id, true)?.Stringify()?.Parse<CompanyModel>();

					if (!model.Company.user_Id.Equals(companyuser.Email, StringComparison.OrdinalIgnoreCase)) ;
					{
						companyuser.Email = model.Company.user_Id;
						companyuser.NormalizedEmail = model.Company.user_Id;
						companyuser.NormalizedUserName = model.Company.user_Id;
						companyuser.UserName = model.Company.user_Id;
					}
					companyuser.Company.Name = model.Company.name;
					companyuser.Company.RcNumber = model.Company.rC_Number;
					companyuser.Company.TinNumber = model.Company.tin_Number;
					companyuser.Company.YearIncorporated = model.Company.year_Incorporated;

					await _userManager.UpdateAsync(companyuser);
					_response = new ApiResponse
					{
						Message = "Company was updated successfully",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				else if (await _userManager.IsInRoleAsync(user, "Company"))
				{
					if (model.Company != null)
					{
						user = MapUserCompanyModel(user, model);
						model.Company.id = user.ElpsId;
						model.Company = _elps.UpdateCompanyDetails(model.Company, user.Email, false)?.Stringify()?.Parse<CompanyModel>();
					}
					else if (model.RegisteredAddress != null)
					{
						model.RegisteredAddress.country_Id = 156;
						model.RegisteredAddress.type = "Registered";
						model.RegisteredAddress.stateId = 2412;
						var addList = new List<RegisteredAddress> { model.RegisteredAddress };

						if (user.Company.AddressId > 0)
						{
							model.RegisteredAddress.id = user.Company.AddressId.Value;
							addList.Add(model.RegisteredAddress);
							var resp = _elps.UpdateCompanyRegAddress(addList);
						}
						else
						{
							var req = _elps.AddCompanyRegAddress(addList, user.ElpsId).Stringify().Parse<List<RegisteredAddress>>().FirstOrDefault();
							user.Company.AddressId = req.id;
						}
						user.Company.Address = model.RegisteredAddress.address_1;

						_response = new ApiResponse
						{
							Message = "Company was updated successfully",
							StatusCode = HttpStatusCode.OK,
							Success = true
						};
					}

					if (!string.IsNullOrEmpty(user.Company.Address) && user.Company.AddressId > 0)
						user.ProfileComplete = true;
					await _userManager.UpdateAsync(user);

					if (user.ProfileComplete)
						_response.Message = "Address update was successful";

					_response.Message = "Profile update was successful";
				}
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

		public async Task<ApiResponse> All()
		{
			var user = await _userManager.FindByEmailAsync(User);
			var apps = await _unitOfWork.Application.Find(x => x.UserId.Equals(user.Id), "ApplicationType,Facility.FacilityType,Facility.LGA.State,Payments,WorkFlow,Messages");

			return new ApiResponse
			{
				Message = "All applications fetched successfully",
				StatusCode = HttpStatusCode.OK,
				Success = true,
				Data = apps.Select(x => new
				{
					CompanyEmail = x.User.Email,
					CompanyName = x.User.Company.Name,
					VesselName = x.Facility.Name,
					VesselType = x.Facility.VesselType.Name,
					x.Facility.Capacity,
					x.Facility.DeadWeight,
					x.Reference,
					x.Status,
					CreatedDate = x.CreatedDate.ToString("MMMM dd, yyyy HH:mm:ss")
				})
			};
		}

		private ApplicationUser MapUserCompanyModel(ApplicationUser user, CompanyInformation model)
		{
			user.Company.Name = model.Company.name;
			user.UserName = model.Company.user_Id;
			user.Company.RcNumber = model.Company.rC_Number;
			user.Company.TinNumber = model.Company.tin_Number;
			user.Company.CountryId = _unitOfWork.Country.Find(x => x.Name.ToLower().Equals(model.Company.nationality.ToLower()))?.Id;
			user.Email = model.Company.user_Id;
			user.FirstName = model.Company.contact_FirstName;
			user.LastName = model.Company.contact_LastName;
			user.PhoneNumber = model.Company.contact_Phone;
			user.Company.TinNumber = model.Company.tin_Number;
			user.Company.YearIncorporated = model.Company.year_Incorporated;
			return user;
		}
	}
}
