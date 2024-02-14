using Azure;
using Bunkering.Access;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;

namespace Bunkering.Jobs;

public class DemandNoticeJob : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DemandNoticeJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Calculate the time until 8 AM.
        DateTime now = DateTime.UtcNow.AddHours(1);
        DateTime eightAM = DateTime.Today.AddHours(8);
        if (now > eightAM)
        {
            eightAM = eightAM.AddDays(1);
        }

        TimeSpan timeUntilEight = eightAM - now;

        // Set up the timer to call the CallService method every 24 hours, starting at 8 AM.
        //_timer = new Timer(CallService, null, timeUntilEight, TimeSpan.FromHours(24));
        _timer = new Timer(CallService, null, 1, TimeSpan.FromHours(24).Milliseconds);

        return Task.CompletedTask;
    }

    private void CallService(object? state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var typeId = unitOfWork.ApplicationType
                .FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)))
                .GetAwaiter().GetResult()?.Id;
            var notes = unitOfWork.vDebitNote.Find(x => x.Status == Enum.GetName(typeof(AppStatus),AppStatus.PaymentPending)
                && x.ApplicationTypeId == typeId && x.TransactionDate.AddDays(21) > DateTime.UtcNow.AddHours(1))
                .GetAwaiter().GetResult().ToList();

            foreach (var c in notes)
                GenerateDemandNotice(unitOfWork, c.COQId, userManger).Wait();
        }
    }

    private async Task GenerateDemandNotice(IUnitOfWork unitOfWork, int id, UserManager<ApplicationUser> userManager)
    {
        try
        {
            var coqRef = unitOfWork.CoQReference.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.IsDeleted == false, "DepotCoQ.Application.User.Company,ProcessingPlantCOQ.Plant").Result;

            var plant = coqRef.PlantCoQId == null ? coqRef.DepotCoQ.Plant : coqRef.ProcessingPlantCOQ.Plant;
            var user = userManager.Users.FirstOrDefault(u => u.ElpsId.Equals(plant.CompanyElpsId));

            if (coqRef == null)
                return;
            else
            {
                var appType = unitOfWork.ApplicationType.FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote))).Result;
                var payment = unitOfWork.Payment.FirstOrDefaultAsync(x => x.ApplicationTypeId.Equals(appType.Id) && x.COQId.Equals(coqRef.Id), "DemandNotices").Result;

                if (payment == null)
                    return;
                else
                {
                    double amount = 0;
                    if (payment.DemandNotices is not null && payment.DemandNotices.LastOrDefault().AddedDate.AddDays(21) > DateTime.UtcNow.AddHours(1))
                        amount = (payment.Amount + payment.DemandNotices.Sum(a => a.Amount)) * 0.10;
                    else
                        amount = payment.Amount * 0.10;

                    if(amount > 0)
                    {
                        var description = $"Demand notice for non-payment of Debit note generated for {coqRef.ProcessingPlantCOQ.Plant.Name} after 21 days as regulated";

                        unitOfWork.DemandNotice.Add(new DemandNotice
                        {
                            Amount = amount,
                            AddedBy = "system",
                            AddedDate = DateTime.UtcNow.AddHours(1),
                            DebitNoteId = coqRef.Id,
                            Description = description,
                            Reference = ""
                        }).Wait();

                        //set company as a defaulter
                        if (!user.IsDefaulter)
                        {
                            user.IsDefaulter = true;
                            user.IsCleared = false;
                            userManager.UpdateAsync(user).Wait();
                        }

                        //set plant or depot as defaulter
                        if(!plant.IsDefaulter)
                        {
                            plant.IsDefaulter = true;
                            plant.IsCleared = false;
                            unitOfWork.Plant.Update(plant).Wait();
                        }
                        unitOfWork.SaveChangesAsync("system").Wait();
                    }                    
                }
            }
        }
        catch (Exception ex)
        {
            //_logger.LogRequest($"An error {ex.Message} occurred while trying to generate extra payment RRR for this application by {User}", true, directory);
            //_response = new ApiResponse { Message = "An error occurred while generating this extra payment RRR. Please try again or contact support.", StatusCode = HttpStatusCode.InternalServerError };
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}