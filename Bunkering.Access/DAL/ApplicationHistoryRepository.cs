using Bunkering.Access.IContracts;
using Bunkering.Core.Data;


namespace Bunkering.Access.DAL
{
    public class ApplicationHistoryRepository : Repository<ApplicationHistory>, IApplicationHistory
    {
        public ApplicationHistoryRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IApplicationHistory : IRepository<ApplicationHistory> { }
}
