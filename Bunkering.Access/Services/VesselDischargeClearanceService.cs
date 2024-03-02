using Azure;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class VesselDischargeClearanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHostingEnvironment _env;
        private string User;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MailSettings _mailSettings;

        public VesselDischargeClearanceService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IHostingEnvironment env, UserManager<ApplicationUser> userManager, IOptions<MailSettings> mailSettings)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _env = env;
            _userManager = userManager;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _mailSettings = mailSettings.Value;
        }

        public async Task<ApiResponse> CreateVesselDischargeClearance(VesselDischargeCleareanceViewModel model)
        {
            try
            {
                var user = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(i => i.Email.Equals(User));
                var create = await _unitOfWork.VesselDischargeClearance.FirstOrDefaultAsync(x => x.AppId == model.AppId);

                if (create != null)
                    return new ApiResponse
                    {
                        Message = "Vessel Discharge Clearance already exist",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == model.AppId);

                if (app == null)
                    return new ApiResponse
                    {
                        Message = "NOA dosen't exist",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                if (create == null)
                {
                    var vesselDischargeClearance = new VesselDischargeClearance
                    {
                        AppId = model.AppId,
                        //DischargeId = appDepot.DischargeId,
                        VesselName = model.VesselName,
                        VesselPort = model.VesselPort,
                        Product = model.Product,
                        Density = model.Density,
                        RON = model.RON,
                        FlashPoint = model.FlashPoint,
                        Color = model.Color,
                        Odour = model.Odour,
                        Oxygenate = model.Oxygenate,
                        Others = model.Others,
                        Comment = model.Comment,
                        //DepotId = model.DepotId,
                        IsAllowed   = model.IsAllowed,
                        FinalBoilingPoint = model.FinalBoilingPoint,
                    };
                    
                    await _unitOfWork.VesselDischargeClearance.Add(vesselDischargeClearance);

                    app.HasCleared = true;
                    await _unitOfWork.Application.Update(app);

                    await _unitOfWork.SaveChangesAsync(user.Id);

                    await _unitOfWork.ApplicationHistory.Add(new ApplicationHistory
                    {
                        Action = "Create",
                        ApplicationId = app.Id,
                        Comment = model.Comment,
                        Date = DateTime.UtcNow.AddHours(1),
                        TargetedTo = user.Id,
                        TargetRole = user.UserRoles.FirstOrDefault().Role.Id,
                        TriggeredBy = user.Id,
                        TriggeredByRole = user.UserRoles.FirstOrDefault().Role.Id
                    });

                    model.Id = vesselDischargeClearance.Id;

                    _response = new ApiResponse
                    {
                        Data = model,
                        Message = "Vessel Discharge Clearance Created",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
            }
            catch(Exception ex)
            {
                _response = new ApiResponse { Message = ex.Message };
            }
            return _response;
        }

        public async Task<ApiResponse> DisAllowVesselDischargeClearance(int id, string comment)
        {
            try
            {
                var user = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(x => x.Email.Equals(User));
                var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.HasCleared);

                if (app == null)
                    return new ApiResponse
                    {
                        Message = "NOA dosen't exist",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false
                    };

                //Fetch Office Supervisor
                var supervisor = _userManager.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(u => u.UserRoles.FirstOrDefault().Role.Name.Equals("Supervisor") && u.IsActive && u.LocationId.Equals(user.LocationId));

                await _unitOfWork.ApplicationHistory.Add(new ApplicationHistory
                {
                    Action = "DisAllow",
                    ApplicationId = app.Id,
                    Comment = comment,
                    Date = DateTime.UtcNow.AddHours(1),
                    TargetedTo = supervisor.Id,
                    TargetRole = supervisor.UserRoles.FirstOrDefault().Role.Id,
                    TriggeredBy = user.Id,
                    TriggeredByRole = user.UserRoles.FirstOrDefault().Role.Id
                });
                await _unitOfWork.SaveChangesAsync(user.Id);
                if(supervisor != null)
                {
                    //send notification to supervisor
                    var template = Utils.ReadTextFile($"{_env.ContentRootPath}\\wwwroot", "GeneralTemplate.cshtml");
                    var body = string.Format(template, comment, DateTime.Now.Year, "https://celps.nmdpra.gov.ng/content/images/mainlogo.png");

                    Utils.SendMail(_mailSettings.Stringify().Parse<Dictionary<string, string>>(), supervisor.Email, "Vessel Clearance Discharge", body);
                }

                _response = new ApiResponse
                {
                    Message = "Vessel Discharge Clearance Created",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                _response = new ApiResponse { Message = ex.Message };
            }
            return _response;
        }

        public async Task<ApiResponse> GetAllVesselDischargeClearance()
        {
            var aLLVesselDischargeClearance = await _unitOfWork.VesselDischargeClearance.GetAll();

            _response = new ApiResponse
            {
                Data = aLLVesselDischargeClearance,
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };

            return _response;
        }
    }
}
