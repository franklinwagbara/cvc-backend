using AutoMapper;
using Azure;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Dtos;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bunkering.Access.Services
{
    public class ProcessingPlantCoQService
    {
        private readonly IHttpContextAccessor _httpCxtAccessor;
        private ApiResponse _apiReponse;
        private readonly UserManager<ApplicationUser> _userManager;
        private string LoginUserEmail = string.Empty;
        private readonly ApplicationContext _context;
        private readonly MessageService _messageService;


        public ProcessingPlantCoQService(IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, 
           ApplicationContext context, MessageService messageService)
        {
            _httpCxtAccessor = httpCxtAccessor;
            _apiReponse = new ApiResponse();
            _userManager = userManager;
            _apiReponse = new ApiResponse();
            LoginUserEmail = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);            
            _context = context;
            _messageService = messageService;
        }


        public async Task<ApiResponse> CreateLiquidStaticCOQ(UpsertPPlantCOQLiquidStaticDto dto)
        {
            var user = await _userManager.FindByEmailAsync(LoginUserEmail);

            if(user == null)
            {
                _apiReponse.Message = "Unathorise, this action is restricted to only authorise users";
                _apiReponse.StatusCode = HttpStatusCode.BadRequest;
                _apiReponse.Success = false;

                return _apiReponse;
            }
           
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                #region Create Coq  
                var coq = new ProcessingPlantCOQ
                {
                    PlantId = dto.PlantId,
                    ProductId = dto.ProductId,
                    Reference = Utils.GenerateCoQRefrenceCode(),
                    MeasurementSystem = dto.MeasurementSystem,
                    CreatedBy = user.Id,
                    Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing),
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    DipMethodId = dto.DipMethodId,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Consignee = dto.Consignee,
                    ConsignorName = dto.ConsignorName,
                    Terminal = dto.Terminal,
                    Destination = dto.Destination,
                    ShipFigure = dto.ShipFigure,
                    ShipmentNo = dto.ShipmentNo,
                    ShoreFigure = dto.ShipFigure,
                    PrevUsBarrelsAt15Degree = dto.PrevUsBarrelsAt15Degree,
                    PrevWTAir = dto.PrevWTAir,
                    DeliveredLongTonsAir = dto.DeliveredLongTonsAir,
                    DeliveredMCubeAt15Degree = dto.DeliveredMCubeAt15Degree,
                    DeliveredMTAir = dto.DeliveredMTAir,
                    DeliveredMTVac = dto.DeliveredMTVac,
                    DeliveredUsBarrelsAt15Degree = dto.DeliveredUsBarrelsAt15Degree,
                };

                _context.ProcessingPlantCOQS.Add(coq);
                _context.SaveChanges();
                #endregion

                #region Create COQ Tank
                var TankReadings = new List<ProcessingPlantCOQTank>();

                foreach (var before in dto.TankBeforeReadings)
                {                    
                    //_context.ProcessingPlantCOQTanks.Add(newCoqTank);
                    //_context.SaveChanges();

                    var after = dto.TankAfterReadings.FirstOrDefault(x => x.TankId == before.TankId);

                    if (after.TankReading != null && before.TankReading != null)
                    {
                        var b = before.TankReading;
                        var a = after.TankReading;

                        
                        var newBTankM = new ProcessingPlantCOQTankReading
                        {
                            //ProcessingPlantCOQTankId = newCoqTank.ProcessingPlantCOQTankId,
                            MeasurementType = ReadingType.Before,
                            ReadingM = b.ReadingM,
                            Temperature = b.Temperature,
                            Density = b.Density,
                            SpecificGravityObs = b.SpecificGravityObs,
                            VolumeCorrectionFactor = b.VolumeCorrectionFactor,
                            BarrelsAtTankTables = b.BarrelsAtTankTables,
                            WTAir = b.WTAir,
                        };
                        

                        var newATankM = new ProcessingPlantCOQTankReading
                        {
                            //ProcessingPlantCOQTankId = newCoqTank.ProcessingPlantCOQTankId,
                            MeasurementType = ReadingType.After,
                            ReadingM = a.ReadingM,
                            Temperature = a.Temperature,
                            Density = a.Density,
                            SpecificGravityObs = a.SpecificGravityObs,
                            VolumeCorrectionFactor = a.VolumeCorrectionFactor,
                            BarrelsAtTankTables = a.BarrelsAtTankTables,
                            WTAir = a.WTAir,
                        };

                        var newTankReadings = new List<ProcessingPlantCOQTankReading>
                        {
                            newBTankM, newATankM
                        };

                        var newCoqTank = new ProcessingPlantCOQTank
                        {
                            ProcessingPlantCOQId = coq.ProcessingPlantCOQId,
                            TankId = before.TankId,
                            SumDiffLongTonsAir = newBTankM.LongTonsAir - newATankM.LongTonsAir,
                            SumDiffMCubeAt15Degree = newBTankM.MCubeAt15Degree - newATankM.MCubeAt15Degree,
                            SumDiffMTAir = newBTankM.MTAir - newATankM.MTAir,
                            SumDiffMTVac = newBTankM.MTVac - newATankM.MTVac,
                            SumDiffUsBarrelsAt15Degree = newBTankM.UsBarrelsAt15Degree - newATankM.UsBarrelsAt15Degree
                        };

                        newCoqTank.ProcessingPlantCOQTankReadings = newTankReadings;

                        TankReadings.Add(newCoqTank);
                    }
                }

                _context.ProcessingPlantCOQTanks.AddRange(TankReadings);

                coq.TotalLongTonsAir = TankReadings.Sum(x => x.SumDiffLongTonsAir) - coq.LeftLongTonsAir;
                coq.TotalMCubeAt15Degree = TankReadings.Sum(x => x.SumDiffMCubeAt15Degree) - coq.LeftMCubeAt15Degree;
                coq.TotalMTAir = TankReadings.Sum(x => x.SumDiffMTAir) - coq.LeftMTAir;
                coq.TotalMTVac = TankReadings.Sum(x => x.SumDiffMTVac) - coq.LeftMTVac;
                coq.TotalUsBarrelsAt15Degree = TankReadings.Sum(x => x.SumDiffUsBarrelsAt15Degree) - coq.TotalUsBarrelsAt15Degree;

                _context.ProcessingPlantCOQS.Update(coq);
                #endregion

                #region Document Submission

                //SubmitDocumentDto sDoc = model.SubmitDocuments.FirstOrDefault();
                //var sDocument = _mapper.Map<SubmittedDocument>(sDoc);

                //var sDocumentList = new List<SubmittedDocument>();

                //dto.SubmitDocuments.ForEach(x =>
                //{
                //    var newSDoc = new SubmittedDocument
                //    {
                //        DocId = x.DocId,
                //        FileId = x.FileId,
                //        DocName = x.DocName,
                //        DocSource = x.DocSource,
                //        DocType = x.DocType,
                //        ApplicationId = coq.ProcessingPlantCOQId,
                //    };

                //    sDocumentList.Add(newSDoc);
                //});

                //_context.SubmittedDocuments.AddRange(sDocumentList);
                #endregion

                _context.SaveChanges();



                //var submit = await _flow.CoqWorkFlow(coq.Id, Enum.GetName(typeof(AppActions), AppActions.Submit), "COQ Submitted", user.Id);
                //if (submit.Item1)
                //{
                //    var message = new Message
                //    {
                //        ApplicationId = coq.Id,
                //        Subject = $"COQ with reference {coq.Reference} Submitted",
                //        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                //        UserId = user.Id,
                //        Date = DateTime.Now.AddHours(1),
                //    };

                //    _context.Messages.Add(message);
                //    _context.SaveChanges();

                //    transaction.Commit();

                //    return new ApiResponse
                //    {
                //        Message = submit.Item2,
                //        StatusCode = HttpStatusCode.OK,
                //        Success = true
                //    };
                //}
                //else
                //{
                //    transaction.Rollback();
                //    return new ApiResponse
                //    {
                //        Message = submit.Item2,
                //        StatusCode = HttpStatusCode.NotAcceptable,
                //        Success = false
                //    };

                //}

            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new ApiResponse
                {
                    Message = $"An error occur, COQ not created: {ex.Message}",
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            transaction.Commit();
            return new ApiResponse
            {
                Message = "COQ created successfully",
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

     
    }
}
