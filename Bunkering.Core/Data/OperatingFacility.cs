﻿using Bunkering.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class OperatingFacility
    {
        public int Id { get; set; }
        public string CompanyEmail { get; set; }
        public string Name { get; set; }
    }
}
