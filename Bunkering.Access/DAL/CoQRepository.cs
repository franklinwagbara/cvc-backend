using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class CoQRepository : Repository<CoQ>, ICoQ
    {
        public CoQRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ICoQ : IRepository<CoQ> { }
}
