﻿using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Access.Query;
using Bunkering.Core.Data;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<AppFeeService>();
            services.AddScoped<JettyService>();
            services.AddScoped<DepotOfficerService>();
            services.AddScoped<JettyOfficerService>();
            services.AddScoped<DepotService>();
            services.AddScoped<NominatedSurveyorService>();
            services.AddScoped<CoQService>();
            services.AddScoped<ProcessingPlantCoQService>();
            services.AddScoped<ApplicationQueries>();
            services.AddScoped<RoleService>();
            services.AddScoped<PlantService>();
            services.AddScoped<ProductService>();
            services.AddScoped<EmailConfigurationService>();
            services.AddScoped<VesselDischargeClearanceService>();
            services.AddScoped<ShipToShipService>();
            services.AddScoped<DippingMethodService>();
            services.AddScoped<MeterTypeService>();
            services.AddScoped<MeterService>();
            services.AddScoped<BatchService>();
            services.AddScoped<SourceRecipientVesselService>();
            services.AddScoped<OperatingFacilityService>();


            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
