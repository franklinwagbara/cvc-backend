using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Dtos
{
    public class LiquidCOQPostDto
    {
        public UpsertPPlantCOQLiquidStaticDto Static { get; set; }
        public UpsertPPlantCOQLiquidDynamicDto Dynamic { get; set; }
    }
}
