using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public string? Address { get; set; }
        public int? StateId { get; set; }
        public string? YearIncorporated { get; set; }
        public int? CountryId { get; set; }
        public string? RcNumber { get; set; }
        public string? TinNumber { get; set; }
    }
}
