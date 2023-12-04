using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class PermitRepository : Repository<Permit>, IPermit
    {
        public PermitRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IPermit : IRepository<Permit> { }
}
