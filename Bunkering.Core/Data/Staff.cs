﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
	public class Staff
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		public string PhoneNo { get; set; }
		public bool IsDeleted { get; set; }
	}
}
