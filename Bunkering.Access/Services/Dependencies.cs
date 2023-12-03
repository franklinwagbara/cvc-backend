using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Bunkering.Access.Services
{
	public static class Dependencies
	{
		public static void Services(this IServiceCollection services)
		{
			services.AddTransient<AppLogger>();
			services.AddScoped<ApplicationContext>();
			services.AddScoped<AppProvessesService>();
			services.AddScoped<AuthService>();
			services.AddTransient<Seeder>();
			services.AddScoped<IElps, ElpsRepostory>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<WorkFlowService>();
			services.AddScoped<AppService>();
			services.AddScoped<MessageService>();
			services.AddScoped<ScheduleService>();
			services.AddScoped<LicenseService>();
			services.AddScoped<CompanyService>();
			services.AddScoped<AppStageDocService>();
			services.AddScoped<PaymentService>();
			services.AddScoped<StaffService>();
			services.AddScoped<LibraryService>();
			services.AddScoped<LocationService>();
			services.AddScoped<OfficeService>();
		}
	}
}
