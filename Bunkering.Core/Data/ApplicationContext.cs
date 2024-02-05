using Bunkering.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Reflection.Metadata.Ecma335;

namespace Bunkering.Core.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationHistory> ApplicationHistories { get; set; }
        public DbSet<AppFee> AppFees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<ApplicationDepot> ApplicationDepots { get; set; }
        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CoQ> CoQs { get; set; }
        public DbSet<COQHistory> COQHistories { get; set; }
        public DbSet<COQCertificate> COQCertificates { get; set; }
        public DbSet<COQTank> COQTanks { get; set; }
        public DbSet<CoQReference> CoQReferences { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<ExtraPayment> ExtraPayments { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityType> FacilityTypes { get; set; }
        public DbSet<FacilityTypeDocument> FacilityTypeDocuments { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Jetty> Jetties { get; set; }
        public DbSet<LGA> LGAs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<LiquidDynamicMeterReading> LiquidDynamicMeterReadings { get; set; }
        public DbSet<NominatedSurveyor> NominatedSurveyors { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantTank> PlantTanks { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<SubmittedDocument> SubmittedDocuments { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<ValidatiionResponse> ValidatiionResponses { get; set; }
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<FacilitySource> FacilitySources { get; set; }
        public DbSet<VesselType> VesselTypes { get; set; }
        public DbSet<vAppVessel> vAppVessel { get; set; }
        public DbSet<vAppPayment> vAppPayment { get; set; }
        public DbSet<vAppUser> vAppUsers { get; set; }
        public DbSet<vFacilityPermit> vFacilityPermit { get; set; }
        public DbSet<vPayment> vPayment { get; set; }
        public DbSet<vDebitNote> VDebitNotes { get; set; }
        public DbSet<PlantFieldOfficer> PlantFieldOfficers { get; set; }
        public DbSet<ProcessingPlantCOQ> ProcessingPlantCOQs { get; set; }
        public DbSet<ProcessingPlantCOQBatch> ProcessingPlantCOQBatches { get; set; }
        public DbSet<ProcessingPlantCOQLiquidDynamicBatch> ProcessingPlantCOQLiquidDynamicBatches { get; set; }
        public DbSet<ProcessingPlantCOQLiquidDynamicMeter> ProcessingPlantCOQLiquidDynamicMeters { get; set; }
        public DbSet<ProcessingPlantCOQBatchTank> ProcessingPlantCOQBatchTanks { get; set; }
        public DbSet<ProcessingPlantCOQTankReading> ProcessingPlantCOQTankReadings { get; set; }
        public DbSet<JettyFieldOfficer> JettyFieldOfficers { get; set; }
        public DbSet<ApplicationSurveyor> ApplicationSurveyors { get; set; }
        public DbSet<MeasurementType> MeasurementTypes { get; set; }
        public DbSet<TankMeasurement> TankMeasurements { get; set; }
        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }
        public DbSet<VesselDischargeClearance> VesselDischargeClearances {  get; set; }
        public DbSet<MeterType> MeterType {  get; set; }
        public DbSet<DippingMethod> DippingMethod {  get; set; }
        public DbSet<TransferDetail> TransferDetail {  get; set; }
        public DbSet<TransferRecord> TransferRecord {  get; set; }
        public DbSet<OperatingFacility> OperatingFacility { get; set; }

        //public virtual DbContextOptionsBuilder EnableDetailedErrors(bool detailedErrorsEnabled = true) => detailedErrorsEnabled;
        //public virtual DbContextOptionsBuilder EnableDetailedErrors(bool detailedErrorEnabled) => DbContextOptionsBuilder;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            //builder.Entity<Facility>().HasOne(x => x.State).WithMany().HasPrincipalKey(x => x.Code).HasForeignKey(x => x.StateCode);
            //builder.Entity<Application>().HasOne(x => x.StorageSalesLicenseFees).WithMany().HasPrincipalKey(x => x.Category).HasForeignKey(x => x.StorageCapacity);
        }

        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }

        private void OnBeforeSaveChanges(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                ChangeTracker.DetectChanges();
                var auditEntries = new List<AuditEntry>();

                var entities = ChangeTracker.Entries()
                    .Where(x => x.State != EntityState.Added
                    && x.State != EntityState.Unchanged
                    && x.State != EntityState.Detached).ToList();

                foreach (var entry in entities)
                {
                    //if (entry.Entity is Audit || entry.State.Equals(EntityState.Detached) || entry.State.Equals(EntityState.Unchanged))
                    //	continue;
                    var auditEntry = new AuditEntry(entry);
                    auditEntry.TableName = entry.Entity.GetType().Name;
                    auditEntry.UserId = userId;
                    auditEntries.Add(auditEntry);
                    foreach (var property in entry.Properties)
                    {
                        string propName = property.Metadata.Name;
                        if (property.Metadata.IsPrimaryKey())
                        {
                            auditEntry.KeyValues[propName] = property.CurrentValue;
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                auditEntry.AuditType = AuditType.Create;
                                auditEntry.NewValues[propName] = property.CurrentValue;
                                break;
                            case EntityState.Deleted:
                                auditEntry.AuditType = AuditType.Delete;
                                auditEntry.OldValues[propName] = property.OriginalValue;
                                break;
                            case EntityState.Modified:
                                if (property.IsModified)
                                {
                                    auditEntry.ChangedColumns.Add(propName);
                                    auditEntry.AuditType = AuditType.Update;
                                    auditEntry.OldValues[propName] = property.OriginalValue;
                                    auditEntry.NewValues[propName] = property.CurrentValue;
                                }
                                break;
                        }
                    }
                    if (auditEntries.Any())
                    {
                        var logs = auditEntries.Select(x => x.ToAudit());
                        AuditLogs.AddRange(logs);
                    }
                }
            }
        }
    }

    public class ApplicationContextOptionsBuilder
    {
    }
}
