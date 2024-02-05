using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class PPCOQHistoryRepository : Repository<PPCOQHistory>, IPPCOQHistory
    {
        public PPCOQHistoryRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IPPCOQHistory: IRepository<PPCOQHistory> { }
}
