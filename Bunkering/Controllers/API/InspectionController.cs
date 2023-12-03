using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    [Route("api/bunkering/[controller]")]
    [ApiController]
    public class InspectionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public InspectionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /// <summary>
        /// This endpoint returns a mode of pending inspection, have application category, user email, and stage for a given inspector/staff 
        /// </summary>
        /// <returns>Returns a modes of pending inspections</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Inspection/pending
        /// 
        /// </remarks>
        /// <param name="email">The inspector/Reviewer email to fetch inspection for</param>
        /// <param name="code">Uniqe key for the request. use any (eyJJc3N1ZXIiOiJBRERPTlNBVVRPR0FTVVRJTElTQVRJT04ifQ, eyJJc3N1ZXIiOiJBVVRPR0FTIn0, eyJhbGciOiJIUzI1NiJ9)</param>
        /// <response code="200">Returns a list of all pending inspections for a user </response>
        /// <response code="404">Returns not found inspection </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(InspectionResponse), 200)]
        [ProducesResponseType(typeof(InspectionResponse), 404)]
        [ProducesResponseType(typeof(InspectionResponse), 4041)]
        [ProducesResponseType(typeof(InspectionResponse), 500)]
        [Produces("application/json")]
        [Route("pending")]
        [HttpGet]
        public async Task<IActionResult> Pending(string email, string code)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var schedules = await _unitOfWork.Appointment.Find(x => user.Id.Equals(x.ScheduledBy) && x.IsAccepted && x.IsAccepted && x.ExpiryDate >= DateTime.UtcNow.AddHours(1), "Application.User,Application.Facility.VesselType,Application.ApplicationType");
                var resposne = new InspectionResponse(); ;
                if (schedules == null || schedules.Count() == 0)
                    return NotFound(new InspectionResponse
                    {
                        message = "No pending schedule was found for the user.",
                        responsecode = "01",
                        success = false
                    });
                else if (string.IsNullOrEmpty(code))
                    return NotFound(new InspectionResponse
                    {
                        message = "Access was denied",
                        responsecode = "02",
                        success = false
                    });
                else
                    return Ok(new InspectionResponse
                    {
                        message = $"{schedules.Count()} schedules found",
                        responsecode = "00",
                        success = true,
                        data = schedules.Select(s => new InspectionDTO
                        {
                            //Address = s.Application.Facility.Address,
                            ApplicationId = s.ApplicationId,
                            ApplicationType = s.Application.ApplicationType.Name,
                            FacilityName = s.Application.Facility.Name,
                            VesselType = s.Application.Facility.VesselType.Name,
                            Reference = s.Application.Reference
                        })
                    });

            }
            catch (Exception ex)
            {
                return BadRequest(
                    new InspectionResponse
                    {
                        message = "An error occured, pls try again or contact. support",
                        responsecode = "03",
                        success = false,
                    });
            }
        }

        /// <summary>
        /// This endpoint returns a success message on successful addition of inspection form 
        /// </summary>
        /// <returns>Returns a message on inspection update</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Inspection/post-inspection
        /// 
        /// </remarks>
        /// <param name="model">The inspection form to be saved for the applictaion</param>
        /// <response code="200">Returns a list of all pending inspections for a user </response>
        /// <response code="404">Returns not found inspection </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(InspectionResponse), 200)]
        [ProducesResponseType(typeof(InspectionResponse), 404)]
        [ProducesResponseType(typeof(InspectionResponse), 4041)]
        [ProducesResponseType(typeof(InspectionResponse), 500)]
        [Produces("application/json")]
        [Route("post-inspection")]
        [HttpPost]
        public async Task<IActionResult> SaveInspection(InspectionReportModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.ScheduledBy);
                var schedules = await _unitOfWork.Appointment.Find(x => user.Id.Equals(x.ScheduledBy) && x.IsAccepted && x.IsAccepted, "Application.User,Application.Facility.VesselType,Application.ApplicationType");
                var resposne = new InspectionResponse();
                if (schedules == null || schedules.Count() == 0)
                    return NotFound(new InspectionResponse
                    {
                        message = "No pending schedule was found for the user.",
                        responsecode = "01",
                        success = false
                    });
                else if (string.IsNullOrEmpty(model.Code))
                    return NotFound(new InspectionResponse
                    {
                        message = "Access was denied",
                        responsecode = "02",
                        success = false
                    });
                else
                {
                    var inspectionForm = await _unitOfWork.Inspection.FirstOrDefaultAsync(x => x.ApplicationId.Equals(model.ApplicationId) && x.Application.Reference.Equals(model.Reference), "Application");
                    if (inspectionForm == null)
                    {
                        await _unitOfWork.Inspection.Add(new Inspection
                        {
                            ApplicationId = model.ApplicationId,
                            Comment = model.Comment,
                            IndicationOfSImilarFacilityWithin2km = model.IndicationOfSImilarFacilityWithin2km,
                            InspectionDocument = model.InspectionDocument,
                            ScheduledBy = user.Id,
                            SietJettyTopographicSurvey = model.SietJettyTopographicSurvey,
                            SiteDrainage = model.SiteDrainage,
                        });
                        await _unitOfWork.SaveChangesAsync(user.Id);
                    }
                    return Ok(new InspectionResponse
                    {
                        message = $"Inspection form added successfully.",
                        responsecode = "00",
                        success = true
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new InspectionResponse
                    {
                        message = "An error occured, pls try again or contact. support",
                        responsecode = "03",
                        success = false,
                    });
            }
        }
    }
}
