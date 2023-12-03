using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class TankRepository : Repository<Tank>, ITank
    {
        public TankRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ITank : IRepository<Tank> { }
}
