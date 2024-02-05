using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Microsoft.AspNetCore.Identity;

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
            var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();
            var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var typeId = unitOfWork.ApplicationType
                .FirstOrDefaultAsync(x => x.Name.Equals(Enum.GetName(typeof(AppTypes), AppTypes.DebitNote)))
                .GetAwaiter().GetResult()?.Id;
            var notes = unitOfWork.vDebitNote
                .Find(x => x.Status == Enum
                .GetName(typeof(AppStatus), AppStatus.PaymentPending)
                && x.ApplicationTypeId == typeId && x.TransactionDate.AddDays(21) > DateTime.UtcNow.AddHours(1))
                .GetAwaiter().GetResult().ToList();

            foreach (var c in notes)
            {
                var app = unitOfWork.Application.FirstOrDefaultAsync(x => x.Id == c.ApplicationId).Result;
                //var user =  userManger.FindByEmailAsync(c.CompanyEmail).GetAwaiter().GetResult();
                //if (user is null)
                //{
                //    continue;
                //}
                //user.IsDefaulter = true;
                paymentService?.GenerateDemandNotice(c.COQId).Wait();
            }
            unitOfWork.SaveChangesAsync("system").Wait();
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