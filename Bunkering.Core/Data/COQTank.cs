using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class COQTank
    {
        public int Id { get; set; }
        public int CoQId { get; set; }
        public string TankName { get; set; }
        public ICollection<TankMeasurement> TankMeasurement { get; set; }
    }
}
