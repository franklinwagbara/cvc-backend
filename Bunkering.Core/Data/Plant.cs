using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public long?  ElpsPlantId { get; set; }
        public long  CompanyElpsId { get; set; }
        public int  PlantType { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<PlantTank> Tanks { get; set; }



    }
}
