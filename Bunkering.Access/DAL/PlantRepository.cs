using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.DAL
{
    public class PlantRepository: Repository<Plant>, IPlant
    {
        public PlantRepository(ApplicationContext context):base(context) { }
     }


    public interface IPlant : IRepository<Plant>{}
}
