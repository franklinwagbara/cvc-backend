using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class COQHistoryRepository : Repository<COQHistory>, ICOQHistory
    {
        public COQHistoryRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ICOQHistory: IRepository<COQHistory>{}
}