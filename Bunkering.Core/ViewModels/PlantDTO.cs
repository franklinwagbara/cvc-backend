using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class PlantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public long FacilityElpsId { get; set; }
        public long CompanyElpsId { get; set; }
        public PlantType PlantType { get; set; }
        public List<PlantTankDTO> Tanks { get; set; }
    }

    public class PlantTankDTO
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string TankName { get; set; }
        public string Product { get; set; }
        public decimal Capacity { get; set; }
        public string Position { get; set; }
    }

    public enum PlantType
    {
        Processing = 2,
        Depot = 1
    }
}
