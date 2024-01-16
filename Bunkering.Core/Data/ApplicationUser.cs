using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class ApplicationUser : IdentityUser
	{
		public int? CompanyId { get; set; }
		public int ElpsId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		[ForeignKey("CompanyId")]
		public Company? Company { get; set; }
		public bool IsActive { get; set; }
		public bool ProfileComplete { get; set; }
		public DateTime? LastJobDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? LastLogin { get; set; }
		public bool IsDeleted { get; set; }
		public int? LocationId { get; set; }
		public int? OfficeId { get; set; }
		public ICollection<ApplicationUserRole> UserRoles { get; set; }

		[ForeignKey(nameof(LocationId))]
		public Location? Location { get; set; }
		[ForeignKey(nameof(OfficeId))]
		public Office? Office { get; set; }
		public string? Signature { get; set; }
    }
}
