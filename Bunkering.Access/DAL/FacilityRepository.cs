using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class FacilityRepository : Repository<Facility>, IFacility
    {
        public FacilityRepository(ApplicationContext context) : base(context){ }
    }

    public interface IFacility : IRepository<Facility> { }
}
