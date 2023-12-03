using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class FacilitySourceDto
    {
        public int FacilityTypeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }
        public int StateId { get; set; }
        public int LgaId { get; set; }
    }
}
