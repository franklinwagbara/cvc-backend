using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Dtos
{
    public class CompanyAddressUpsertDto
    {
            //public int CompanyElpsId { get; set; }
            public string address_1 { get; set; }
            public string address_2 { get; set; }
            public string city { get; set; }
            public int country_Id { get; set; }
            public int stateId { get; set; }
            //public string countryName { get; set; }
            //public string stateName { get; set; }
            public string postal_code { get; set; }
            //public string type { get; set; }
      
    }
}
