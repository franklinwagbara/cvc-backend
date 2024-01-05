using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.DAL
{
    public class PlantTankRepository:  Repository<PlantTank>, IPlantTank
    {
        public PlantTankRepository(ApplicationContext context) : base(context) { }
    }

    public interface IPlantTank : IRepository<PlantTank> { }
}
