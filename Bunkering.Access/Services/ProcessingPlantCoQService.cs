using AutoMapper;
using Azure;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Dtos;
using Bunkering.Core.Exceptions;
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
        private readonly IMapper _mapper;
        private ApiResponse _apiReponse;
        private readonly WorkFlowService _flow;
        private readonly UserManager<ApplicationUser> _userManager;
        private string LoginUserEmail = string.Empty;
        private readonly ApplicationContext _context;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly MessageService _messageService;


        public ProcessingPlantCoQService(IUnitOfWork unitOfWork, IHttpContextAccessor httpCxtAccessor, UserManager<ApplicationUser> userManager, ApplicationContext context, IMapper mapper, WorkFlowService workFlowService)
        {
            _httpCxtAccessor = httpCxtAccessor;
            _apiReponse = new ApiResponse();
            _userManager = userManager;
            _apiReponse = new ApiResponse();
            LoginUserEmail = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);            
            _context = context;
            _mapper = mapper;
            _flow = workFlowService;
            _unitOfWork = unitOfWork;
            //_messageService = messageService;
        }

        public async Task<ApiResponse> GetAllCoQ()
        {
            try
            {
                var coqs = await _unitOfWork.ProcessingPlantCoQ.GetAll();

                return new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = coqs.Select(c => new
                    {
                        //PlantName = c.Plant?.Name,
                        //ProductName = c.Product?.Name,
                        PlantId = c.PlantId,
                        ProductId = c.ProductId,
                        Reference = c.Reference,
                        MeasurementSystem = c.MeasurementSystem,
                        StartTime = c.StartTime,
                        EndTime = c.EndTime,
                        ConsignorName = c.ConsignorName,
                        Consignee = c.Consignee,
                        Terminal = c.Terminal,
                        Destination = c.Destination,
                        ShipmentNo = c.ShipmentNo,
                        Id = c.ProcessingPlantCOQId
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = true,
                    Data = null
                };
            }
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
                    Price = dto.Price,
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

                SubmitDocumentDto sDoc = dto.SubmitDocuments.FirstOrDefault() ?? throw new Exception("No documents passed");
                var sDocument = _mapper.Map<PPCOQSubmittedDocument>(sDoc);

                var sDocumentList = new List<PPCOQSubmittedDocument>();

                dto.SubmitDocuments.ForEach(x =>
                {
                    var newSDoc = new PPCOQSubmittedDocument
                    {
                        DocId = x.DocId,
                        FileId = x.FileId,
                        DocName = x.DocName,
                        DocSource = x.DocSource,
                        DocType = x.DocType,
                        ProcessingPlantCOQId = coq.ProcessingPlantCOQId,
                    };

                    sDocumentList.Add(newSDoc);
                });

                _context.PPCOQSubmittedDocuments.AddRange(sDocumentList);
                #endregion

                _context.SaveChanges();

                var submit = await _flow.PPCoqWorkFlow(coq.ProcessingPlantCOQId, Enum.GetName(typeof(AppActions), AppActions.Submit), "COQ Submitted", userId);
                if (submit.Item1)
                {
                    var message = new Message
                    {
                        //ApplicationId = coq.ProcessingPlantCOQId,
                        IsCOQ = false,
                        IsPPCOQ = true,
                        ProcessingCOQId = coq.ProcessingPlantCOQId,
                        Subject = $"COQ with reference {coq.Reference} Submitted",
                        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                        UserId = userId,
                        Date = DateTime.Now.AddHours(1),
                    };

                    _context.Messages.Add(message);
                    _context.SaveChanges();

                    transaction.Commit();

                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    transaction.Rollback();
                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Success = false
                    };

                }

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
                    Price = dto.Price,
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

                SubmitDocumentDto sDoc = dto.SubmitDocuments.FirstOrDefault() ?? throw new Exception("No documents passed");
                var sDocument = _mapper.Map<PPCOQSubmittedDocument>(sDoc);

                var sDocumentList = new List<PPCOQSubmittedDocument>();

                dto.SubmitDocuments.ForEach(x =>
                {
                    var newSDoc = new PPCOQSubmittedDocument
                    {
                        DocId = x.DocId,
                        FileId = x.FileId,
                        DocName = x.DocName,
                        DocSource = x.DocSource,
                        DocType = x.DocType,
                        ProcessingPlantCOQId = coq.ProcessingPlantCOQId,
                    };

                    sDocumentList.Add(newSDoc);
                });

                _context.PPCOQSubmittedDocuments.AddRange(sDocumentList);
                #endregion

                _context.SaveChanges();



                var submit = await _flow.PPCoqWorkFlow(coq.ProcessingPlantCOQId, Enum.GetName(typeof(AppActions), AppActions.Submit), "COQ Submitted", userId);
                if (submit.Item1)
                {
                    var message = new Message
                    {
                        IsCOQ = false,
                        IsPPCOQ = true,
                        ProcessingCOQId = coq.ProcessingPlantCOQId,
                        Subject = $"COQ with reference {coq.Reference} Submitted",
                        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                        UserId = userId,
                        Date = DateTime.Now.AddHours(1),
                    };

                    _context.Messages.Add(message);
                    _context.SaveChanges();

                    transaction.Commit();

                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    transaction.Rollback();
                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Success = false
                    };

                }

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
        }

        public async Task<ApiResponse> GetPPCOQDetailsById(int Id)
        {
            try
            {
                var coq = (await _unitOfWork.ProcessingPlantCoQ
                    .Find(c => c.ProcessingPlantCOQId == Id, "Plant,Product")).FirstOrDefault()
                    ?? throw new NotFoundException($"COQ with Id={Id} does not exist");

                var currentDesk = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(coq.CurrentDeskId)) 
                    ?? throw new NotFoundException($"User could not be found. CurrentDeskID = {coq.CurrentDeskId}");

                var coqDTO = _mapper.Map<ProcessingPlantCOQDTO>(coq);
                coqDTO.CurrentDeskName = $"{currentDesk.LastName}, {currentDesk.FirstName}";
                coqDTO.CurrentDeskEmail = currentDesk.Email;

                return new ApiResponse
                {
                    Data = coqDTO,
                    Message = "Successfull",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };

                //var gastankList = new List<GasTankReadingsPerCoQ>();
                //var liqtankList = new List<LiquidTankReadingsPerCoQ>();
                //var product = new Product();

                //var docs = await _context.PPCOQSubmittedDocuments.FirstOrDefaultAsync(c => c.ProcessingPlantCOQId == coq.ProcessingPlantCOQId);

                //if (coq.ProductId != null)
                //    product = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id.Equals(coq.ProductId));
                //else
                //{
                //    var app = await _unitOfWork.ApplicationDepot.FirstOrDefaultAsync(x => x.Id.Equals(coq.AppId), "Product");
                //    if (app != null)
                //        product = app.Product;
                //}
                //if (product != null)
                //{
                //    switch (product.ProductType.ToLower())
                //    {
                //        case "gas":
                //            //foreach (var item in tanks)
                //            //{
                //            //    var reading = _mapper.Map<List<CreateCoQGasTankDTO>>(item.TankMeasurement);
                //            //    reading.ForEach(x => x.TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == item.TankId).TankName);
                //            //    gastankList.Add(reading);
                //            //}
                //            gastankList = await _context.COQTanks
                //                                .Include(c => c.TankMeasurement).Where(c => c.CoQId == coq.Id)
                //                                .Select(c => new GasTankReadingsPerCoQ
                //                                {
                //                                    TankId = c.TankId,
                //                                    Id = c.Id,
                //                                    TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == c.TankId).TankName,
                //                                    CoQId = c.CoQId,
                //                                    TankMeasurement = c.TankMeasurement.Select(m => new CreateCoQGasTankDTO
                //                                    {
                //                                        LiquidDensityVac = m.LiquidDensityVac,
                //                                        MolecularWeight = m.MolecularWeight,
                //                                        ObservedLiquidVolume = m.ObservedLiquidVolume,
                //                                        ObservedSounding = m.ObservedSounding,
                //                                        ShrinkageFactorLiquid = m.ShrinkageFactorLiquid,
                //                                        ShrinkageFactorVapour = m.ShrinkageFactorVapour,
                //                                        TapeCorrection = m.TapeCorrection,
                //                                        VapourFactor = m.VapourFactor,
                //                                        VapourPressure = m.VapourPressure,
                //                                        VapourTemperature = m.VapourTemperature,
                //                                        TankVolume = m.TankVolume,
                //                                        MeasurementType = m.MeasurementType
                //                                    }).ToList()
                //                                }).ToListAsync();
                //            break;
                //        default:

                //            //foreach (var item in tanks)
                //            //{
                //            //    liqtankList.AddRange(item.TankMeasurement.Select(x => new CreateCoQLiquidTankDto
                //            //    {
                //            //        Density = x.Density,
                //            //        DIP = x.DIP,
                //            //        FloatRoofCorr = x.FloatRoofCorr,
                //            //        GOV = x.GOV,
                //            //        TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == item.TankId).TankName,
                //            //        MeasurementType = x.MeasurementType,
                //            //        Tempearture = x.Tempearture,
                //            //        TOV = x.TOV,
                //            //        VCF = x.VCF,
                //            //        WaterDIP = x.WaterDIP,
                //            //        WaterVolume = x.WaterVolume
                //            //    }).ToList());
                //            //    //var reading = _mapper.Map<List<CreateCoQLiquidTankDto>>(item.TankMeasurement);
                //            //    //reading.ForEach(x => x.TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == item.TankId).TankName);
                //            //    //liqtankList.Add(reading);
                //            //}
                //            liqtankList = await _context.COQTanks
                //                                 .Include(c => c.TankMeasurement).Where(c => c.CoQId == coq.Id)
                //                                 .Select(c => new LiquidTankReadingsPerCoQ
                //                                 {
                //                                     TankId = c.TankId,
                //                                     Id = c.Id,
                //                                     CoQId = c.CoQId,
                //                                     TankName = _context.PlantTanks.FirstOrDefault(x => x.PlantTankId == c.TankId).TankName,
                //                                     TankMeasurement = c.TankMeasurement.Select(m => new CreateCoQLiquidTankDto
                //                                     {
                //                                         Density = m.Density,
                //                                         DIP = m.DIP,
                //                                         FloatRoofCorr = m.FloatRoofCorr,
                //                                         GOV = m.GOV,
                //                                         MeasurementType = m.MeasurementType,
                //                                         Tempearture = m.Tempearture,
                //                                         TOV = m.TOV,
                //                                         VCF = m.VCF,
                //                                         WaterDIP = m.WaterDIP,
                //                                         WaterVolume = m.WaterVolume
                //                                     }).ToList()
                //                                 })
                //                                 .ToListAsync();
                //            break;
                //    }
                //}
                //var dictionary = coq.Stringify().Parse<Dictionary<string, object>>();
                //var coqData = new CoQsDataDTO()
                //{
                //    Vessel = new(),
                //    DateOfSTAfterDischarge = coq.DateOfSTAfterDischarge,
                //    DateOfVesselArrival = coq.DateOfVesselArrival,
                //    DateOfVesselUllage = coq.DateOfVesselUllage,
                //    ArrivalShipFigure = coq.ArrivalShipFigure,
                //    DepotPrice = coq.DepotPrice,
                //    MT_AIR = coq.MT_AIR,
                //    MT_VAC = coq.MT_VAC,
                //    GOV = coq.GOV,
                //    GSV = coq.GSV,
                //    Status = coq.Status,
                //    AppId = coq.AppId,
                //    Reference = coq.Reference,
                //    QuauntityReflectedOnBill = coq.QuauntityReflectedOnBill

                //};
                //if (coq.AppId != null || coq.Reference != null)
                //{
                //    var app = await _unitOfWork.Application.FirstOrDefaultAsync(x => x.Id.Equals(coq.AppId) || x.Reference == coq.Reference, "Facility");
                //    var jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == app.Jetty)?.Name;
                //    if (app != null)
                //    {
                //        coqData.Reference = app.Reference;
                //        coqData.MarketerName = app?.MarketerName ?? string.Empty;
                //        coqData.MotherVessel = app.MotherVessel;
                //        coqData.Jetty = jetty;
                //        coqData.LoadingPort = app.LoadingPort;
                //        coqData.Vessel.Name = app.Facility.Name;
                //        coqData.Vessel.VesselType = app.Facility?.VesselType?.Name ?? string.Empty;
                //        coqData.NominatedSurveyor = (await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(c => c.Id == app.SurveyorId)).Name;
                //        coqData.AppId = app.Id;
                //        coqData.ApplicationTypeId = app.ApplicationTypeId;
                //        coqData.ApplicationType = (await _unitOfWork.ApplicationType.FirstOrDefaultAsync(c => c.Id == app.ApplicationTypeId)).Name;
                //    }
                //}
                //coqData.ProductType = product.ProductType;
                //coqData.CurrentDesk = _userManager.Users.FirstOrDefault(u => u.Id.Equals(coq.CurrentDeskId)).Email;
                //coqData.Plant = _context.Plants.FirstOrDefault(p => p.Id.Equals(coq.PlantId)).Name;

                //if (product.ProductType != null && product.ProductType.ToLower().Equals("gas"))
                //    return new()
                //    {
                //        Success = true,
                //        StatusCode = HttpStatusCode.OK,
                //        Data = new
                //        {
                //            coq = coqData,
                //            tankList = gastankList,
                //            docs
                //        }
                //    };
                //else
                //    return new()
                //    {
                //        Success = true,
                //        StatusCode = HttpStatusCode.OK,
                //        Data = new
                //        {
                //            coq = coqData,
                //            tankList = liqtankList,
                //            docs
                //        }
                //    };
            }
            catch (Exception ex)
            {
                if (ex is ConflictException) return new ApiResponse { Message = ex.Message, StatusCode = HttpStatusCode.Conflict, Success = false };
                else if (ex is NotFoundException) return new ApiResponse { Message = ex.Message, StatusCode = HttpStatusCode.NotFound, Success = false };
                else return new ApiResponse { Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError, Success = false };
            }

        }

        public async Task<ApiResponse> Process(int id, string act, string comment)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LoginUserEmail) ?? throw new Exception($"User with the email={LoginUserEmail} was not found.");
                var coq = await _unitOfWork.ProcessingPlantCoQ.FirstOrDefaultAsync(x => x.ProcessingPlantCOQId.Equals(id)) ?? throw new Exception($"COQ with the ID={id} could not be found.");

                var result = await _flow.PPCoqWorkFlow(id, act, comment);

                if (result.Item1)
                    return new ApiResponse
                    {
                        Data = result.Item1,
                        Message = "COQ Application has been pushed",
                        Success = true,
                        StatusCode = HttpStatusCode.OK
                    };
                else throw new Exception("COQ Application could not be pushed.");
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        public async Task<ApiResponse> CreateLiquidCOQ(LiquidCOQPostDto dto)
        {


            var userId = _httpCxtAccessor.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);

            if (userId == null)
            {
                _apiReponse.Message = "Unathorise, this action is restricted to only authorise users";
                _apiReponse.StatusCode = HttpStatusCode.BadRequest;
                _apiReponse.Success = false;

                return _apiReponse;
            }

            string refNo = Utils.GenerateCoQRefrenceCode();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var dynamic = dto.Dynamic;

                var staticCoq =  await CreateLiquidStatic(dto.Static, userId, refNo);

                if(!staticCoq.Success) {
                    _apiReponse.Message = "Error: static coq not created";
                    _apiReponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiReponse.Success = false;

                    return _apiReponse;
                }

                var dynamicCoq = await CreateLiquidDynamic(dto.Dynamic, userId, refNo);

                if(!dynamicCoq.Success)
                {
                    _apiReponse.Message = "Error: dynamic coq not created";
                    _apiReponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiReponse.Success = false;

                    return _apiReponse;
                }

                var coq = (ProcessingPlantCOQ)staticCoq.Data;

                var submit = await _flow.PPCoqWorkFlow(coq.ProcessingPlantCOQId, Enum.GetName(typeof(AppActions), AppActions.Submit), "COQ Submitted", userId);
                if (submit.Item1)
                {
                    var message = new Message
                    {
                        IsCOQ = false,
                        IsPPCOQ = true,
                        ProcessingCOQId = coq.ProcessingPlantCOQId,
                        Subject = $"COQ with reference {coq.Reference} Submitted",
                        Content = $"COQ with reference {coq.Reference} has been submitted to your desk for further processing",
                        UserId = userId,
                        Date = DateTime.Now.AddHours(1),
                    };

                    _context.Messages.Add(message);
                    _context.SaveChanges();

                    transaction.Commit();

                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
                else
                {
                    transaction.Rollback();
                    return new ApiResponse
                    {
                        Message = submit.Item2,
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Success = false
                    };

                }

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
        }


        private async Task<ApiResponse> CreateLiquidStatic(UpsertPPlantCOQLiquidStaticDto dto, string userId, string refNo)
        {

                #region Create Coq  
                var coq = new ProcessingPlantCOQ
                {
                    PlantId = dto.PlantId,
                    ProductId = dto.ProductId,
                    Reference = refNo,
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
                    Price = dto.Price,
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



                    var newBatch = new ProcessingPlantCOQBatch
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

                coq.PrevMCubeAt15Degree = (coq.PrevUsBarrelsAt15Degree / 6.294);

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

                SubmitDocumentDto sDoc = dto.SubmitDocuments.FirstOrDefault() ?? throw new Exception("No documents passed");
                var sDocument = _mapper.Map<PPCOQSubmittedDocument>(sDoc);

                var sDocumentList = new List<PPCOQSubmittedDocument>();

                dto.SubmitDocuments.ForEach(x =>
                {
                    var newSDoc = new PPCOQSubmittedDocument
                    {
                        DocId = x.DocId,
                        FileId = x.FileId,
                        DocName = x.DocName,
                        DocSource = x.DocSource,
                        DocType = x.DocType,
                        ProcessingPlantCOQId = coq.ProcessingPlantCOQId,
                    };

                    sDocumentList.Add(newSDoc);
                });

                _context.PPCOQSubmittedDocuments.AddRange(sDocumentList);
                #endregion

                _context.SaveChanges();

                                

                return new ApiResponse
                {
                    Message = "COQ submited",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = coq
                };
             

           
        }

        private async Task<ApiResponse> CreateLiquidDynamic(UpsertPPlantCOQLiquidDynamicDto dto, string userId, string refNo)
        {
            #region Create Coq  
            var coq = new ProcessingPlantCOQ
            {
                PlantId = dto.PlantId,
                ProductId = dto.ProductId,
                Reference = refNo,
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
                Price = dto.Price,
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

            SubmitDocumentDto sDoc = dto.SubmitDocuments.FirstOrDefault() ?? throw new Exception("No documents passed");
            var sDocument = _mapper.Map<PPCOQSubmittedDocument>(sDoc);

            var sDocumentList = new List<PPCOQSubmittedDocument>();



            _context.PPCOQSubmittedDocuments.AddRange(sDocumentList);
            #endregion

            _context.SaveChanges();


            return new ApiResponse
            {
                Message = "COQ submited",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = coq
            };

        }
           
    }
}
