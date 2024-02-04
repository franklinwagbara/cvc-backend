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
        //private readonly UserManager<ApplicationUser> _userManager;
        private string LoginUserEmail = string.Empty;
        private readonly ApplicationContext _context;
        //private readonly MessageService _messageService;


        public ProcessingPlantCoQService(IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _httpCxtAccessor = httpCxtAccessor;
            _apiReponse = new ApiResponse();
            //_userManager = userManager;
            _apiReponse = new ApiResponse();
            LoginUserEmail = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);            
            _context = context;
            //_messageService = messageService;
        }


        public async Task<ApiResponse> CreateLiquidStaticCOQ(UpsertPPlantCOQLiquidStaticDto dto)
        {
           
               
                var userId = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);

                if (userId == null)
                {
                    _apiReponse.Message = "Unathorise, this action is restricted to only authorise users";
                    _apiReponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiReponse.Success = false;

                    return _apiReponse;
                }

               

                using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                #region Create Coq  
                var coq = new ProcessingPlantCOQ
                {
                    PlantId = dto.PlantId,
                    ProductId = dto.ProductId,
                    Reference = Utils.GenerateCoQRefrenceCode(),
                    MeasurementSystem = dto.MeasurementSystem,
                    CreatedBy = userId,
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
                    PrevUsBarrelsAt15Degree = dto.PrevUsBarrelsAt15Degree,
                    PrevWTAir = dto.PrevWTAir,
                    DeliveredLongTonsAir = dto.DeliveredLongTonsAir,
                    DeliveredMCubeAt15Degree = dto.DeliveredMCubeAt15Degree,
                    DeliveredMTAir = dto.DeliveredMTAir,
                    DeliveredMTVac = dto.DeliveredMTVac,
                    DeliveredUsBarrelsAt15Degree = dto.DeliveredUsBarrelsAt15Degree,
                };

                _context.ProcessingPlantCOQs.Add(coq);
                _context.SaveChanges();
                #endregion

                #region Create COQ batch Tank
                var batches = new List<ProcessingPlantCOQBatch>();
                var TankReadings = new List<ProcessingPlantCOQBatchTank>();
                foreach (var batch in dto.COQBatches)
                {
                   

                    double longTonsAirB = 0; double mCubeAt15DegreeB = 0; double mTAirB = 0; double mTVacB = 0; double usBarrelsAt15DegreeB = 0;

                    double longTonsAirA = 0; double mCubeAt15DegreeA = 0; double mTAirA = 0; double mTVacA = 0; double usBarrelsAt15DegreeA = 0;

                    foreach (var before in batch.TankBeforeReadings)
                    {
                        //_context.ProcessingPlantCOQTanks.Add(newCoqTank);
                        //_context.SaveChanges();

                        var after = batch.TankAfterReadings.FirstOrDefault(x => x.TankId == before.TankId);

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

                            var newCoqTank = new ProcessingPlantCOQBatchTank
                            {
                                TankId = before.TankId,
                            };

                            newCoqTank.ProcessingPlantCOQTankReadings = newTankReadings;

                            TankReadings.Add(newCoqTank);

                            //before reading of model calculated fees
                            longTonsAirB += newBTankM.LongTonsAir;
                            mCubeAt15DegreeB += newBTankM.MCubeAt15Degree;
                            mTAirB += newBTankM.MTAir;
                            mTVacB += newBTankM.MTVac;
                            usBarrelsAt15DegreeB += newBTankM.UsBarrelsAt15Degree;

                            //after reading of model calculated fees
                            longTonsAirA += newATankM.LongTonsAir;
                            mCubeAt15DegreeA += newATankM.MCubeAt15Degree;
                            mTAirA += newATankM.MTAir;
                            mTVacA += newATankM.MTVac;
                            usBarrelsAt15DegreeA += newATankM.UsBarrelsAt15Degree;
                        }
                    }

                   

                    var newBatch= new ProcessingPlantCOQBatch
                    {
                        ProcessingPlantCOQId = coq.ProcessingPlantCOQId,
                        BatchId = batch.BatchId,
                        ProcessingPlantCOQBatchTanks = TankReadings,
                        SumDiffLongTonsAir = longTonsAirB - longTonsAirA,
                        SumDiffMCubeAt15Degree = mCubeAt15DegreeB - mCubeAt15DegreeA,
                        SumDiffMTAir = mTAirB - mTAirA,
                        SumDiffMTVac = mTVacB - mTVacA,
                        SumDiffUsBarrelsAt15Degree = usBarrelsAt15DegreeB - usBarrelsAt15DegreeA
                    };

                    batches.Add(newBatch);
                }

                _context.ProcessingPlantCOQBatches.AddRange(batches);

                _context.SaveChanges();

                 coq.PrevMCubeAt15Degree  = (coq.PrevUsBarrelsAt15Degree / 6.294);

                coq.PrevMTVac = (coq.PrevMCubeAt15Degree * 769.79) / 1000;
                coq.PrevMTAir = coq.PrevMTVac * coq.PrevWTAir;
                coq.PrevLongTonsAir = coq.PrevMTAir * 0.984206;
                coq.LeftUsBarrelsAt15Degree = coq.PrevUsBarrelsAt15Degree - batches.LastOrDefault().SumDiffUsBarrelsAt15Degree;

                coq.LeftMCubeAt15Degree = coq.LeftUsBarrelsAt15Degree / 6.294;
                coq.LeftMTVac = coq.LeftMCubeAt15Degree * 0.76786;
                coq.LeftMTAir = coq.LeftMTVac * coq.PrevWTAir;
                coq.LeftLongTonsAir = coq.LeftMTAir * 0.984206;

                coq.TotalLongTonsAir = batches.FirstOrDefault().SumDiffLongTonsAir + coq.PrevLongTonsAir - coq.LeftLongTonsAir;
                coq.TotalMCubeAt15Degree = batches.FirstOrDefault().SumDiffMCubeAt15Degree + coq.PrevMCubeAt15Degree - coq.LeftMCubeAt15Degree;
                coq.TotalMTAir = batches.FirstOrDefault().SumDiffMTAir + coq.PrevMTAir - coq.LeftMTAir;
                coq.TotalMTVac = batches.FirstOrDefault().SumDiffMTVac + coq.PrevMTVac - coq.LeftMTVac;
                coq.TotalUsBarrelsAt15Degree = batches.FirstOrDefault().SumDiffUsBarrelsAt15Degree + coq.PrevUsBarrelsAt15Degree - coq.LeftUsBarrelsAt15Degree;
                coq.ShoreFigure = coq.TotalMTVac;

                _context.ProcessingPlantCOQs.Update(coq);
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


        public async Task<ApiResponse> CreateLiquidDynamicCOQ(UpsertPPlantCOQLiquidDynamicDto dto)
        {


            var userId = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);

            if (userId == null)
            {
                _apiReponse.Message = "Unathorise, this action is restricted to only authorise users";
                _apiReponse.StatusCode = HttpStatusCode.BadRequest;
                _apiReponse.Success = false;

                return _apiReponse;
            }



            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                #region Create Coq  
                var coq = new ProcessingPlantCOQ
                {
                    PlantId = dto.PlantId,
                    ProductId = dto.ProductId,
                    Reference = Utils.GenerateCoQRefrenceCode(),
                    MeasurementSystem = dto.MeasurementSystem,
                    CreatedBy = userId,
                    Status = Enum.GetName(typeof(AppStatus), AppStatus.Processing),
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    MeterTypeId = dto.MeterTypeId,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Consignee = dto.Consignee,
                    ConsignorName = dto.ConsignorName,
                    Terminal = dto.Terminal,
                    Destination = dto.Destination,
                    ShipFigure = dto.ShipFigure,
                    ShipmentNo = dto.ShipmentNo,
                    PrevUsBarrelsAt15Degree = dto.PrevUsBarrelsAt15Degree,
                    PrevWTAir = dto.PrevWTAir,
                    PrevMCubeAt15Degree = dto.PrevMCubeAt15Degree,
                    PrevMTVac = dto.PrevMTVac,
                    PrevMTAir = dto.PrevMTVac * dto.PrevWTAir,
                    PrevLongTonsAir = dto.PrevMTVac * dto.PrevWTAir * 0.984206,
                };

                _context.ProcessingPlantCOQs.Add(coq);
                _context.SaveChanges();
                #endregion

                #region Create COQ batch meter
                var batches = new List<ProcessingPlantCOQLiquidDynamicBatch>();
                var MeterReadings = new List<ProcessingPlantCOQLiquidDynamicMeter>();
                foreach (var batch in dto.COQBatches)
                {


                    double longTonsAir = 0; double mCubeAt15Degree = 0; double mTAir = 0; double mTVac = 0; double usBarrelsAt15Degree = 0;

                    foreach (var meter in batch.MeterReadings)
                    {
                       

                        if (meter.MeterBeforReadingDto != null && meter.MeterAfterReadingDto != null)
                        {

                            var newBMeter = new LiquidDynamicMeterReading
                            {
                                MeasurementType = ReadingType.Before,
                                MCube = meter.MeterBeforReadingDto.MCube,
                            };


                            var newAMeter = new LiquidDynamicMeterReading
                            {
                                MeasurementType = ReadingType.After,
                                MCube = meter.MeterAfterReadingDto.MCube,
                            };


                            var newMeterReadings = new List<LiquidDynamicMeterReading>
                            {
                                newBMeter, newAMeter
                            };

                            var newCoqMeter = new ProcessingPlantCOQLiquidDynamicMeter
                            {
                                MeterId = meter.MeterId,
                                Temperature = meter.Temperature,
                                Density = meter.Density,
                                MeterFactor = meter.MeterFactor,
                                Ctl = meter.Ctl,
                                Cpl = meter.Cpl,
                                WTAir = meter.WTAir,
                                LiquidDynamicMeterReadings = newMeterReadings

                            };

                            MeterReadings.Add(newCoqMeter);

                            //reading of model calculated fees
                            longTonsAir += newCoqMeter.LongTonsAir;
                            mCubeAt15Degree += newCoqMeter.MCubeAt15Degree;
                            mTAir += newCoqMeter.MTAir;
                            mTVac += newCoqMeter.MTVac;
                            usBarrelsAt15Degree += newCoqMeter.UsBarrelsAt15Degree;
                        }
                    }

                    var newBatch = new ProcessingPlantCOQLiquidDynamicBatch
                    {
                        ProcessingPlantCOQId = coq.ProcessingPlantCOQId,
                        BatchId = batch.BatchId,
                        ProcessingPlantCOQLiquidDynamicMeters = MeterReadings,
                        SumDiffLongTonsAir = longTonsAir,
                        SumDiffMCubeAt15Degree = mCubeAt15Degree,
                        SumDiffMTAir = mTAir,
                        SumDiffMTVac = mTVac,
                        SumDiffUsBarrelsAt15Degree = usBarrelsAt15Degree
                    };

                    batches.Add(newBatch);
                }

                _context.ProcessingPlantCOQLiquidDynamicBatches.AddRange(batches);

                _context.SaveChanges();

                coq.LeftUsBarrelsAt15Degree = coq.PrevUsBarrelsAt15Degree - batches.LastOrDefault().SumDiffUsBarrelsAt15Degree;

                //coq.LeftMCubeAt15Degree  = coq.LeftUsBarrelsAt15Degree / 6.294;
                //coq.LeftMTVac = coq.LeftMCubeAt15Degree * 0.76786;
                //coq.LeftMTAir = coq.LeftMTVac * coq.PrevWTAir;
                //coq.LeftLongTonsAir = coq.LeftMTAir * 0.984206;

                coq.TotalLongTonsAir = batches.Sum(x => x.SumDiffLongTonsAir) + coq.PrevLongTonsAir - coq.LeftLongTonsAir;
                coq.TotalMCubeAt15Degree = batches.Sum(x => x.SumDiffMCubeAt15Degree) + coq.PrevMCubeAt15Degree - coq.LeftMCubeAt15Degree;
                coq.TotalMTAir = batches.Sum(x => x.SumDiffMTAir) + coq.PrevMTAir - coq.LeftMTAir;
                coq.TotalMTVac = batches.Sum(x => x.SumDiffMTVac) + coq.PrevMTVac - coq.LeftMTVac;
                coq.TotalUsBarrelsAt15Degree = batches.Sum(x => x.SumDiffUsBarrelsAt15Degree) + coq.PrevUsBarrelsAt15Degree - coq.LeftUsBarrelsAt15Degree;
                coq.ShoreFigure = coq.TotalMTVac;

                _context.ProcessingPlantCOQs.Update(coq);
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
