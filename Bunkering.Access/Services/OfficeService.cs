using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
	public class OfficeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		ApiResponse _response;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly WorkFlowService _flow;
		public string User;

		public OfficeService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, WorkFlowService flow)
		{
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_flow = flow;
			User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

		}

		public async Task<ApiResponse> CreateOffice(OfficeViewModel model)
		{
			try
			{
				var off = await _unitOfWork.Office.FirstOrDefaultAsync(x => x.Name == model.Name && x.StateId == model.StateId);
				if (off == null)
				{
					var office = new Office
					{
						Name = model.Name,
						StateId = model.StateId,
					};

					await _unitOfWork.Office.Add(office);
					await _unitOfWork.SaveChangesAsync("");

					_response = new ApiResponse
					{
						Data = office,
						Message = "Office Created",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				else if (off != null)
				{
					_response = new ApiResponse
					{
						Message = "Office Already Exist",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				else
				{
					_response = new ApiResponse
					{
						Message = "Unable to Create Office",
						StatusCode = HttpStatusCode.BadRequest,
						Success = false
					};

				}

			}
			catch (Exception ex)
			{
				_response = new ApiResponse { Message = ex.Message };

			}
			return _response;

		}
		public async Task<ApiResponse> EditOffice(OfficeViewModel model)
		{
			try
			{
				var office = await _unitOfWork.Office.FirstOrDefaultAsync(x => x.Id == model.Id);
				if (office != null)
				{
					office.Name = model.Name;
					office.StateId = model.StateId;

					await _unitOfWork.Office.Update(office);
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
						Message = "Office not Found",
						StatusCode = HttpStatusCode.BadRequest,
						Success = false,
					};

				}

			}
			catch (Exception ex)
			{
				_response = new ApiResponse { Message = ex.Message };
			}
			return _response;
		}
	}

}
