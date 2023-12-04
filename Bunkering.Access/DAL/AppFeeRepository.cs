using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class AppFeeRepository : Repository<AppFee>, IAppFee
    {
        public AppFeeRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IAppFee : IRepository<AppFee> { }
}
