using Azure;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Bunkering.Access.Services
{
	public class StaffService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IElps _elps;
		private string User;
		ApiResponse _response;

		public StaffService(
			IUnitOfWork unitOfWork,
			UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			IHttpContextAccessor contextAccessor, IElps elps)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_roleManager = roleManager;
			_elps = elps;
			User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
		}

		public async Task<ApiResponse> Dashboard()
		{
			var user = await _userManager.FindByEmailAsync(User);
			var allApps = await _userManager.IsInRoleAsync(user, "Company") ? await _unitOfWork.Application.Find(x => x.UserId.Equals(user.Id), "Payments") : await _unitOfWork.Application.GetAll("Payments");
			//var apps = await _userManager.IsInRoleAsync(user, Roles.FAD)
			//	? (allApps.Where(x => x.FADStaffId.Equals(user.Id) && !x.FADApproved && x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Processing))))
			//	: (allApps.Where(x => x.CurrentDeskId.Equals(user.Id)));
			var apps = allApps.Where(x => x.CurrentDeskId.Equals(user.Id));
			var permits = await _userManager.IsInRoleAsync(user, "Company") ? await _unitOfWork.Permit.Find(x => x.Application.UserId.Equals(user.Id), "Application") : await _unitOfWork.Permit.GetAll("Application");
			var facilities = await _userManager.IsInRoleAsync(user, "Company") ? await _unitOfWork.Facility.Find(x => x.CompanyId.Equals(user.CompanyId), "VesselType") : await _unitOfWork.Facility.GetAll("VesselType");
			var payments = await _userManager.IsInRoleAsync(user, "Company") ? await _unitOfWork.Payment.Find(x => allApps.Any(a => a.Id.Equals(x.ApplicationId))) : await _unitOfWork.Payment.GetAll();

			if (user != null)
				_response = new ApiResponse
				{
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
					Success = true,
					Data = new
					{
						DeskCount = apps.Count(),
						TotalApps = allApps.Count(),
						TMobileFacs = 0,
						TFixedFacs = 0,
						TotalLicenses = permits.Count(),
						TLicensedfacs = facilities.Count(x => x.IsLicensed),
						TValidLicense = permits.Count(x => x.ExpireDate > DateTime.UtcNow.AddHours(1)),
						TAmount = payments.Where(x => x.Status.ToLower().Equals(Enum.GetName(typeof(AppStatus), AppStatus.PaymentCompleted))).Sum(x => x.Amount),
						TProcessing = allApps.Count(x => x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Processing))),
						TApproved = allApps.Count(x => x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Completed))),
						TRejected = allApps.Count(x => x.Status.Equals(Enum.GetName(typeof(AppStatus), AppStatus.Rejected))),
						TExpiring30 = await _userManager.IsInRoleAsync(user, "Company")
						? permits.Count(x => x.Application.UserId.Equals(user.Id) && x.ExpireDate.AddDays(30) >= DateTime.UtcNow.AddHours(1))
						: permits.Count(x => x.ExpireDate.AddDays(30) >= DateTime.UtcNow.AddHours(1))
					}
				};
			else
				_response = new ApiResponse
				{
					Message = "No Application was found",
					StatusCode = HttpStatusCode.NotFound,
					Success = false
				};

			return _response;
		}

		public async Task<ApiResponse> AllUsers()
		{
			var users = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
				.Include(lo => lo.Location)
				.Include(ol => ol.Office).Where(x => x.CompanyId == null && !x.IsDeleted).ToList();
			var apps = await _unitOfWork.Application.GetAll();
			return new ApiResponse
			{
				Message = "Users found",
				StatusCode = HttpStatusCode.OK,
				Success = true,
				Data = users.Select(x => new
				{
					x.Id,
					Name = $"{x.FirstName} {x.LastName}",
					x.Email,
					Role = x.UserRoles.FirstOrDefault()?.Role.Name,
					x.LocationId,
					x.OfficeId,
					x.PhoneNumber,
					x.CreatedBy,
					DateCreated = x.CreatedOn,
					x.IsActive,
					AppCount = apps != null && apps.Count() != 0? apps.Count(y => y.UserId.Equals(x.Id)): 0,

				})
			};
		}

		public async Task<ApiResponse> Create(UserViewModel model)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(User);
				var staff = await _userManager.FindByEmailAsync(model.Email);
				if (staff == null)
				{
					staff = new ApplicationUser
					{
						ElpsId = model.ElpsId,
						Email = model.Email,
						UserName = model.Email,
						EmailConfirmed = true,
						PhoneNumber = model.Phone,
						IsActive = true,
						FirstName = model.FirstName,
						LastName = model.LastName,
						LocationId = model.LocationId,
						OfficeId = model.OfficeId,
						CreatedBy = user.Email,
						CreatedOn = DateTime.UtcNow.AddHours(1),

					};
					await _userManager.CreateAsync(staff);

					var role = await _roleManager.FindByIdAsync(model.RoleId);
					if (role != null)
						await _userManager.AddToRoleAsync(staff, role.Name);

					_response = new ApiResponse
					{
						Message = "User was profiled successfully.",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				else
				{
					_response = new ApiResponse
					{
						Message = "User already exists",
						StatusCode = HttpStatusCode.BadRequest,
						Success = false
					};
					if (staff.IsDeleted)
						_response.Message += " and has been deleted. Please contact Admin to restore the user.";
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

		public async Task<ApiResponse> Edit(UserViewModel model)
		{
			try
			{
				var user = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(x => x.Id.Equals(model.Id));
				if (user != null)
				{
					if (!model.Email.Equals(user.Email))
					{
						var checkEmail = await _userManager.FindByEmailAsync(model.Email);
						if (checkEmail != null)
						{
							return new ApiResponse
							{
								Message = "Email Already Exist",
								StatusCode = HttpStatusCode.BadRequest,
								Success = false
							};
						}

					}
					var role = await _roleManager.FindByIdAsync(model.RoleId);
					var apps = await _unitOfWork.Application.Find(x => x.CurrentDeskId.Equals(user.Id));
					if (apps != null && apps.Count() > 0)
					{
						if (role != null && !await _userManager.IsInRoleAsync(user, role.Name))
						{
							return new ApiResponse
							{
								Message = "There are pending applications on the staff desk, pls reroute and try again",
								StatusCode = HttpStatusCode.BadRequest,
								Success = false
							};
						}
					}
					user.PhoneNumber = model.Phone;
					user.Email = model.Email;
					user.FirstName = model.FirstName;
					user.LastName = model.LastName;
					user.IsActive = model.IsActive;

					await _userManager.UpdateAsync(user);

					if (!await _userManager.IsInRoleAsync(user, role.Name))
					{
						await _userManager.RemoveFromRoleAsync(user, user.UserRoles.FirstOrDefault().Role.Name);
						await _userManager.AddToRoleAsync(user, role.Name);
					}
					_response = new ApiResponse
					{
						Message = "Staff profile updated successfully!",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				else
				{
					_response = new ApiResponse
					{
						Message = "Staff not found",
						StatusCode = HttpStatusCode.BadRequest,
						Success = false
					};
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

		public async Task<ApiResponse> AllElpsStaff()
		{
			var elpsStaff = _elps.GetAllStaff();
			var result = new ApiResponse
			{
				Data = elpsStaff,
				Message = "All Elps Staff",
				StatusCode = HttpStatusCode.OK,
				Success = true

			};
			return result;
		}
		//test
		public async Task<ApiResponse> DeleteUser(string id)
		{
			var deactive = await _userManager.Users.FirstOrDefaultAsync(s => s.Id.Equals(id));
			if (deactive != null)
			{
				if (!deactive.IsDeleted)
				{
					deactive.IsDeleted = true;
					await _userManager.UpdateAsync(deactive);

					_response = new ApiResponse
					{
						Data = deactive,
						Message = "User has been deleted",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				_response = new ApiResponse
				{
					Data = deactive,
					Message = "User is already deleted",
					StatusCode = HttpStatusCode.OK,
					Success = true
				};
			}

			return _response;
		}

	}
}
