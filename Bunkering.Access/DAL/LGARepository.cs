using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class LGARepository : Repository<LGA>, ILGA
    {
        public LGARepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ILGA : IRepository<LGA> { }
}