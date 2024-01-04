using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class PlantTank
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string TankName { get; set; }
        public string Product { get; set; }
        public decimal Capacity { get; set; }
        public string Position { get; set; }
        public bool IsDeleted { get; set; }
    }
}
