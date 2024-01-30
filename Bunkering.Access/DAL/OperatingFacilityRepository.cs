using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bunkering.Access.DAL
{
    public class OperatingFacilityRepository : Repository<OperatingFacility>, IOperatingFacility
    {
        public OperatingFacilityRepository(ApplicationContext context) : base(context)
        {
        }

    }

    public interface IOperatingFacility : IRepository<OperatingFacility> { }
}
