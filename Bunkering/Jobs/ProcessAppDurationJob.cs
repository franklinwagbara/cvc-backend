using System;
using System.Threading;
using System.Threading.Tasks;
using Bunkering.Access.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bunkering.Jobs;
public class ProcessAppDurationJob : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProcessAppDurationJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CallService, null, TimeSpan.Zero, TimeSpan.FromHours(15));

        return Task.CompletedTask;
    }

    private void CallService(object state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var appService = scope.ServiceProvider.GetRequiredService<AppService>();

            appService.GetLongProcessingApps();
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

public class NavalReportJob : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NavalReportJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Calculate the time until 8 AM.
        DateTime now = DateTime.Now;
        DateTime execTime = DateTime.Today.AddHours(23);
        if (now > execTime)
        {
            execTime = execTime.AddDays(1);
        }

        TimeSpan timeUntilExecution = execTime - now;

        // Set up the timer to call the CallService method every 24 hours, starting at 23:00.
        _timer = new Timer(CallService!, null, timeUntilExecution, TimeSpan.FromHours(24));

        return Task.CompletedTask;
    }

    private void CallService(object state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            //var navalService = scope.ServiceProvider.GetRequiredService<NavalReportService>();

            // Call your service here.
            //navalService.SendReport();
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
