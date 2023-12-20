using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class ApplicationDepotRepository : Repository<ApplicationDepot>, IApplicationDepot
    {
        public ApplicationDepotRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
