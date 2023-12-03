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
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
	public class LocationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		ApiResponse _response;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly WorkFlowService _flow;


		public LocationService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, WorkFlowService flow)
		{
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_flow = flow;
		}

		public async Task<ApiResponse> CreateLocation(LocationViewModel model)
		{
			try
			{
				var loc = await _unitOfWork.Location.FirstOrDefaultAsync(l => l.Name == model.Name);
				if (loc == null)
				{
					var location = new Location
					{
						Name = model.Name,
					};

					await _unitOfWork.Location.Add(location);
					await _unitOfWork.SaveChangesAsync("");

					_response = new ApiResponse
					{
						Data = location,
						Message = "Location Created",
						StatusCode = HttpStatusCode.OK,
						Success = true,
					};

				}
				else if (loc != null)
				{
					_response = new ApiResponse
					{
						Message = "Location Already Exist",
						StatusCode = HttpStatusCode.Found,
						Success = true
					};
				}
				else
				{
					_response = new ApiResponse
					{
						Message = "Unable to Create Location",
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


		public async Task<ApiResponse> EditLocation(LocationViewModel model)
		{
			var location = await _unitOfWork.Location.FirstOrDefaultAsync(x => x.Id == model.Id);
			if (location != null)
			{
				location.Name = model.Name;

				await _unitOfWork.Location.Update(location);
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
					Message = "Location not Found",
					StatusCode = HttpStatusCode.NotFound,
					Success = false,
				};
			}
			return _response;

		}

	}
}
