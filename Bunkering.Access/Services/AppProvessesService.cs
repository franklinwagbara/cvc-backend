using AutoMapper;
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
	public class AppProvessesService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly RoleManager<ApplicationRole> _role;
		private readonly UserManager<ApplicationUser> _userManager;
		ApiResponse _response;
		private string User;
		IMapper _mapper;

		public AppProvessesService(
			IUnitOfWork unitOfWork,
			IHttpContextAccessor contextAccessor,
			RoleManager<ApplicationRole> role,
			UserManager<ApplicationUser> userManager,
			IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_role = role;
			_contextAccessor = contextAccessor;
			User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task<ApiResponse> Index()
		{
			var processes = (await _unitOfWork.Workflow.GetAll()).ToList();
			if (processes != null)
			{
				var flows = new List<WorkflowviewModel>();
				var roles = await _role.Roles.ToListAsync();
				var apptyeps = await _unitOfWork.ApplicationType.GetAll();
				var vesselType = await _unitOfWork.VesselType.GetAll();
				var location = await _unitOfWork.Location.GetAll();

				processes.ForEach(r =>
				{
					var trole = roles.FirstOrDefault(x => x.Id.ToLower().Equals(r.TriggeredByRole.ToLower()));
					var rrole = roles.FirstOrDefault(x => x.Id.ToLower().Equals(r.TargetRole.ToLower()));
					var flow = _mapper.Map<WorkflowviewModel>(r);

					flow.FromLocation = location.FirstOrDefault(l => l.Id.Equals(r.FromLocationId))?.Name;
					flow.FromLocationId = location.FirstOrDefault(l => l.Id.Equals(r.FromLocationId))?.Id;
                    flow.ToLocation = location.FirstOrDefault(t => t.Id.Equals(r.ToLocationId))?.Name;
                    flow.ToLocationId = location.FirstOrDefault(t => t.Id.Equals(r.ToLocationId))?.Id;
                    flow.TriggeredByRole = trole?.Name;
					flow.TriggeredByRoleId = trole?.Id;
					flow.TargetRole = rrole?.Name;
					flow.TargetRoleId = rrole?.Id;
					flow.ApplicationType = r.ApplicationTypeId != null ? apptyeps.FirstOrDefault(x => x.Id.Equals(r.ApplicationTypeId))?.Name : "N/A";
					flow.ApplicationTypeId = r.ApplicationTypeId != null ? apptyeps.FirstOrDefault(x => x.Id.Equals(r.ApplicationTypeId))?.Id : 1;
					flow.VesselType = vesselType.FirstOrDefault(x => x.Id.Equals(r.VesselTypeId))?.Name;
					flow.VesselTypeId = vesselType.FirstOrDefault(x => x.Id.Equals(r.VesselTypeId))?.Id;
                    flows.Add(flow);
				});

				_response = new ApiResponse
				{
					Message = "App processes fetched successfully",
					StatusCode = HttpStatusCode.OK,
					Success = true,
					Data = flows
				};
			}
			else
				_response = new ApiResponse
				{
					Message = "No processes found",
					StatusCode = HttpStatusCode.NotFound,
					Success = false,
				};
			return _response;
		}

		public async Task<ApiResponse> AddFlow(WorkflowviewModel model)
		{
			var user = await _userManager.FindByEmailAsync(User);

			var wk = _mapper.Map<WorkFlow>(model);
			//wk.TriggeredByRole = (await _role.Roles.FirstOrDefaultAsync(x => x.Id == model.TriggeredByRoleId))?.Name;
			//wk.TargetRole = (await _role.Roles.FirstOrDefaultAsync(x => x.Id == model.TargetRoleId))?.Name;

			await _unitOfWork.Workflow.Add(wk);
			await _unitOfWork.SaveChangesAsync(user.Id);

			return new ApiResponse
			{
				Message = "New workflow added successfully!",
				StatusCode = HttpStatusCode.OK,
				Success = true,
			};
		}

		public async Task<ApiResponse> EditFlow(WorkflowviewModel model)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(User);
				var flow = await _unitOfWork.Workflow.FirstOrDefaultAsync(x => x.Id == model.Id);
				if (flow != null)
				{
					flow.Rate = model.Rate;
					flow.Status = model.Status;
					flow.TargetRole = model.TargetRoleId;
					flow.Action = model.Action;
					flow.TriggeredByRole = model.TriggeredByRoleId;
					flow.VesselTypeId = (int)model.VesselTypeId;
					flow.FromLocationId = model.FromLocationId;
					flow.ToLocationId = model.ToLocationId;

					await _unitOfWork.Workflow.Update(flow);
					await _unitOfWork.SaveChangesAsync(user.Id);

					_response = new ApiResponse
					{
						Message = "Workflow updated successfully",
						StatusCode = HttpStatusCode.OK,
						Success = true
					};
				}
				else
					_response = new ApiResponse
					{
						Message = "Work flow not found",
						StatusCode = HttpStatusCode.NotFound,
						Success = false
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

		public async Task<ApiResponse> CloneFlow(int id)
		{
			if (id == 2)
			{
				var wk = new List<WorkFlow>();
				var user = await _userManager.FindByEmailAsync(User);
				var processes = (await _unitOfWork.Workflow.Find(x => x.VesselTypeId == 1)).ToList();
				if (processes != null)
				{
					processes.ForEach(x =>
					{
						wk.Add(new WorkFlow
						{
							Action = x.Action,
							VesselTypeId = id,
							Rate = x.Rate,
							Status = x.Status,
							TargetRole = x.TargetRole,
							TriggeredByRole = x.TriggeredByRole
						});
					});

					await _unitOfWork.Workflow.AddRange(wk);
					await _unitOfWork.SaveChangesAsync(user.Id);
				}
			}
			return new ApiResponse
			{
				Message = "Clone was successful",
				StatusCode = HttpStatusCode.OK,
				Success = true
			};
		}

		public async Task<ApiResponse> ArchiveFlow(int id)
		{
			var archive = await _unitOfWork.Workflow.FirstOrDefaultAsync(x => x.Id == id);
			if (archive != null)
			{
				archive.IsArchived = true;

				_response = new ApiResponse
				{
					Data = archive,
					Message = "WorkFlow has been deleted",
					StatusCode = HttpStatusCode.OK,
					Success = true
				};
			}
			else
			{
				_response = new ApiResponse
				{
					Data = archive,
					Message = "WorkFlow has been deleted already",
					StatusCode = HttpStatusCode.OK,
					Success = true
				};
			}
			return _response;
		}
	}
}
