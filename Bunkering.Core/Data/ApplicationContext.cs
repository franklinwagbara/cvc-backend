﻿using Bunkering.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
		public DbSet<Audit> AuditLogs { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<ExtraPayment> ExtraPayments { get; set; }
		public DbSet<Facility> Facilities { get; set; }
		public DbSet<FacilityType> FacilityTypes { get; set; }
		public DbSet<FacilityTypeDocument> FacilityTypeDocuments { get; set; }
		public DbSet<Office> Offices { get; set; }
		public DbSet<Inspection> Inspections { get; set; }
		public DbSet<LGA> LGAs { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Payment> Payments { get; set; }
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
