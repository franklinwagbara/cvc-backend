﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string CraetedDate { get; set; }
    }
}
