﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class StatesViewModel
    {
        public int Id { get; set; }    
        public string Name { get; set; }
        public string Code { get; set; }

        public int CountryID { get; set; }

        public string CountryName { get; set; }

        
    }
}
