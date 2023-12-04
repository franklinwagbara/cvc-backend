﻿using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.DAL
{
	public class FacilitySourceRepository : Repository<FacilitySource>, IFacilitySource
	{
		public FacilitySourceRepository(ApplicationContext context) : base(context)
		{
		}
	}
}
