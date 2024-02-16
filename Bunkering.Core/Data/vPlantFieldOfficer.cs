using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class vPlantFieldOfficer
    {
        public int ID { get; set; }
        public int PlantID { get; set; }
        public string OfficerID { get; set; }
        public string DepotName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string LastName { get; set; }
    }
}
