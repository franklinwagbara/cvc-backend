using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public int ElpsId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string Email { get; set; }
		public string RoleId { get; set; }
		public int LocationId { get; set; }
		public int OfficeId { get; set; }
		public string? Phone { get; set; }
		public bool IsActive { get; set; }
	}
}
