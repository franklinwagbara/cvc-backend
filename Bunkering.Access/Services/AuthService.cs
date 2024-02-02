using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Bunkering.Access.DAL;
using Newtonsoft.Json.Linq;
using Bunkering.Core.Utils;

namespace Bunkering.Access.Services
{
	public class AuthService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<ApplicationUser> _user;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
		ApiResponse _response;
		private readonly IElps _elps;
		private readonly string User;
		private readonly IConfiguration _configuration;
		private readonly AppSetting _appSetting;

		public AuthService(
			IHttpContextAccessor httpContextAccessor,
			UserManager<ApplicationUser> user,
			SignInManager<ApplicationUser> signInManager,
			IElps elps,
			IConfiguration configuration,
			IOptions<AppSetting> appSetting,
			IUnitOfWork unitOfWork)
		{
			_httpContextAccessor = httpContextAccessor;
			_user = user;
			_signInManager = signInManager;
			_elps = elps;
			User = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
			_configuration = configuration;
			_appSetting = appSetting.Value;
			_unitOfWork = unitOfWork;
		}

		public async Task<ApiResponse> UserAuth(LoginViewModel model)
		{
			var user = new ApplicationUser();
			var hash = Utils.GenerateSha512($"{_appSetting.AppEmail}.{model.Email.ToUpper()}.{_appSetting.AppId.ToUpper()}");
			//if (Debugger.IsAttached || (!Debugger.IsAttached && model.Code.Equals(hash)))
			//{
			//check if it's company login
			var company = _elps.GetCompanyDetailByEmail(model.Email);

			if (company.Count > 0)
			{
				int compelspid = int.Parse(company.GetValue("id"));
				user = _user.Users.Include(x => x.Company).FirstOrDefault(x => x.ElpsId == compelspid);

				if (user == null)
					user = await RegisterCompany(company);
				else if (user != null && !user.Email.Equals(model.Email))
				{
					var addid = company.GetValue("registered_Address_Id");
					user.Email = model.Email;
					user.UserName = model.Email;
					user.Company.AddressId = string.IsNullOrEmpty(addid)
						? 0 : int.Parse(addid);
					user.Company.Name = company.GetValue("name");
					user.PhoneNumber = company.GetValue("contact_phone");

					var result = await _user.UpdateAsync(user);
				}
			}
			else
			{
				//get staffn
				var staff = _elps.GetStaff(model.Email);
				if (staff != null)
				{
					user = _user.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault(x => x.ElpsId.Equals(staff.Id));
					if (user != null)
					{
						user.ElpsId = staff.Id;
						user.FirstName = staff.FirstName;
						user.LastName = staff.LastName;
						user.IsActive = true;
						user.ProfileComplete = true;

						if (!user.Email.Equals(model.Email))
						{
							user.Email = model.Email;
							user.UserName = model.Email;

							var result = await _user.UpdateAsync(user);
						}
					}
					else
					{
						user = new ApplicationUser
						{
							ElpsId = staff.Id,
							FirstName = staff.FirstName,
							LastName = staff.LastName,
							Email = staff.Email.ToLower(),
							UserName = staff.Email.ToLower(),
							IsActive = true,
							CreatedOn = DateTime.UtcNow.AddHours(1),
							CreatedBy = "system"
						};
						var result = await _user.CreateAsync(user);

						if (result.Succeeded)
							await _user.AddToRoleAsync(user, "Staff");
					}
				}
			}
			if (user is { IsActive: true })
			{
				await _signInManager.SignInAsync(user, false);

				if (await _user.IsInRoleAsync(user, "Staff"))
					return new ApiResponse
					{
						Message = "Access to this portal is denied, please contact ICT/Support.",
						StatusCode = HttpStatusCode.BadRequest,
						Success = false
					};
				else
				{
					// await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
					//     principal,
					//     new AuthenticationProperties
					//     {
					//         IsPersistent = true,
					//         ExpiresUtc = DateTime.Now.AddMinutes(60)
					//     });
					_response = new ApiResponse
					{
						Message = "Company's profile is complete",
						StatusCode = HttpStatusCode.OK,
						Success = true,
						Data = user.Id
					};
					if (!user.ProfileComplete && _user.IsInRoleAsync(user, "Company").Result)
						_response.Message = "Company's profile is not complete";
					if (user.ProfileComplete && _user.IsInRoleAsync(user, "Company").Result)
						_response.Message = "Company's profile is complete";
				}
			}
			else if (user is { IsActive: false })
				_response = new ApiResponse
				{
					Message = "Access to this portal is denied, please contact ICT/Support.",
					StatusCode = HttpStatusCode.Forbidden,
					Success = false,
				};
			else
				_response = new ApiResponse
				{
					Message = "An error occurred, please contact Support/ICT.",
					StatusCode = HttpStatusCode.InternalServerError,
					Success = false,
				};
			//}
			return _response;
		}

		public async Task<ApiResponse> ValidateUser(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				var user = await _user.Users.Include(c => c.Company)
					.Include(c => c.Office)
					.Include(c => c.Location)
					.Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
					.FirstOrDefaultAsync(x => x.Id.Equals(id));

				//var operationFacility = await _unitOfWork.OperatingFacility.FirstOrDefaultAsync(x => x.CompanyId == user.CompanyId);
				if (user != null)
				{

					string plantTypeName = null;
					if(user.Company != null && user.Company.OperatingFacilityId > 0)
					{
						plantTypeName = Enum.GetName(typeof(PlantType), user.Company.OperatingFacilityId);
                    }

                    _response = new ApiResponse
					{
						Message = "User found",
						StatusCode = HttpStatusCode.OK,
						Success = true,
						Data = new
						{
							UserId = user.Email,
							UserUuid = user.Id,
							user.ElpsId,
							FirstName = user.UserRoles.FirstOrDefault()?.Role?.Name?.Equals("Company") is not true ? user.FirstName : user.Company.Name,
							user.LastName,
							UserRoles = user.UserRoles.FirstOrDefault(x => x.Role?.Name?.Equals("Staff") is false)?.Role?.Name,
							user.CreatedOn,
							user.LastLogin,
							user.ProfileComplete,
							Status = user.IsActive,
							Location = user.Location?.Name,
							Office = user.Office?.Name,
							Directorate = user.Directorate,
							operationFacility = plantTypeName,

                            Token = GenerateToken(user, user.UserRoles.FirstOrDefault(x => x.Role?.Name?.Equals("Staff") is false)?.Role?.Name, plantTypeName)
						},
					};
				}
				else
					_response = new ApiResponse { Message = "User not found", StatusCode = HttpStatusCode.NotFound };
			}
			else
				_response = new ApiResponse { Message = "User invalid", StatusCode = HttpStatusCode.BadRequest };

			return _response;
		}

		private string GenerateToken(ApplicationUser user, string? role, string operatingFacilityName = null)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
			var claims = new List<Claim>();
			if (user.Office != null)
			{
				claims.Add(new Claim("office", user.Office.Name));
			}
			if (user.Location != null)
			{
				claims.Add(new Claim("location", user.Location.Name));
			}
			if (user.Company != null)
			{
				claims.Add(new Claim("company", user.Company.Name));
				if(operatingFacilityName != null)
				{
                    claims.Add(new Claim("operation facility", operatingFacilityName));
                }
				
			}
			if (role != null)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
					
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.PrimarySid, user.Id),
					new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			tokenDescriptor.Subject.AddClaims(claims);
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		private async Task<ApplicationUser> RegisterCompany(Dictionary<string, string> dic)
		{
			var elpsaddid = dic.GetValue("registered_Address_Id");
			var company = new Company
			{
				Name = dic.GetValue("name"),
				//Nationality = dic.GetValue("nationality"),

				RcNumber = dic.GetValue("rC_Number"),
				TinNumber = dic.GetValue("tin_Number"),
				YearIncorporated = dic.GetValue("year_Incorporated"),
				AddressId = string.IsNullOrEmpty(elpsaddid) ? 0 : int.Parse(elpsaddid),
			};

			var user = new ApplicationUser
			{
				UserName = dic.GetValue("user_id"),
				Email = dic.GetValue("user_id"),
				EmailConfirmed = true,
				ElpsId = int.Parse(dic.GetValue("id")),
				PhoneNumber = dic.GetValue("contact_Phone"),
				ProfileComplete = false,
				FirstName = dic.GetValue("contact_firstname"),
				LastName = dic.GetValue("contact_lastname"),
				Company = company,
				IsActive = true,
				CreatedOn = DateTime.UtcNow.AddHours(1),
				CreatedBy = "system"
			};
			await _user.CreateAsync(user);
			await _user.AddToRoleAsync(user, "Company");

			return user;
		}

		public async Task<ApiResponse> ChangePassword(PasswordViewModel model)
		{
			bool status = false;
			try
			{
				var response = _elps.ChangePassword(new
				{
					model.OldPassword,
					model.NewPassword,
					ConfirmPassword = model.CPassword
				}, User);

				if (response != null)
				{
					if (response.GetValue("msg").Equals("ok", StringComparison.OrdinalIgnoreCase) && response.GetValue("code").Equals("1"))
					{
						_response = new ApiResponse
						{
							Message = "Password changed successfully, please login again to continue",
							StatusCode = HttpStatusCode.OK,
							Success = true
						};
					}
					else
						_response = new ApiResponse
						{
							Message = "Password change unsuccessful, please try again",
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
	}
}
